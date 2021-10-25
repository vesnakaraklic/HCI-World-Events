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
    /// Interaction logic for PrikazTipa.xaml
    /// </summary>
    public partial class PrikazTipa : Page
    {
        private Tip tip;
        public PrikazTipa(Tip tip)
        {
            InitializeComponent();
            this.tip = tip;
            InitFields();
        }

        private void InitFields()
        {
            Uri icon = new Uri(tip.Ikonica, UriKind.RelativeOrAbsolute);
            JpegBitmapDecoder decoder2 = new JpegBitmapDecoder(icon, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource2 = decoder2.Frames[0];
            iconImage.ImageSource = bitmapSource2;

            naziv.Text = tip.Naziv;
            opis.Text = tip.Opis;
        }
        private void ViewExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.instance.PanelContent.Content = new Stranice.ListaTipova();
        }
    }
}
