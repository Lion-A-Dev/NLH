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
using static NorthenLightHospital_LA_JC.frmConnexion;

namespace NorthenLightHospital_LA_JC
{
    /// <summary>
    /// AUTEUR:         Jean COUTURIER
    /// 
    /// Mise à Jour:    YY/MM/DD par
    ///                 24/09/12
    ///                 24/09/26 par Jean
    ///                 24/09/27 par Jean
    ///                 
    /// Objectif: 
    ///     Formulaire qui permet, si c'est un administrateur qui c'est connecté
    ///     précédement, d'ajouter ou modifier les informations d'un médecin sans
    ///     pouvoir changer son ID. Il permet également la suppréssion d'un médecin.
    ///     le bouton retour permet de revenir à la fenêtre de connexion.
    /// </summary>
    public partial class frmAdmin : Window
    {
        NLH_Entities nlh;

        public frmAdmin()
        {
            InitializeComponent();
            initGridMedecin();
        }

        /// <summary>
        /// Gestion de l'evenement qui permet l'ajout ou la modification d'un medecin.
        /// </summary>
        private void btnAjouterMed_Click(object sender, RoutedEventArgs e)
        {
            Medecin med = dgMedecin.SelectedItem as Medecin;
            bool OK = true;

            OK = string.IsNullOrEmpty(med.Prenom) ? false : true;
            OK = string.IsNullOrEmpty(med.Nom) || !OK ? false : true;

            // Si les informations saisies sont valides.
            if (OK)
            {
                // Création et utilisation de notre objet EmployesDataContext.
                using (nlh = new NLH_Entities())
                {
                    // Si le ID est égal à 0, il s'agit d'un nouveau medecin.
                    if (med.IDMedecin == 0)
                    {
                        // Création de notre objet de type Medecin.
                        Medecin newMed = new Medecin
                        {
                            //Récupération des valeurs du nouveau medecin.
                            Prenom = med.Prenom,
                            Nom = med.Nom
                        };
                        // Enregistrement de notre nouveau medecin.
                        nlh.Medecin.Add(newMed);
                    }
                    // Sinon, il s'agit d'un employé existant.
                    else
                    {
                        // Récupération du medecin dans la liste des medecins.
                        Medecin m = nlh.Medecin.FirstOrDefault(x => x.IDMedecin == med.IDMedecin);
                        // Modification des informations du médecin récupéré.
                        m.Prenom = med.Prenom;
                        m.Nom = med.Nom;
                    }
                    try
                    {
                        // Enregistrement dans la base de données.
                        nlh.SaveChanges();
                        MessageBox.Show("Enregistrement dans la base de données réussi.",
                            "Enregistrement", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    // Récupération de l'erreur retournée par SQL Server.
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(),
                            "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vous devez saisir toutes les informations du médecin correctement.",
                    "Enregistrement", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //Mis à jour de la grille de médecins
            initGridMedecin();
            btnAjouterMed.Content = "AJOUTER";
        }

        /// <summary>
        /// Gestion de l'evenement pour supprimer un medecin selectionné dans le data grid
        /// </summary>
        private void btnSuppMed_Click(object sender, RoutedEventArgs e)
        {
            Medecin med = dgMedecin.SelectedItem as Medecin;
            if (med != null)
            {
                // On demande à l’utilisateur de confirmaer son action.
                var resultat = MessageBox.Show("Êtes-vous certain de vouloir supprimer le médecin "
                    + med.Prenom + " " + med.Nom + "\nL'action est irréversible !",
                    "Attention", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                // Si la réponse est positive, on procède à la suppression.
                if (resultat == MessageBoxResult.Yes)
                {
                    // Création et utilisation de notre contextr.
                    using (nlh = new NLH_Entities())
                    {
                        // Sélection du médecin dans la liste.
                        Medecin m = nlh.Medecin.FirstOrDefault(x => x.IDMedecin == med.IDMedecin);
                        // Suppression du médecin sélectionné dans la liste.
                        nlh.Medecin.Remove(m);
                        // Mise-à-jour de la base de données.
                        nlh.SaveChanges();
                    }
                }
            }
            else
            {
                MessageBox.Show("Vous devez sélectionner un médecin.",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            initGridMedecin();
        }

        /// <summary>
        /// Fermeture tu form Admin
        /// retour au form Connexion
        /// </summary>
        private void btnRetourMenu_Click(object sender, RoutedEventArgs e)
        {
            //Quit this form pour réafficher le menu principal ?
            //Menu principal qui prévaut la fenetre de connexion ?

            this.Close();
        }

        /// <summary>
        /// Méthode de remplissage du grid des medecins
        /// est appeler a la cration de la page,
        /// apres un ajout, une modification
        /// ou une suppression d'un médecin
        /// </summary>
        private void initGridMedecin()
        {
            nlh = new NLH_Entities();
            dgMedecin.ItemsSource = nlh.Medecin.ToList();
        }

        /// <summary>
        /// Modifier le btnAjouter en fonction de la nouveauté de l'objet selectionné
        /// si le medecin existe deja (id > 0) le btn est Modifier
        /// sinon (id = 0) le btn est Ajouter
        /// </summary>
        private void dgMedecin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Medecin med = dgMedecin.SelectedItem as Medecin;
            btnAjouterMed.Content = (med == null || med.IDMedecin == 0) ? "AJOUTER" : "MODIFIER";
        }
    }
}
