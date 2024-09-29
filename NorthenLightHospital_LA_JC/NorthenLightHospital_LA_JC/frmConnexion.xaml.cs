using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NorthenLightHospital_LA_JC
{
    /// <summary>
    /// AUTEUR:         Lion Arar
    /// 
    /// Mise à Jour:    YY/MM/DD par
    ///                 24/09/23
    ///                 24/09/28 par Jean Couturier
    ///                 
    /// Objectif: 
    ///     Formulaire de connexion. Suivant le titre de l'utilisateur le form correspondant sera afficher
    ///     quand le form afficher est fini d'utiliser le form de connexion se re-affiche pret 
    ///     pour une nouvelle connexion.
    /// </summary>
    public partial class frmConnexion : Window
    {
        Gestion gestion = new Gestion();
        User currentUser = new User();

        frmAdmin frmAdmin;
        frmMedecin frmMedecin;
        frmPrepose frmPrepose;

        public frmConnexion()
        {
            InitializeComponent();
            username.Focus();
        }

        /// <summary>
        /// Procédure de mis a jour a la listeUtilisateur
        /// Si un admin se connect et ajoute ou modifie un medecin
        /// la liste sera automatique mis a jour grace au dbContext
        /// </summary>
        private void Window_Activated(object sender, EventArgs e)
        {
            gestion.majListMedecin();
        }

        /// <summary>
        /// Evenement qui gèere la validation des idantifiants saisis.
        /// </summary>
        private void btnCo_Click(object sender, RoutedEventArgs e)
        {
            if (validationEmpty()) {
                currentUser = gestion.validationUser(username.Text.Trim().ToLower(), password.Password.ToString().ToLower().Trim());
                ouvrirFormCorrespondant();
            }
        }

        private bool validationEmpty()
        {
            if (username.Text == "" || password.Password == "")
            {
                MessageBox.Show("Erreur, vous devez remplir tout les champs", "Erreur", MessageBoxButton.OK);
                return false;
            }
            else
                return true;
        }

        private void ouvrirFormCorrespondant()
        {
            if (currentUser != null)
            {
                MessageBox.Show($"Bienvenu {currentUser.Username.ToUpper()} !", "Connexion Validé", MessageBoxButton.OK);

                if (currentUser.Titre == "Medecin")
                {
                    frmMedecin = new frmMedecin(currentUser, gestion);
                    this.Hide();
                    frmMedecin.ShowDialog();
                    this.Show();

                }
                else if (currentUser.Titre == "Admin")
                {
                    frmAdmin = new frmAdmin();
                    this.Hide();
                    frmAdmin.ShowDialog();
                    this.Show();
                }
                else if (currentUser.Titre == "Prepose")
                {
                    frmPrepose = new frmPrepose();
                    this.Hide();
                    frmPrepose.ShowDialog();
                    this.Show();
                }
                clearBox();
            }
            else
            {
                MessageBox.Show("Erreur, utilisateur entré n'existe pas", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                clearBox();
            }
        }

        private void clearBox()
        {
            username.Text = null;
            password.Password = null;
            username.Focus();
        }


        private void btnQuitter_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
