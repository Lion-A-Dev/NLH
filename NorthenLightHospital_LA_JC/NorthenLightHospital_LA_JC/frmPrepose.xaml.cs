using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static NorthenLightHospital_LA_JC.frmConnexion;

namespace NorthenLightHospital_LA_JC
{
    /// <summary>
    /// AUTEUR:         Lion Arar
    /// 
    /// Mise à Jour:    YY/MM/DD par
    ///                 24/09/24
    ///                 
    /// Objectif: 
    ///     Formulaire qui permet la recherche d'un patient.
    ///     si il existe dans la db on peu créer une nouvelle admission
    ///     sans remplir toute les info du patient
    ///     sinon il faut remplir les deux formulaires.
    /// </summary>
    public partial class frmPrepose : Window
    {
        User prepose;
        frmConnexion connex;
        NLH_Entities hospitalDB;
        List<Province> provinceList;

        private List<Admission> listAdmission;
        private List<Patient> listPatient;
        private List<Medecin> listMD;
        private List<Assurance> listAss;
        private List<TypeLit> listTypeLit;
        private List<Lit> listLit;
        private List<Departement> listDept;

        private decimal frais;



        public frmPrepose()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //INIT DB
            hospitalDB = new NLH_Entities();

            //INIT LISTS
            listAdmission = hospitalDB.Admission.ToList();
            listPatient = hospitalDB.Patient.ToList();
            listMD = hospitalDB.Medecin.ToList();
            listAss = hospitalDB.Assurance.ToList();
            listTypeLit = hospitalDB.TypeLit.ToList();
            listLit = hospitalDB.Lit.ToList();
            listDept = hospitalDB.Departement.ToList();

            //GROUPBOXES DISABLED
            groupNewPatient.IsEnabled = false;
            gridSearchPatient.IsEnabled = false;
            groupAdmission.IsEnabled = false;
            dpDateConge.IsEnabled = false;

            //INIT LISTPROVINCE
            initProvince();

            //INIT COMBO BOXES
            cboProvince.ItemsSource = provinceList;
            cboAssurance.ItemsSource = hospitalDB.Assurance.ToList();
            cboIDMed.ItemsSource = hospitalDB.Medecin.ToList();
            cboTypeChambre.ItemsSource = listTypeLit;
            


            //ITEMSOURCE = AVAILABLE BEDS
            cboNumLit.ItemsSource = hospitalDB.Lit.ToList().Where(l => l.Occupe == false);
            
            dpDateAdmin.SelectedDate = DateTime.Today;
          
            nonOperation.IsChecked = true;


        }

