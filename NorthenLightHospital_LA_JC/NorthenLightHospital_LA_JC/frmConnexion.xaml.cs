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
    /// Logique d'interaction pour frmConnexion.xaml
    /// </summary>
    public partial class frmConnexion : Window
    {
        List<Utilisateur> listeUtilisateur;
        frmAdmin frmAdmin;
        frmMedecin frmMedecin;
        frmPrepose frmPrepose;

        Utilisateur currentUser;

        public  class Utilisateur
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Titre { get; set; }
        }


        public frmConnexion()
        {
            InitializeComponent();
        }

        private void btnCo_Click(object sender, RoutedEventArgs e)
        {
            validationEmpty();
            validationUser();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listeUtilisateur = new List<Utilisateur>();
            listeUtilisateur.Add(new Utilisateur() { Username = "Jean".ToLower(), Password = "1234", Titre = "Admin" });
            listeUtilisateur.Add(new Utilisateur() { Username = "Lion".ToLower(), Password = "9876", Titre = "Medecin" });
            listeUtilisateur.Add(new Utilisateur() { Username = "Billie".ToLower(), Password = "1793", Titre = "Prepose" });
        }

        private void validationEmpty()
        {
            foreach(UIElement element in gridCon.Children)
            {
                if (element is TextBox txt)
                {
                    if (string.IsNullOrEmpty(txt.Text))
                    {
                        MessageBox.Show("Erreu, vous devez entrez un nom Utilisateur", "Erreur", MessageBoxButton.OK);
                        return;
                    }
                }
                else if(element is PasswordBox pbox)
                {
                    if (string.IsNullOrEmpty(pbox.Password))
                    {
                        MessageBox.Show("Erreu, vous devez entrez un mot de passe", "Erreur", MessageBoxButton.OK);
                        return;
                    }
                }

                    
            }
        }

        private void validationUser()
        {
            bool found = false;
            string titre = "";

            foreach (Utilisateur users in listeUtilisateur)
            {
                if (username.Text.ToLower().Equals(users.Username) && password.Password.Equals(users.Password))
                {
                    found = true;
                    MessageBox.Show("Bienvenue " +  " " + users.Username,"Bienvenue",MessageBoxButton.OK);
                    titre = users.Titre;
                    currentUser = users;
                    break;
                }
            }
            if (found == true)
            {

                if (titre == "Medecin")
                {
                    frmMedecin = new frmMedecin(currentUser);
                    this.Hide();
                    frmMedecin.Show();

                }
                else if (titre == "Admin")
                {
                    frmAdmin = new frmAdmin(currentUser);
                    this.Hide();
                    frmAdmin.Show();
                }
                else if (titre == "Prepose")
                {
                    frmPrepose = new frmPrepose(this, currentUser);
                    this.Hide();
                    frmPrepose.Show();
                }
            }
            else
            {
                MessageBox.Show("Erreur, utilisateur entre n'existe pas", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
    }
}

