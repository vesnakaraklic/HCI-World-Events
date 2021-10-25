using Projekat2020.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Projekat2020.Modali
{
    /// <summary>
    /// Interaction logic for YesNoModal.xaml
    /// </summary>
    public partial class YesNoModal : Window
    {
        private Etiketa etiketa = new Etiketa();
        private Tip tip = new Tip();
        private Dogadjaj dogadjaj = new Dogadjaj();
        private string tipObjekta="";
        public YesNoModal(Object o, string s)
        {
            InitializeComponent();
            tipObjekta = s;
            switch (tipObjekta)
            {
                case "Etiketa":
                    etiketa = (Etiketa)o;
                    break;
                case "Tip":
                    tip = (Tip)o;
                    break;
                case "Dogadjaj":
                    dogadjaj = (Dogadjaj)o;
                    break;
                default:
                    break;
            }
        }
        private void Yes_Click(object sender, RoutedEventArgs e)
        {

            switch (tipObjekta)
            {
                case "Etiketa":
                    etiketa.delete();
                    break;
                case "Tip":
                    tip.delete();
                    break;
                case "Dogadjaj":
                    dogadjaj.deleteFromXML();
                    break;
                default:
                    break;
            }
            this.Close();
        }
        private void No_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
