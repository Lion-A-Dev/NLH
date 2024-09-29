using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static NorthenLightHospital_LA_JC.frmConnexion;

namespace NorthenLightHospital_LA_JC
{
    /// <summary>
    ///  AUTEUR:         Jean Couturier
    /// 
    ///  Mise à Jour:    YY/MM/DD par
    ///                  24/09/28
    ///                 
    ///  Objectif: 
    ///     Class de controle qui authentifie l'utilisateur qui tente de ce connecter
    ///     Si l'utilisateur est un medecin il lui permettra de récupére 
    ///     une liste contenant tous ses patient admis
    /// </summary>
    public class Gestion
    {
        private List<User> listeUtilisateur = new List<User>();
        private List<Medecin> medecins = new List<Medecin>();

        private NLH_Entities nlh;

        public Gestion()
        {
            listeUtilisateur.Add(new User() { Username = "amine", Password = "1234", Titre = "Admin" } );
            listeUtilisateur.Add(new User() { Username = "billie", Password = "1793", Titre = "Prepose" } );
            majListMedecin();
        }

        /// <summary>
        /// Procédure de mise à jour de la liste de médecin 
        /// pour authentification. Si un Admin se connect et ajoute ou modifie un medecin 
        /// les changement seront automatiquement effectué
        /// </summary>
        public void majListMedecin()
        {
            listeUtilisateur.RemoveAll(u => u.Titre == "Medecin");

            nlh = new NLH_Entities();
            medecins = nlh.Medecin.ToList();

            foreach (Medecin m in medecins)
                listeUtilisateur.Add(new User() { Username = m.Prenom.ToLower(), Password = m.IDMedecin.ToString(), Titre = "Medecin" });

            //Verification de la liste mise à jour
            foreach (var u in listeUtilisateur)
                Console.WriteLine("user: " + u.Username + " Pw: " + u.Password + " Titre: " + u.Titre);
        }

        /// <summary>
        /// Fonction qui valide les input saisie par l'utilisateur
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="pw"></param>
        /// <returns>une instance de User correspondant aux identifaints saisi</returns>
        public User validationUser(string nom, string pw)
        {
            User user;
            return user = listeUtilisateur.FirstOrDefault(u => nom == u.Username && pw == u.Password);
        }


        //******************************************************************************************** Récupération des Admissions et Patient correspondant ***

        /// <summary>
        /// Fonction qui cherche dans la base de données la liste de toute
        /// les admission
        /// </summary>
        /// <param name="idMed"></param>
        /// <returns>la liste de toutes les admissions lié au medecin x</returns>
        public List<Admission> getListPatientAdmis(int idMed)
        {
            nlh = new NLH_Entities();
            List<Admission> listPatientAdmis = new List<Admission>();

            foreach (Admission a in nlh.Admission)
            {
                if (a.IDMedecin == idMed)
                {
                    listPatientAdmis.Add(a);
                }
            }
            return listPatientAdmis;
        }

        /// <summary>
        /// Fonction qui cherche dans la base de données la liste de tous
        /// les patient. Permet l'affichage du nom et prenom du patient
        /// dans le form Medecin pouir attribution de congé
        /// </summary>
        /// <param name="idMed"></param>
        /// <returns>la liste de tous les Patient lié a aux admission récupére précédement</returns>
        public List<Patient> getListPatient(int idMed)
        {
            nlh = new NLH_Entities();
            Patient pat = new Patient();
            List<Patient> listPatient = new List<Patient>();

            foreach (Admission a in nlh.Admission)
            {
                if (a.IDMedecin == idMed)
                {
                    pat = nlh.Patient.Single(p => p.NSS == a.NSS);
                    listPatient.Add(pat);
                }
            }
            return listPatient;
        }
    }
}
