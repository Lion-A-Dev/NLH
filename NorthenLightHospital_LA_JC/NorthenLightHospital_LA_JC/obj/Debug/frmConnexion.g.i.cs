﻿#pragma checksum "..\..\frmConnexion.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "FFF3A17B024A0755F0371B965A14C2D50E16D764223B4F682F58FA5D07D14D55"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using NorthenLightHospital_LA_JC;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace NorthenLightHospital_LA_JC {
    
    
    /// <summary>
    /// frmConnexion
    /// </summary>
    public partial class frmConnexion : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\frmConnexion.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gridCon;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\frmConnexion.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox username;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\frmConnexion.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox password;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\frmConnexion.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCo;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\frmConnexion.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnQuitter;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/NorthenLightHospital_LA_JC;component/frmconnexion.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\frmConnexion.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 9 "..\..\frmConnexion.xaml"
            ((NorthenLightHospital_LA_JC.frmConnexion)(target)).Activated += new System.EventHandler(this.Window_Activated);
            
            #line default
            #line hidden
            return;
            case 2:
            this.gridCon = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.username = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.password = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 5:
            this.btnCo = ((System.Windows.Controls.Button)(target));
            
            #line 17 "..\..\frmConnexion.xaml"
            this.btnCo.Click += new System.Windows.RoutedEventHandler(this.btnCo_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnQuitter = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\frmConnexion.xaml"
            this.btnQuitter.Click += new System.Windows.RoutedEventHandler(this.btnQuitter_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

