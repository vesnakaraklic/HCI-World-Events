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
    /// Interaction logic for PrikazDogadjaja.xaml
    /// </summary>
    public partial class PrikazDogadjaja : Page
    {
        private Dogadjaj dogadjaj;
        public PrikazDogadjaja(Dogadjaj dogadjaj)
        {
            InitializeComponent();
            this.dogadjaj = dogadjaj;
            InitFields();
        }

        private void InitFields()
        {
            Uri icon = new Uri(dogadjaj.Ikonica, UriKind.RelativeOrAbsolute);
            JpegBitmapDecoder decoder2 = new JpegBitmapDecoder(icon, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource2 = decoder2.Frames[0];
            iconImage.ImageSource = bitmapSource2;

            naziv.Text = dogadjaj.Naziv;
            opis.Text = dogadjaj.Opis;
            grad.Text = dogadjaj.Grad;
            drzava.Text = dogadjaj.Drzava;
            datumPocetka.Text = dogadjaj.NaredniDatum.ToString();
            if (dogadjaj.Humanitaran == "Da")
            {
                humanitaran.Text = "Humanitaran dogadjaj";
                humanitaran.Visibility = Visibility.Visible;
            }
            tip.Text = dogadjaj.Tip.Naziv;
            switch (dogadjaj.Posecenost)
            {
                case 0:
                    posecenost.Text = " do 1000";
                    break;
                case 1:
                    posecenost.Text = "1000-5000";
                    break;
                case 2:
                    posecenost.Text = "5000-10000";
                    break;
                case 3:
                    posecenost.Text = "preko 10000";
                    break;
                default:
                    break;
            }
            cena.Text = dogadjaj.CenaTroskova.ToString() + "$";
            foreach (Object o in dogadjaj.IstorijaDatuma)
            {
                prethodniDatum.Text = prethodniDatum.Text + " " + ((DateTime)o).Day.ToString() + "." + ((DateTime)o).Month.ToString() + "." + ((DateTime)o).Year.ToString() + ";";
            }
            etikete.ItemsSource = dogadjaj.Etiketa;
        }

        private void ViewExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.instance.PanelContent.Content = new Stranice.ListaDogadjaja();
        }
    }
}
