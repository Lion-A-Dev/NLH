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
    ///                 24/09/28
    ///                 
    /// Objectif: 
    ///     Formulaire qui permet, si c'est un médecin qui c'est connecté
    ///     précédement, d'ajouter ou modifier la date de congé d'un patient
    ///     le bouton retour permet de revenir à la fenêtre de connexion.
    /// </summary>
    public partial class frmMedecin : Window
    {
        NLH_Entities nlh;
        int idMedecin;
        List<Patient> listPatients = new List<Patient>();
        List<Admission> PatientAdmis = new List<Admission>();

        public frmMedecin(User user, Gestion gestion)
        {
            InitializeComponent();

            idMedecin = int.Parse(user.Password);

            PatientAdmis = gestion.getListPatientAdmis(idMedecin);
            cbPatient.ItemsSource = PatientAdmis;

            listPatients = gestion.getListPatient(idMedecin);

        }

        /// <summary>
        /// Evenement qui gère l'attribution d'un date de congé a une admission existante
        /// si une date est attribué, le lit occupé par le patient change de occupé à libre.
        /// </summary>
        private void btnAjouterCong_Click(object sender, RoutedEventArgs e)
        {
            Admission ad = cbPatient.SelectedItem as Admission;
            bool congé = true;

            congé = ValiderCongéEtChirurgie(ad);

            if (dpCongé.SelectedDate > ad.DateAdmission && congé)
            {
                using (nlh = new NLH_Entities())
                {
                    //Récuperation des instances Admission et Lit pour l'update des données
                    Admission admission = nlh.Admission.FirstOrDefault(a => a.IDAdmission == ad.IDAdmission);
                    Lit lit = nlh.Lit.FirstOrDefault(l => l.NumeroLit == ad.NumeroLit);

                    //Attribution de la date de congé choisie
                    admission.DateDuConge = dpCongé.SelectedDate;

                    //Libération du lit occuypé par le patient:
                    lit.Occupe = false;


                    //Demande de confirmation
                    MessageBoxResult reponse = MessageBox.Show($"Êtes-vous sûr de valider {admission.DateDuConge} comme date de congé " +
                            $"pour le patient admi sous le numéro {admission.IDAdmission} ?",
                            "CONFIRMATION", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (reponse == MessageBoxResult.Yes)
                    {
                        try
                        {
                            // Enregistrement dans la base de données.
                            nlh.SaveChanges();
                            MessageBox.Show("Attribution de congé validé.", "VALIDATION", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            cbPatient.SelectedItem = null;
                        }
                        catch (Exception ex)
                        {
                            // Récupération de l'erreur retournée par SQL Server.
                            MessageBox.Show(ex.ToString(),
                                "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Attribution de congé annulé.", "ANNULATION", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        cbPatient.SelectedItem = null;
                    }
                }
            }
            else
            {
                MessageBox.Show("La date de congé doit être ultérieur à la date d'admission\n" +
                    "ainsi qu'à la date de chirurgie (si il y a lieu) !", "Date non VALIDE", MessageBoxButton.OK, MessageBoxImage.Error);
                dpCongé.SelectedDate = DateTime.Now;
            }
                
            
        }

        /// <summary>
        ///     Méthode qui s'assure qu'une date de chirurgie est presente dans le 
        ///     dossier d'admission du patient selectionné. 
        ///     Si il y a une date de chirurgie la fonction vérifie que la date de congé choisie et bien ultérieur à celle-ci
        ///     Si il n'y a pas de date de chirurgie programmé la date de congé ny sera pas confronté.
        /// </summary>
        /// <param name="ad">
        ///     Instance Admission passé en paramètre pour récupérer la date de chirurgie (Si il y a lieu)
        /// </param>
        /// <returns>
        ///     Return true si il n'y a pas de chirurgie daté OU si la date de congé est srictement ultérieur à la date de chirurgie
        /// </returns>
        private bool ValiderCongéEtChirurgie(Admission ad)
        {
            if (ad.DateChirurgie != null)
                return (dpCongé.SelectedDate > ad.DateChirurgie) ? true : false;
            else
                return true;

            //Sous UNE expression ternaire: 
            //return (ad.DateChirurgie != null) ? ((dpCongé.SelectedDate > ad.DateChirurgie) ? true : false) : true;
        }

        /// <summary>
        /// Mis à jour de certain label en fonction de l'admission
        /// du cbPatient selectionné
        /// </summary>
        private void cbPatient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Admission admis = cbPatient.SelectedItem as Admission;
            if (admis != null)
            {
                Patient pat = listPatients.First(p => p.NSS == admis.NSS);
                lblNomPat.Content = $"{pat.Prenom}  {pat.Nom}";

                if (admis.ChirurgieProgramme == false)
                    lblOperation.Content = "N/A";
                else
                    lblOperation.Content = admis.DateChirurgie;
            }
            else
            {
                lblNomPat.Content = null;
                lblOperation.Content = null;
            }
        }

        private void btnRetourMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}