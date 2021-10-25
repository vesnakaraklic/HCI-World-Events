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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekat2020.Stranice
{
    /// <summary>
    /// Interaction logic for PrikazEtikete.xaml
    /// </summary>
    public partial class PrikazEtikete : Page
    {
        private Etiketa etiketa;
        public PrikazEtikete(Etiketa etiketa)
        {
            InitializeComponent();
            this.etiketa = etiketa;
            InitFields();
        }

        private void InitFields()
        {
            boja.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(etiketa.Boja);
            opis.Text = etiketa.Opis;
        }

        private void ViewExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.instance.PanelContent.Content = new Stranice.ListaEtiketa();
        }
    }
}
