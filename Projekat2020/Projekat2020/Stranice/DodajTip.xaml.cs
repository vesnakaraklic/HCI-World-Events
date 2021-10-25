using Microsoft.Win32;
using Projekat2020.Modali;
using Projekat2020.Modeli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using System.Xml.Serialization;

namespace Projekat2020.Stranice
{
    /// <summary>
    /// Interaction logic for DodajTip.xaml
    /// </summary>
    public partial class DodajTip : Page
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static DodajTip instance;
        private string _tipId = "";
        private string _tipNaziv = "";
        private string _putanjaIkonice;
        private bool editovanje = false;
        private bool idValid = false;
        private bool nazivValid = false;
        // "C:\\Users\\vesna\\Desktop\\HCI2020\\defaultType.jpg";
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public string TipId
        {
            get
            {
                return _tipId;
            }
            set
            {
                if (value != _tipId)
                {
                    _tipId = value;
                    validate();
                    OnPropertyChanged("TipId");
                }
            }
        }

        public string TipNaziv
        {
            get
            {
                return _tipNaziv;
            }
            set
            {
                if (value != _tipNaziv)
                {
                    _tipNaziv = value;
                    validate();
                    OnPropertyChanged("TipNaziv");

                }
            }
        }

        public string PutanjaIkonice
        {
            get
            {
                return _putanjaIkonice;
            }
            set
            {
                if (value != _putanjaIkonice)
                {
                    _putanjaIkonice = value;
                    OnPropertyChanged("PutanjaIkonice");
                    validate();
                }
            }
        }
        public DodajTip()
        {
            if (instance == null)
            {
                instance = this;
            }
            InitializeComponent();
            this.NazivStranice.Text = "Dodaj Tip";
            this.DataContext = this;

            validate();
        }

        public DodajTip(Tip tip)
        {
            if (instance == null)
            {
                instance = this;
            }
            InitializeComponent();
            editovanje = true;
            this.NazivStranice.Text = "Izmeni Tip";
            this.DataContext = this;
            InitFields(tip);

        }

        private void InitFields(Tip tip)
        {
            TipId = tip.Id.ToString();
            idValid = true;
            TipID.IsEnabled = false;
            TipNaziv = tip.Naziv;
            nazivValid = true;
            TipOpis.Text = tip.Opis;
            Uri Mapa = new Uri(tip.Ikonica, UriKind.RelativeOrAbsolute);
            JpegBitmapDecoder decoder2 = new JpegBitmapDecoder(Mapa, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource2 = decoder2.Frames[0];

            PutanjaIkonice = tip.Ikonica;
            validate();
            IkonicaTipaDogadjaja.Source = bitmapSource2;
            IkonicaTipaDogadjaja.Stretch = Stretch.Uniform;
        }

        private void Izaberi_slikuTipa(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = "Deskop";
            fileDialog.Filter = "Image (*.jpg)|*.jpg|Image (*.png)|*.png";
            Nullable<bool> result = fileDialog.ShowDialog(); //tip koji moze biti null, kupi cancel i ok u zavisnosti od klika
            if (result == true)
            {
                var file = fileDialog.SafeFileName; //daje nam tacno ime fajlaa koji smo izabrali
                PutanjaIkonice = fileDialog.FileName; //u tooltipu je cela putanja

                Uri Mapa = new Uri(PutanjaIkonice, UriKind.RelativeOrAbsolute);
                JpegBitmapDecoder decoder2 = new JpegBitmapDecoder(Mapa, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                BitmapSource bitmapSource2 = decoder2.Frames[0];

                IkonicaTipaDogadjaja.ToolTip = file;
                IkonicaTipaDogadjaja.Source = bitmapSource2;
                IkonicaTipaDogadjaja.Stretch = Stretch.Uniform;
            }
        }

        private void DodajTip_Click(object sender, RoutedEventArgs e)
        {
            XmlSerializer desr = new XmlSerializer(typeof(List<Tip>));
            StreamReader sr = new StreamReader(MainWindow.instance.putanjaTipa);
            List<Tip> listaTipova = (List<Tip>)desr.Deserialize(sr);
            sr.Close();

            Tip noviTip = new Tip();
            noviTip.Id = int.Parse(TipID.Text);
            noviTip.Naziv = TipNAZIV.Text;
            noviTip.Opis = TipOpis.Text;
            noviTip.Ikonica = PutanjaIkonice;
            if (editovanje)
            {
                noviTip.edit();
            }
            else
            {
                listaTipova.Add(noviTip);
                StreamWriter sw = new StreamWriter(MainWindow.instance.putanjaTipa);
                XmlSerializer ser = new XmlSerializer(typeof(List<Tip>));
                ser.Serialize(sw, listaTipova);
                sw.Close();
                MainWindow.instance.PanelContent.Content = new Stranice.ListaTipova();
            }

        }

        private List<Tip> getIzmenjenuListuTipa(List<Tip> listaTipova, Tip tip)
        {
            for (int i = 0; i < listaTipova.Count(); i++)
            {
                if (listaTipova[i].Id == tip.Id)
                {
                    listaTipova[i] = tip;
                }
            }
            return listaTipova;
        }
        public void ValidateTrue(string id)
        {
            switch (id)
            {
                case "TipId":
                    idValid = true;
                    break;
                case "TipNaziv":
                    nazivValid = true;
                    break;
                default:
                    break;
            }
            validate();
        }
        public void ValidateFalse(string id)
        {
            switch (id)
            {
                case "TipId":
                    idValid = false;
                    break;
                case "TipNaziv":
                    nazivValid = false;
                    break;
                default:
                    break;
            }
            validate();
        }
        public void validate()
        {
            if (!idValid || !nazivValid || PutanjaIkonice == null || _tipId == "" || _tipNaziv == "")
            {
                SubmitButton.IsEnabled = false;
            }
            else
            {
                SubmitButton.IsEnabled = true;
            }
        }

        private void IzadjiButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.instance.PanelContent.Content = new Stranice.ListaTipova();
        }

        private void Help_ButtonIdTipa_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovo polje unesite id";
            modal.DrugoPoljeHelp.Text = "tipa koji zelite da dodate.";
            modal.CetvrtoPoljeHelp.Text = "Unos ovog polja je obavezan!";
            modal.Show();
        }

        private void Help_ButtonNazivTipa_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovo polje unesite naziv";
            modal.DrugoPoljeHelp.Text = "tipa koji zelite da dodate.";
            modal.CetvrtoPoljeHelp.Text = "Unos ovog polja je obavezan!";
            modal.Show();
        }

        private void Help_ButtonIkonicaTipa_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovom polju izaberite sliku";
            modal.DrugoPoljeHelp.Text = "tipa koji zelite da dodate.";
            modal.CetvrtoPoljeHelp.Text = "Unos ovog polja je obavezan!";
            modal.Show();
        }

        private void Help_ButtonOpisEtikete_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovo polje unesite opis";
            modal.DrugoPoljeHelp.Text = "tipa koji zelite da dodate.";
            modal.Show();
        }
    }
}