        private void btn_ajouter_Click(object sender, RoutedEventArgs e)
        {
            if (validateAll())
            {
                bool patientExists = false;

                Patient patient;
                patient = hospitalDB.Patient.FirstOrDefault(p => p.NSS == txtSearch.Text.Trim());
                Admission admision = creationAdmission();

                if (patient != null)
                {
                    hospitalDB.Admission.Add(admision);
                    MessageBox.Show("Added Admission, Patient Existed");

                }
                else
                {
                    patient = creationPatient();
                    hospitalDB.Patient.Add(patient);
                    Admission newAdmission = creationAdmission();
                    hospitalDB.Admission.Add(newAdmission);
                    MessageBox.Show("Added new Admission, Patient did not Exsit");

                }

                try
                {
                    // Enregistrement dans la base de données.
                    hospitalDB.SaveChanges();
                    MessageBox.Show("Admission validé.", "VALIDATION", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                catch (Exception ex)
                {
                    // Récupération de l'erreur retournée par SQL Server.
                    MessageBox.Show(ex.ToString(),
                        "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            
        }

        private int GetAge(DateTime birthDate)
        {
            DateTime today = DateTime.Now;
            int age = today.Year - birthDate.Year;
            if (today.Month < birthDate.Month || (today.Month == birthDate.Month && today.Day < birthDate.Day)) { age--; }
            return age;
        }

        private Admission creationAdmission()
        {
            Admission admission = new Admission();
            admission.NumeroLit = Convert.ToInt32(cboNumLit.Text);
            admission.DateAdmission = DateTime.Today;
            if (yesOperation.IsChecked == true)
            {
                admission.ChirurgieProgramme = true;
                admission.DateChirurgie = (DateTime)dpDateChirurgie.SelectedDate;

            }
            else
            {
                admission.ChirurgieProgramme = false;
            }
            // le ?? false verifie si le checkbox est cocher, si oui, cest true, si non cest false
            admission.Televiseur = checkTV.IsChecked ?? false;
            admission.Telephone = checkPhone.IsChecked ?? false;
            admission.NSS = txtNSS.Text;

            Medecin med = cboIDMed.SelectedItem as Medecin;
            admission.IDMedecin = med.IDMedecin;
            admission.Medecin = med;

            Lit bed = cboNumLit.SelectedItem as Lit;
            admission.Lit = bed;
            admission.NumeroLit = bed.NumeroLit;

            return admission;
        }

        private Patient creationPatient()
        {
            Patient nouveauPatient = new Patient();

            //Assurance as object might not be needed
            nouveauPatient.Assurance = cboAssurance.SelectedItem as Assurance;
            Assurance ass = cboAssurance.SelectedItem as Assurance;
            nouveauPatient.IDAssurance = ass.IDAssurance;
            nouveauPatient.NSS = txtNSS.Text;
            nouveauPatient.Prenom = txtPrenom.Text;
            nouveauPatient.Nom = txtNom.Text;
            nouveauPatient.Adresse = txtAddre.Text;
            nouveauPatient.Telephone = txtTelephone.Text;
            nouveauPatient.Ville = txtVille.Text;
            nouveauPatient.CodePostal = txtCodeP.Text;
            nouveauPatient.Province = cboProvince.Text;
            nouveauPatient.DateNaissance = (DateTime)dateNaiss.SelectedDate;
            return nouveauPatient;
        }

        private bool validateAll()
        {
            bool all = false;
            
            bool regex = validationAllRegEx();
            bool patient = validationPatient();
            bool admin = validationAdmission();
            bool chambre = validationChambre();
            bool chirurgie = validationChirurgie();
            

            if (regex == true && patient == true && admin == true && chambre == true && chirurgie == true)
            {
                all = true;
            }
            return all;
        }


        private bool validationChirurgie()
        {
            bool valid = false;
            if (yesOperation.IsChecked == true && (dpDateChirurgie.SelectedDate == null || dpDateChirurgie.SelectedDate < DateTime.Today))
            {
                MessageBox.Show("Erreur, vous avez prevue une chirurgie, svp selectionner une date valide");
                return valid;
            }
            valid = true;
            return valid;
        }

        private bool validationChambre()
        {
            bool valid = false;
            foreach (UIElement element in gridChambre.Children)
            {
                if (element is ComboBox combobox && combobox.SelectedItem == null)
                {
                    MessageBox.Show("Erreur, vous ne pouvez pas ajouter une Admission sans selectionner un type de chambre ou un lit");
                    return valid;
                }
            }
            valid = true;
            return valid;
        }

        private bool validationAdmission()
        {
            bool valid = false;
            foreach (UIElement element in gridAdmission.Children)
            {
                if (element is DatePicker dp && dp == dpDateAdmin && (dp.SelectedDate < DateTime.Today || dp.SelectedDate > DateTime.Today || dp.SelectedDate == null))
                {
                    MessageBox.Show("Erreur, vous devez selectionner une date valide");
                    return valid;
                }

                if (element is ComboBox comboBox && comboBox == cboIDMed && cboIDMed.SelectedItem == null)
                {
                    MessageBox.Show("Erreur, vous devez choisir un medecin");
                    return valid;
                }
            }
            valid = true;
            return valid;
        }

        private bool validationPatient()
        {
            bool valid = false;


            foreach (UIElement element in gridNouveauPatient.Children)
            {
                if (element is TextBox txtbox)
                {
                    if (string.IsNullOrEmpty(txtbox.Text))
                    {
                        MessageBox.Show("Erreur, il faut remplir tous les cases vous " +
                            "laisser " + txtbox.Name + " vide");
                        return valid;
                    }
                }
                if (element is DatePicker dp && dp == dateNaiss)
                {
                    if (dp.SelectedDate == null)
                    {
                        MessageBox.Show("Erreur il faut choisir une date de naissance");
                        //VALIDATE THE BIRTH DATE
                        return valid;
                    }
                }
                if (element is ComboBox cb)
                {
                    if (cb == cboAssurance && cb.SelectedItem == null)
                    {
                        MessageBox.Show("Erreur, il faut choisir une option Assurance");
                        return valid;
                    }
                    else if (cb == cboProvince && cb.SelectedItem == null)
                    {
                        MessageBox.Show("Erreur, il faut choisir une province");
                        return valid;
                    }
                }

            }
            valid = true;

            return valid;
        }

        private bool validationAllRegEx()
        {
            bool valid = false;
            if (nssRegEx() == false)
            {
                MessageBox.Show("Erreur le NSS doit etre entrer sous form de 000000000000");
                return valid;
            }
            if (phoneRegEx() == false)
            {
                MessageBox.Show("Erreur, le telephone doit etre entre avec le patron xxxxxxxxxx");
                return valid ;
            }
            if (codePostalRegEx() == false)
            {
                MessageBox.Show("Erreur le Code Postal doit etre sous le format A1B2C3 ou a1b2c3");
                return valid;
            }
            else
            {
                valid = true;
            }
            return valid;
        }

        private bool phoneRegEx()
        {
            bool valid = false;
            string phonePattern = @"^[0-9]{10}$";
            valid = Regex.IsMatch(txtTelephone.Text, phonePattern);
            return valid;
        }

        private bool codePostalRegEx()
        {
            bool valid = false;
            string zipPattern = @"^[A-Z]{1}[0-9]{1}[A-Z]{1}[0-9]{1}[A-Z]{1}[0-9]{1}$";

            valid = Regex.IsMatch(txtCodeP.Text.ToUpper(), zipPattern);
            return valid;
        }

        private bool nssRegEx()
        {
            bool valid = false;
            string nssPatern = @"^[0-9]{12}$";
            valid = Regex.IsMatch(txtNSS.Text, nssPatern);
            return valid;
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
            txtSearch.IsEnabled = false;
            btn_search.IsEnabled = false;
            
        }

        private void cboTypeChambre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TypeLit type = cboTypeChambre.SelectedItem as TypeLit;
            
            try
            {
                int agePatient = GetAge((DateTime)dateNaiss.SelectedDate);
                if (agePatient <= 16 && nonOperation.IsChecked == true)
                {
                    cboNumLit.ItemsSource = listLit.Where(l => l.TypeLit == type && l.IDDepartement == 20 && l.Occupe == false);
                }
                else if (yesOperation.IsChecked == true)
                {
                    cboNumLit.ItemsSource = listLit.Where(l => l.TypeLit == type && l.IDDepartement == 10 && l.Occupe == false);
                }
                else
                {
                    cboNumLit.ItemsSource = listLit.Where(l => l.TypeLit == type && l.IDDepartement == 30 && l.Occupe == false);
                }
            }catch(Exception ex)
            {
                MessageBox.Show("Il se peut que vous n'avez pas choisie de Date de Naissance, veuillez faire ca, avant de selectionner un type de chambre");
                return;
            }
            
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

            //cboNumLit.ItemsSource = listLit.Where(l => l.IDDepartement == 10);

            TypeLit type = cboTypeChambre.SelectedItem as TypeLit;

            try
            {
                int agePatient = GetAge((DateTime)dateNaiss.SelectedDate);
                if (agePatient <= 16 && nonOperation.IsChecked == true)
                {
                    cboNumLit.ItemsSource = listLit.Where(l => l.TypeLit == type && l.IDDepartement == 20 && l.Occupe == false);
                }
                else if (yesOperation.IsChecked == true)
                {
                    cboNumLit.ItemsSource = listLit.Where(l => l.TypeLit == type && l.IDDepartement == 10 && l.Occupe == false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Il se peut que vous n'avez pas choisie de Date de Naissance, veuillez faire ca, avant de selectionner un type de chambre");
                return;
            }


        }

        private void nonOperation_Checked(object sender, RoutedEventArgs e)
        {
            dpDateChirurgie.IsEnabled = false;

            TypeLit type = cboTypeChambre.SelectedItem as TypeLit;

            try
            {
                int agePatient = GetAge((DateTime)dateNaiss.SelectedDate);
                if (agePatient <= 16 && nonOperation.IsChecked == true)
                {
                    cboNumLit.ItemsSource = listLit.Where(l => l.TypeLit == type && l.IDDepartement == 20 && l.Occupe == false);
                }
                else
                {
                    cboNumLit.ItemsSource = listLit.Where(l => l.TypeLit == type && l.IDDepartement == 30 && l.Occupe == false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Il se peut que vous n'avez pas choisie de Date de Naissance, veuillez faire ca, avant de selectionner un type de chambre");
                return;
            }
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
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
                    cboIDMed.SelectedItem = listMD.FirstOrDefault(m => m.IDMedecin == admission.IDMedecin);
                    cboNumLit.SelectedItem = listLit.FirstOrDefault(lit => lit.NumeroLit == admission.NumeroLit);
                    cboTypeChambre.SelectedItem = listTypeLit.FirstOrDefault(tl => tl.LitDesc == admission.Lit.TypeLit.LitDesc);
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
                    cboAssurance.SelectedItem = listAss.FirstOrDefault(ass => ass.IDAssurance == patient.IDAssurance);


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

        private void cboAssurance_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Assurance selectedAss = cboAssurance.SelectedItem as Assurance;

            //if (selectedAss.NomCompagnie.Equals("Sans Assurance"))
            //{
            //    if (true)
            //    {

            //    }
            //}
        }
    }
}