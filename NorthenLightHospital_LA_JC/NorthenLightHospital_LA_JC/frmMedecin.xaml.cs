﻿using System;
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
    /// Logique d'interaction pour frmMedecin.xaml
    /// </summary>
    public partial class frmMedecin : Window
    {
        Utilisateur medecin;
        public frmMedecin(Utilisateur docteur)
        {
            InitializeComponent();
            medecin = docteur;
        }

    }
}
