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
    /// Interaction logic for DodajEtiketu.xaml
    /// </summary>
    public partial class DodajEtiketu : Page
    {
        public static DodajEtiketu instance;

        public event PropertyChangedEventHandler PropertyChanged;

        private string _etiketaId = "";
        private string _etiketaboja = "";
        private bool editovanje = false;
        private bool idValid = false;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public string EtiketaId
        {
            get
            {
                return _etiketaId;
            }
            set
            {
                if (value != _etiketaId)
                {
                    _etiketaId = value;
                    validate();
                    OnPropertyChanged("EtiketaId");
                }
            }
        }

        public string Boja
        {
            get
            {
                return _etiketaboja;
            }
            set
            {
                if (value != _etiketaboja)
                {
                    _etiketaboja = value;
                    validate();
                    OnPropertyChanged("Boja");
                }
            }
        }

        public DodajEtiketu()
        {
            if (instance == null)
            {
                instance = this;
            }
            InitializeComponent();
            this.NazivStranice.Text = "Dodaj Etiketu";
            this.DataContext = this;
            validate();
        }

        public DodajEtiketu(Etiketa etiketa)
        {
            if (instance == null)
            {
                instance = this;
            }
            InitializeComponent();
            this.NazivStranice.Text = "Izmeni Etiketu";
            editovanje = true;
            InitFields(etiketa);
            this.DataContext = this;
            validate();
        }

        private void InitFields(Etiketa etiketa)
        {
            EtiketaId = etiketa.Id.ToString();
            idValid = true;
            EtiketaID.IsEnabled = false;
            Boja = etiketa.Boja;
            EtiketaBoja.SelectedColor = (Color)ColorConverter.ConvertFromString(etiketa.Boja);
            EtiketaOpis.Text = etiketa.Opis;
        }

        private void DodajEtiketu_Click(object sender, RoutedEventArgs e)
        {
            XmlSerializer desr = new XmlSerializer(typeof(List<Etiketa>));
            StreamReader sr = new StreamReader(MainWindow.instance.putanjaEtikete);
            List<Etiketa> listaEtiketa = (List<Etiketa>)desr.Deserialize(sr);
            sr.Close();

            Etiketa novaEtiketa = new Etiketa();
            novaEtiketa.Id = int.Parse(EtiketaID.Text);
            novaEtiketa.Boja = EtiketaBoja.SelectedColor.ToString();
            novaEtiketa.Opis = EtiketaOpis.Text;

            if (editovanje)
            {
                novaEtiketa.edit();
            }
            else
            {
                listaEtiketa.Add(novaEtiketa);
                StreamWriter sw = new StreamWriter(MainWindow.instance.putanjaEtikete);
                XmlSerializer ser = new XmlSerializer(typeof(List<Etiketa>));
                ser.Serialize(sw, listaEtiketa);
                sw.Close();
                MainWindow.instance.PanelContent.Content = new Stranice.ListaEtiketa();
            }
        }

        private List<Etiketa> getIzmenjenuListuEtiketa(List<Etiketa> listaEtiketa, Etiketa etiketa)
        {
            for (int i = 0; i < listaEtiketa.Count(); i++)
            {
                if (listaEtiketa[i].Id == etiketa.Id)
                {
                    listaEtiketa[i] = etiketa;
                }
            }
            return listaEtiketa;
        }
        public void ValidateTrue(string id)
        {
            switch (id)
            {
                case "EtiketaId":
                    idValid = true;
                    break;
                default:
                    break;
            }
        }
        public void ValidateFalse(string id)
        {
            switch (id)
            {
                case "EtiketaId":
                    idValid = false;
                    break;
                default:
                    break;
            }
        }
        public void validate()
        {
            if (!idValid || _etiketaboja == string.Empty)
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
            MainWindow.instance.PanelContent.Content = new Stranice.ListaEtiketa();
        }

        private void EtiketaBoja_Closed(object sender, RoutedEventArgs e)
        {
            Boja = EtiketaBoja.SelectedColor.ToString();
            Console.WriteLine(Boja);
        }

        private void Help_ButtonIdEtikete_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovo polje unesite id";
            modal.DrugoPoljeHelp.Text = "etikete koju zelite da dodate.";
            modal.CetvrtoPoljeHelp.Text = "Unos ovog polja je obavezan!";
            modal.Show();
        }

        private void Help_ButtonBojaEtikete_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovom polju odaberite boju";
            modal.DrugoPoljeHelp.Text = "etikete koju zelite da dodate.";
            modal.CetvrtoPoljeHelp.Text = "Unos ovog polja je obavezan!";
            modal.Show();
        }

        private void Help_ButtonOpisEtikete_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovo polje unesite opis";
            modal.DrugoPoljeHelp.Text = "etikete koju zelite da dodate.";
            modal.CetvrtoPoljeHelp.Text = "Unos ovog polja je obavezan!";
            modal.Show();
        }
    }
    
}
