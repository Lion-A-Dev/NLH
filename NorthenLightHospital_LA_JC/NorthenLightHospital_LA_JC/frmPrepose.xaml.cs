using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// Logique d'interaction pour frmPrepose.xaml
    /// </summary>
    public partial class frmPrepose : Window
    {
        Utilisateur prepose;
        frmConnexion connex;
        NLH_Entities hospitalDB;
        List<Province> provinceList;
        

        public frmPrepose(frmConnexion con,Utilisateur clerk)
        {
            InitializeComponent();
            connex = con;
            prepose = clerk;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //GROUPBOXES DISABLED
            groupNewPatient.IsEnabled = false;
            gridSearchPatient.IsEnabled = false;
            groupAdmission.IsEnabled = false;

            //INIT DB
            hospitalDB = new NLH_Entities();

            //INIT LISTPROVINCE
            initProvince();

            //INIT COMBO BOXES
            cboProvince.ItemsSource = provinceList;
            cboAssurance.DataContext = hospitalDB.Assurance.ToList();
            cboIDMed.DataContext = hospitalDB.Medecin.ToList();

            cboTypeChambre.DataContext = hospitalDB.TypeLit.ToList();
            //call method that updates the type of Rooms
            displayAvailableRooms();


            //DATACONTEXT = AVAILABLE BEDS
            cboNumLit.DataContext = hospitalDB.Lit.ToList().Where(l => l.Occupe == false);

            nonOperation.IsChecked = true;

            dpDateAdmin.SelectedDate = DateTime.Now;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            connex.Show();
        }

        private void quit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            connex.Show();
        }

        private void newPatient_Checked(object sender, RoutedEventArgs e)
        {
            groupNewPatient.IsEnabled = true;
            groupAdmission.IsEnabled = true;
            txtSearch.Text = "";
            txtSearch.IsEnabled = false;
            btn_search.IsEnabled = false;

            //call method that updates the type of Rooms available instead
            //dont thik the call for the method is needed
            cboTypeChambre.SelectedIndex = 0;
        }

        private void findPatient_Checked(object sender, RoutedEventArgs e)
        {
            groupNewPatient.IsEnabled = false;
            groupAdmission.IsEnabled = false;
            gridSearchPatient.IsEnabled = true;

            txtSearch.IsEnabled = true;
            btn_search.IsEnabled = true;

        }

        private void initProvince()
        {
            provinceList = new List<Province>();
            provinceList.Add(new Province("Alberta"));
            provinceList.Add(new Province("British Columbia"));
            provinceList.Add(new Province("Manitoba"));
            provinceList.Add(new Province("New Brunswick"));
            provinceList.Add(new Province("Newfoundland and Labrador"));
            provinceList.Add(new Province("Nova Scotia"));
            provinceList.Add(new Province("Ontario"));
            provinceList.Add(new Province("Prince Edward Island"));
            provinceList.Add(new Province("Quebec"));
            provinceList.Add(new Province("Saskatchewan"));
            provinceList.Add(new Province("Northwest Territories"));
            provinceList.Add(new Province("Nunavut"));
            provinceList.Add(new Province("Yukon"));
        }
    
        public class Province
        {
            private string nom { get; set; }

            public Province(string nom)
            {
                this.nom = nom; 
            }

            public string Nom
            {
                get { return nom; }
            }

        }

        private void btn_annuler_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = string.Empty;

            txtNSS.Text = string.Empty;
            txtNom.Text = string.Empty;
            txtPrenom.Text = string.Empty;
            dateNaiss.SelectedDate = null;
            txtAddre.Text = string.Empty;
            txtCodeP.Text = string.Empty;
            txtVille.Text = string.Empty;
         
            txtTelephone.Text = string.Empty;
            txtUrgence.Text = string.Empty;
            
            dpDateAdmin.SelectedDate = null;
            dpDateConge.SelectedDate = null;
            dpDateChirurgie.SelectedDate = null;

            cboAssurance.SelectedItem = null;
            cboNumLit.SelectedItem = null;
            cboIDMed.SelectedItem = null;
            cboTypeChambre.SelectedItem = null;
            cboProvince.SelectedItem = null;

            checkTV.IsChecked = false;
            checkPhone.IsChecked = false;

            yesOperation.IsChecked = false;
            nonOperation.IsChecked = false;
        }

        private void yesOperation_Checked(object sender, RoutedEventArgs e)
        {
            dpDateChirurgie.IsEnabled = true;
        }

        private void nonOperation_Checked(object sender, RoutedEventArgs e)
        {
            dpDateChirurgie.IsEnabled = false;
        }

        private void displayAvailableRooms()
        {
            TypeLit type = new TypeLit();
            var availableTypes = hospitalDB.Database.SqlQuery<string>("select distinct type.LitDesc \r\nfrom TypeLit type, Lit bed \r\nwhere type.IDType = bed.IDType \r\nand bed.Occupe = 0;").ToList();

            cboTypeChambre.ItemsSource = availableTypes;

            cboTypeChambre.DisplayMemberPath = type.LitDesc;



        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            Patient foundPatient = new Patient();
            //Admission foundAdmission = new Admission();
            //foundAdmission.Patient = foundPatient;
            var sqlAdmin = hospitalDB.Database.SqlQuery<string>("select admin.IDAdmission\r\nfrom Admission admin, Patient patient\r\nwhere admin.NSS = patient.NSS");


            //its own method to call
            foreach (Patient patient in hospitalDB.Patient)
            {
                if (patient.NSS.Equals(txtSearch.Text))
                {
                    foundPatient = patient;
                    txtNSS.Text = foundPatient.NSS;
                    txtNom.Text = foundPatient.Nom;
                    txtPrenom.Text = foundPatient.Prenom;
                    dateNaiss.SelectedDate = foundPatient.DateNaissance;
                    txtAddre.Text = foundPatient.Adresse;
                    txtCodeP.Text = foundPatient.CodePostal;
                    txtVille.Text = foundPatient.Ville;
                    cboProvince.Text = foundPatient.Province;
                    cboAssurance.SelectedItem = foundPatient.Assurance;

                    txtTelephone.Text = foundPatient.Telephone;
                    // txtUrgence.Text = this is a problem
                    break;
                }
            }
            

            //Show Admission here
            //{
            //}
            
        }
    }
}
