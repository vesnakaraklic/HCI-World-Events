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
    /// Interaction logic for PrikazDogadjajXY.xaml
    /// </summary>
    public partial class PrikazDogadjajXY : Page
    {
        private DogadjajXY dogadjaj;
        public PrikazDogadjajXY(DogadjajXY dogadjaj)
        {
            InitializeComponent();
            this.dogadjaj = dogadjaj;
            InitFields();
        }

        private void InitFields()
        {
            Uri icon = new Uri(dogadjaj.Dogadjaj.Ikonica, UriKind.RelativeOrAbsolute);
            JpegBitmapDecoder decoder2 = new JpegBitmapDecoder(icon, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource2 = decoder2.Frames[0];
            iconImage.ImageSource = bitmapSource2;

            naziv.Text = dogadjaj.Dogadjaj.Naziv;
            opis.Text = dogadjaj.Dogadjaj.Opis;
            grad.Text = dogadjaj.Dogadjaj.Grad;
            drzava.Text = dogadjaj.Dogadjaj.Drzava;
            datumPocetka.Text = dogadjaj.Dogadjaj.NaredniDatum.ToString();
            if (dogadjaj.Dogadjaj.Humanitaran == "Da")
            {
                humanitaran.Text = "Humanitaran dogadjaj";
                humanitaran.Visibility = Visibility.Visible;
            }
            tip.Text = dogadjaj.Dogadjaj.Tip.Naziv;
            switch (dogadjaj.Dogadjaj.Posecenost)
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
            cena.Text = dogadjaj.Dogadjaj.CenaTroskova.ToString() + "$";
            foreach (Object o in dogadjaj.Dogadjaj.IstorijaDatuma)
            {
                prethodniDatum.Text = prethodniDatum.Text + " " + ((DateTime)o).Day.ToString() + "." + ((DateTime)o).Month.ToString() + "." + ((DateTime)o).Year.ToString() + ";";
            }
            etikete.ItemsSource = dogadjaj.Dogadjaj.Etiketa;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            dogadjaj.delete();
            dogadjaj.Dogadjaj.addToXML();
        }
    }
}
