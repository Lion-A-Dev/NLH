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

        private List<Admission> listAdmission;
        private List<Patient> listPatient;

        private Admission tableAdmin;
        private Patient tablePatient;


        public frmPrepose(frmConnexion con,Utilisateur clerk)
        {
            InitializeComponent();
            connex = con;
            prepose = clerk;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //INIT DB
            hospitalDB = new NLH_Entities();

            //INIT LISTS
            listAdmission = hospitalDB.Admission.ToList();
            listPatient = hospitalDB.Patient.ToList();


            //GROUPBOXES DISABLED
            groupNewPatient.IsEnabled = false;
            gridSearchPatient.IsEnabled = false;
            groupAdmission.IsEnabled = false;

            

            //INIT LISTPROVINCE
            initProvince();

            //INIT COMBO BOXES
            cboProvince.ItemsSource = provinceList;
            cboAssurance.DataContext = hospitalDB.Assurance.ToList();
            cboIDMed.DataContext = hospitalDB.Medecin.ToList();

            cboTypeChambre.DataContext = hospitalDB.TypeLit.ToList();
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
            provinceList.Add(new Province("AB"));
            provinceList.Add(new Province("BC"));
            provinceList.Add(new Province("MN"));
            provinceList.Add(new Province("NB"));
            provinceList.Add(new Province("NF"));
            provinceList.Add(new Province("NS"));
            provinceList.Add(new Province("ON"));
            provinceList.Add(new Province("PEI"));
            provinceList.Add(new Province("QC"));
            provinceList.Add(new Province("SK"));
            provinceList.Add(new Province("NWT"));
            provinceList.Add(new Province("NVT"));
            provinceList.Add(new Province("YK"));
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
            lblIDAdmin.Content = string.Empty;
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

            nonOperation.IsChecked = true ;
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
            rechercheADmission();
            recherchePatient();
        }

        private void rechercheADmission()
        {
            bool foundAdmission = false;

            //LA RECHERCHE DE L'ADMISSION EN QUESTION
            foreach (Admission admission in listAdmission)
            {
                if (admission.NSS.Equals(txtSearch.Text))
                {
                    //GROUP ADMISSION
                    lblIDAdmin.Content = admission.IDAdmission;
                    dpDateAdmin.SelectedDate = admission.DateAdmission;
                    dpDateConge.SelectedDate = admission.DateDuConge;
                    cboIDMed.SelectedItem = admission.IDMedecin;
                    cboTypeChambre.SelectedItem = admission.Lit;
                    cboNumLit.SelectedItem = admission.NumeroLit;
                    if (admission.Telephone == true)
                    {
                        checkPhone.IsChecked = true;
                    }
                    if (admission.Televiseur == true)
                    {
                        checkTV.IsChecked = true;
                    }


                    //GROUPD CHIRURGIE
                    if (admission.ChirurgieProgramme == true)
                    {
                        dpDateChirurgie.SelectedDate = admission.DateChirurgie;
                        yesOperation.IsChecked = true;
                    }
                    foundAdmission = true;
                    break;
                }
            }
            if (!foundAdmission)
            {
                MessageBox.Show("Erreur, Admission non-trouve");
                lblIDAdmin.Content = string.Empty;
                dpDateAdmin.SelectedDate = null;
                dpDateConge.SelectedDate = null;
                cboIDMed.SelectedItem = null;
                cboTypeChambre.SelectedItem = null;
                cboNumLit.SelectedItem = null;
                checkPhone.IsChecked = null;
                checkTV.IsChecked = null;
                yesOperation = null;
                nonOperation = null;
                dpDateChirurgie.SelectedDate = null;

            }
        }

        private void recherchePatient()
        {
            bool foundPatient = false;

            //DISPLAY DU PATIENT EN QUESTION
            foreach (Patient patient in listPatient)
            {
                if (patient.NSS.Equals(txtSearch.Text))
                {
                    //GROUP PATIENT
                    txtNSS.Text = patient.NSS;
                    txtNom.Text = patient.Nom;
                    txtPrenom.Text = patient.Prenom;
                    dateNaiss.SelectedDate = patient.DateNaissance;
                    txtAddre.Text = patient.Adresse;
                    txtCodeP.Text = patient.CodePostal;
                    txtVille.Text = patient.Ville;
                    cboProvince.SelectedItem = this.provinceList.FirstOrDefault(p => p.Nom == patient.Province.ToString());
                    //cboAssurance.SelectedItem = hospitalDB.Assurance.Where(p => p.)
                    txtTelephone.Text = patient.Telephone;
                    // txtUrgence.Text = this is a problem
                    foundPatient = true;
                    break;
                }
                
            }
            if (!foundPatient)
            {
                MessageBox.Show("Erreur, Patient non-trouve");
                txtNSS.Text = "";
                txtNom.Text = "";
                txtPrenom.Text = "";
                dateNaiss.SelectedDate = null;
                txtAddre.Text = "";
                txtCodeP.Text = "";
                txtVille.Text = "";
                cboProvince.Text = null;
                cboAssurance.SelectedItem = null;
                txtTelephone.Text = "";
            }
        }
    }
}
