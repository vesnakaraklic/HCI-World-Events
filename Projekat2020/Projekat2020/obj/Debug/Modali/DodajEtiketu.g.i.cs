﻿#pragma checksum "..\..\..\Modali\DodajEtiketu.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "DACC96E5791AF8A0480737A802E15A042C00B902"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Projekat2020.Modali;
using Projekat2020.Validacija;
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
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Chromes;
using Xceed.Wpf.Toolkit.Core.Converters;
using Xceed.Wpf.Toolkit.Core.Input;
using Xceed.Wpf.Toolkit.Core.Media;
using Xceed.Wpf.Toolkit.Core.Utilities;
using Xceed.Wpf.Toolkit.Panels;
using Xceed.Wpf.Toolkit.Primitives;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using Xceed.Wpf.Toolkit.PropertyGrid.Commands;
using Xceed.Wpf.Toolkit.PropertyGrid.Converters;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;
using Xceed.Wpf.Toolkit.Zoombox;


namespace Projekat2020.Modali {
    
    
    /// <summary>
    /// DodajEtiketu
    /// </summary>
    public partial class DodajEtiketu : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 35 "..\..\..\Modali\DodajEtiketu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox EtiketaID;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\..\Modali\DodajEtiketu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Xceed.Wpf.Toolkit.ColorPicker EtiketaBoja;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\Modali\DodajEtiketu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox EtiketaOpis;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\Modali\DodajEtiketu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock EtiketaOpisGreske;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\Modali\DodajEtiketu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SubmitButton;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\Modali\DodajEtiketu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button IzadjiButton;
        
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
            System.Uri resourceLocater = new System.Uri("/Projekat2020;component/modali/dodajetiketu.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Modali\DodajEtiketu.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            
            #line 15 "..\..\..\Modali\DodajEtiketu.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.IzadjiButton_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.EtiketaID = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.EtiketaBoja = ((Xceed.Wpf.Toolkit.ColorPicker)(target));
            
            #line 59 "..\..\..\Modali\DodajEtiketu.xaml"
            this.EtiketaBoja.Closed += new System.Windows.RoutedEventHandler(this.EtiketaBoja_Closed);
            
            #line default
            #line hidden
            return;
            case 4:
            this.EtiketaOpis = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.EtiketaOpisGreske = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.SubmitButton = ((System.Windows.Controls.Button)(target));
            
            #line 63 "..\..\..\Modali\DodajEtiketu.xaml"
            this.SubmitButton.Click += new System.Windows.RoutedEventHandler(this.DodajEtiketu_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.IzadjiButton = ((System.Windows.Controls.Button)(target));
            
            #line 64 "..\..\..\Modali\DodajEtiketu.xaml"
            this.IzadjiButton.Click += new System.Windows.RoutedEventHandler(this.IzadjiButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

