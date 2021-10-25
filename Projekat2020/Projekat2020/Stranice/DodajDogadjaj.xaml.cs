using Microsoft.Win32;
using Projekat2020.Modali;
using Projekat2020.Modeli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// Interaction logic for DodajDogadjaj.xaml
    /// </summary>
    public partial class DodajDogadjaj : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static DodajDogadjaj instance;

        private List<object> listaDatuma = new List<object>();
        private string humanitarnost="";
        private string _dogadjajId;
        private bool idValid = true;
        private string _dogadjajNaziv;
        private bool nazivValid = true;
        private string _dogadjajDrzava;
        private string _dogadjajGrad;
        private bool gradValid = true;
        private string _dogadjajCena;
        private bool cenaValid = true;
        private string _selektovanTip="";
        private string putanjaIkonice;
        private bool editovanje = false;
        private List<Etiketa> selektovaneEtikete = new List<Etiketa>();

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public string DogadjajId
        {
            get
            {
                return _dogadjajId;
            }
            set
            {
                if (value != _dogadjajId)
                {
                    _dogadjajId = value;
                    OnPropertyChanged("DogadjajId");
                }
            }
        }

        public string DogadjajNaziv
        {
            get
            {
                return _dogadjajNaziv;
            }
            set
            {
                if (value != _dogadjajNaziv)
                {
                    _dogadjajNaziv = value;
                    OnPropertyChanged("DogadjajNaziv");
                }
            }
        }

        public string DogadjajGrad
        {
            get
            {
                return _dogadjajGrad;
            }
            set
            {
                if (value != _dogadjajGrad)
                {
                    _dogadjajGrad = value;
                    OnPropertyChanged("DogadjajGrad");
                }
            }
        }

        public string DogadjajCena
        {
            get
            {
                return _dogadjajCena;
            }
            set
            {
                if (value != _dogadjajCena)
                {
                    _dogadjajCena = value;
                    OnPropertyChanged("DogadjajCena");
                }
            }
        }

        public string SelektovanTip
        {
            get
            {
                return _selektovanTip;
            }
            set
            {
                if (value != _selektovanTip)
                {
                    _selektovanTip = value;
                    OnPropertyChanged("SelektovanTip");
                    validate();
                }
            }
        }
        public DodajDogadjaj()
        {
            if (instance == null)
            {
                instance = this;
            }
            InitializeComponent();
            this.NazivStranice.Text = "Dodaj Dogadjaj";
            this.DataContext = this;
            countryComboBox.ItemsSource = GetCountries();
            PrikaziTipove();
            PrikaziEtikete();
            CalendarDateRange cdr = new CalendarDateRange(DateTime.Today, DateTime.MaxValue);
            CalMultiple.BlackoutDates.Add(cdr);
            //CalMultiple.BlackoutDates.Add(MyCalendarDateRangePrevious);
            myDatePickerNext.BlackoutDates.AddDatesInPast();
        }

        public DodajDogadjaj(Dogadjaj dogadjaj)
        {
            if (instance == null)
            {
                instance = this;
            }
            InitializeComponent();
            this.NazivStranice.Text = "Izmeni Dogadjaj";
            this.DataContext = this;
            countryComboBox.ItemsSource = GetCountries();
            editovanje = true;
            PrikaziTipove();
            PrikaziEtikete();
            CalendarDateRange cdr = new CalendarDateRange(DateTime.Today, DateTime.MaxValue);
            CalMultiple.BlackoutDates.Add(cdr);
            myDatePickerNext.BlackoutDates.AddDatesInPast();
            InitFields(dogadjaj);
        }

        private void InitFields(Dogadjaj dogadjaj)
        {
            DogadjajId = dogadjaj.Id.ToString();
            DogadjajID.IsEnabled = false;
            DogadjajNaziv = dogadjaj.Naziv;
            DogadjajOpis.Text = dogadjaj.Opis;
            ListaComboBoxTipova.SelectedIndex = NadjiIndeksElementa(dogadjaj.Tip, getListaTipova());
            PosecenostDogadjaja.SelectedIndex = dogadjaj.Posecenost;
            putanjaIkonice = dogadjaj.Ikonica;
            Uri Mapa = new Uri(putanjaIkonice, UriKind.RelativeOrAbsolute);
            JpegBitmapDecoder decoder2 = new JpegBitmapDecoder(Mapa, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource2 = decoder2.Frames[0];

            IkonicaDogadjaja.Source = bitmapSource2;
            IkonicaDogadjaja.Stretch = Stretch.Uniform;

            if (dogadjaj.Humanitaran == "Da")
            {
                HumanitarnostDa.IsChecked = true;
                humanitarnost = "Da";
            }
            else
            {
                HumanitarnostNe.IsChecked = true;
                humanitarnost = "Ne";
            }
            DogadjajCena = dogadjaj.CenaTroskova.ToString();
            countryComboBox.SelectedItem = dogadjaj.Drzava;
            myDatePickerNext.SelectedDate = dogadjaj.NaredniDatum;
            foreach (Object o in dogadjaj.IstorijaDatuma)
            {
                listaDatuma.Add(o);
                CalMultiple.SelectedDates.Add((DateTime)o);
            }
            DogadjajGrad = dogadjaj.Grad;
            foreach (Etiketa et in ListaSvihEtiketa_ListBox.Items)
            {
                foreach (Etiketa eti in dogadjaj.Etiketa)
                {
                    if (eti.Id == et.Id)
                    {
                        ListaSvihEtiketa_ListBox.SelectedItems.Add(et);
                    }
                }

            }

        }

        private void DodajDogadjaj_Click(object sender, RoutedEventArgs e)
        {
            XmlSerializer desr = new XmlSerializer(typeof(List<Dogadjaj>));
            StreamReader sr = new StreamReader(MainWindow.instance.putanjaDogadjaja);
            List<Dogadjaj> listaDogadjaja = (List<Dogadjaj>)desr.Deserialize(sr);
            sr.Close();
            Dogadjaj trenutniDogadjaj = new Dogadjaj();
            trenutniDogadjaj.Id = int.Parse(DogadjajID.Text);
            trenutniDogadjaj.Naziv = DogadjajNAZIV.Text;
            trenutniDogadjaj.Opis = DogadjajOpis.Text;
            trenutniDogadjaj.Tip = (Tip)ListaComboBoxTipova.SelectedItem;
            trenutniDogadjaj.Posecenost = PosecenostDogadjaja.SelectedIndex;
            trenutniDogadjaj.Humanitaran = humanitarnost;
            trenutniDogadjaj.CenaTroskova = double.Parse(DogadjajCENATroskova.Text);
            trenutniDogadjaj.Drzava = countryComboBox.SelectedItem.ToString();
            trenutniDogadjaj.Grad = DogadjajGRAD.Text;
            List<object> listaIstorija = new List<object>();
            foreach(object o in CalMultiple.SelectedDates)
            {
                listaIstorija.Add(o);
            }
            trenutniDogadjaj.IstorijaDatuma = listaIstorija;
            trenutniDogadjaj.NaredniDatum = myDatePickerNext.SelectedDate.Value;
            if (putanjaIkonice == null || putanjaIkonice == string.Empty)
            {
                trenutniDogadjaj.Ikonica = ((Tip)ListaComboBoxTipova.SelectedItem).Ikonica;
                Console.WriteLine(((Tip)ListaComboBoxTipova.SelectedItem).Ikonica);
            }
            else
            {
                trenutniDogadjaj.Ikonica = putanjaIkonice;
            }

            trenutniDogadjaj.Etiketa = selektovaneEtikete;
            if (editovanje)
            {
                listaDogadjaja = getIzmenjenuListu(listaDogadjaja, trenutniDogadjaj);
            }
            else
            {
                listaDogadjaja.Add(trenutniDogadjaj);
            }
            StreamWriter sw = new StreamWriter(MainWindow.instance.putanjaDogadjaja);
            XmlSerializer ser = new XmlSerializer(typeof(List<Dogadjaj>));
            ser.Serialize(sw, listaDogadjaja);
            sw.Close();
            MainWindow.instance.PanelContent.Content = new Stranice.ListaDogadjaja();
            
        }

        private List<Dogadjaj> getIzmenjenuListu(List<Dogadjaj> listaDogadjaja, Dogadjaj dogadjaj)
        {
            for (int i = 0; i < listaDogadjaja.Count(); i++)
            {
                if (listaDogadjaja[i].Id == dogadjaj.Id)
                {
                    listaDogadjaja[i] = dogadjaj;
                }
            }
            return listaDogadjaja;
        }

        private void StackPanel_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }

        private void HumanitarnostDa_Click(object sender, RoutedEventArgs e)
        {
            humanitarnost = "Da";
            validate();
        }

        private void HumanitarnostNe_Click(object sender, RoutedEventArgs e)
        {
            humanitarnost = "Ne";
            validate();
        }

        private void Izaberi_slikuDogadjaja(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = "Deskop";
            fileDialog.Filter = "Image (*.jpg)|*.jpg|Image (*.png)|*.png";
            Nullable<bool> result = fileDialog.ShowDialog(); //tip koji moze biti null, kupi cancel i ok u zavisnosti od klika

            if (result == true)
            {
                var file = fileDialog.SafeFileName; //daje nam tacno ime fajla koji smo izabrali
                putanjaIkonice = fileDialog.FileName; //u tooltipu je cela putanja

                Uri Mapa = new Uri(putanjaIkonice, UriKind.RelativeOrAbsolute);
                JpegBitmapDecoder decoder2 = new JpegBitmapDecoder(Mapa, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                BitmapSource bitmapSource2 = decoder2.Frames[0];

                IkonicaDogadjaja.ToolTip = file;
                IkonicaDogadjaja.Source = bitmapSource2;
                IkonicaDogadjaja.Stretch = Stretch.Uniform;
            }
        }

        //lista svih drzava 
        public class LocalesRetrievalException : Exception
        {
            public LocalesRetrievalException(string message)
                : base(message)
            {
            }
        }

        #region Windows API

        private delegate bool EnumLocalesProcExDelegate(
           [MarshalAs(UnmanagedType.LPWStr)]String lpLocaleString,
           LocaleType dwFlags, int lParam);

        [DllImport(@"kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool EnumSystemLocalesEx(EnumLocalesProcExDelegate pEnumProcEx,
           LocaleType dwFlags, int lParam, IntPtr lpReserved);

        private enum LocaleType : uint
        {
            LocaleAll = 0x00000000,             // Enumerate all named based locales
            LocaleWindows = 0x00000001,         // Shipped locales and/or replacements for them
            LocaleSupplemental = 0x00000002,    // Supplemental locales only
            LocaleAlternateSorts = 0x00000004,  // Alternate sort locales
            LocaleNeutralData = 0x00000010,     // Locales that are "neutral" (language only, region data is default)
            LocaleSpecificData = 0x00000020,    // Locales that contain language and region data
        }

        #endregion

        public enum CultureTypes : uint
        {
            SpecificCultures = LocaleType.LocaleSpecificData,
            NeutralCultures = LocaleType.LocaleNeutralData,
            AllCultures = LocaleType.LocaleWindows
        }

        public static List<CultureInfo> GetCultures(CultureTypes cultureTypes)
        {
            List<CultureInfo> cultures = new List<CultureInfo>();
            EnumLocalesProcExDelegate enumCallback = (locale, flags, lParam) =>
            {
                try
                {
                    cultures.Add(new CultureInfo(locale));
                }
                catch (CultureNotFoundException)
                {
                    // This culture is not supported by .NET (not happened so far)
                    // Must be ignored.
                }
                return true;
            };

            if (EnumSystemLocalesEx(enumCallback, (LocaleType)cultureTypes, 0, (IntPtr)0) == false)
            {
                int errorCode = Marshal.GetLastWin32Error();
                throw new LocalesRetrievalException("Win32 error " + errorCode + " while trying to get the Windows locales");
            }

            // Add the two neutral cultures that Windows misses 
            // (CultureInfo.GetCultures adds them also):
            if (cultureTypes == CultureTypes.NeutralCultures || cultureTypes == CultureTypes.AllCultures)
            {
                cultures.Add(new CultureInfo("zh-CHS"));
                cultures.Add(new CultureInfo("zh-CHT"));
            }

            return cultures;
        }

        public static List<string> GetCountries()
        {
            List<CultureInfo> cultures = GetCultures(CultureTypes.SpecificCultures);
            List<string> countries = new List<string>();

            foreach (CultureInfo culture in cultures)
            {
                RegionInfo region = new RegionInfo(culture.Name);

                if (!(countries.Contains(region.EnglishName)))
                {
                    countries.Add(region.EnglishName);
                }
            }

            return countries;
        }
        private void PrikaziTipove()
        {
            ListaComboBoxTipova.ItemsSource = getListaTipova();
            ListaComboBoxTipova.DisplayMemberPath = "Naziv";
            //ListaComboBoxTipova.SelectedValuePath = "Naziv";
        }

        private void PrikaziEtikete()
        {
            ListaSvihEtiketa_ListBox.ItemsSource = getListaEtiketa();
            ListaSvihEtiketa_ListBox.SelectedValue = "Id";
            //ListaSvihEtiketa_ListBox.SelectedValuePath = "Id";
        }

        private List<Tip> getListaTipova()
        {
            XmlSerializer desr = new XmlSerializer(typeof(List<Tip>));
            List<Tip> listaTipovaZaPrikaz = new List<Tip>();
            using (StreamReader sr = new StreamReader(MainWindow.instance.putanjaTipa))
            {
                List<Tip> desrLista = (List<Tip>)desr.Deserialize(sr);
                listaTipovaZaPrikaz = desrLista;
                sr.Close();
            }
            return listaTipovaZaPrikaz;

        }

        private List<Etiketa> getListaEtiketa()
        {
            XmlSerializer desr = new XmlSerializer(typeof(List<Etiketa>));
            List<Etiketa> listaEtiketaZaPrikaz = new List<Etiketa>();
            using (StreamReader sr = new StreamReader(MainWindow.instance.putanjaEtikete))
            {
                List<Etiketa> desrLista = (List<Etiketa>)desr.Deserialize(sr);
                listaEtiketaZaPrikaz = desrLista;
                sr.Close();
            }
            return listaEtiketaZaPrikaz;
        }

        private void ListaComboBoxaTipova_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelektovanTip = (sender as ComboBox).SelectedValue.ToString();

        }

        private void ListaSvihEtiketa_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //selektovaneEtikete = new List<Etiketa>(e.AddedItems[0]);
            foreach (Etiketa et in e.AddedItems)
            {
                selektovaneEtikete.Add(et);

            }
            foreach (Etiketa et in e.RemovedItems)
            {
                selektovaneEtikete.Remove(et);

            }
        }

        private int NadjiIndeksElementa(Tip element, List<Tip> niz)
        {
            int i = 0;

            foreach (Tip e in niz)
            {
                if (element.Id == e.Id)
                {
                    return i;
                }

                i++;
            }
            return -1;
        }

        public void ValidateTrue(string id)
        {
            switch (id)
            {
                case "DogadjajId":
                    idValid = true;
                    break;
                case "DogadjajNaziv":
                    nazivValid = true;
                    break;
                case "DogadjajCena":
                    cenaValid = true;
                    break;
                case "DogadjajGrad":
                    gradValid = true;
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
                case "DogadjajId":
                    idValid = false;
                    break;
                case "DogadjajNaziv":
                    nazivValid = false;
                    break;
                case "DogadjajCena":
                    cenaValid = false;
                    break;
                case "DogadjajGrad":
                    gradValid = false;
                    break;
                default:
                    break;
            }
            validate();
        }
        public void validate()
        {
            if (!idValid || !nazivValid || !cenaValid || !gradValid || _selektovanTip == string.Empty || humanitarnost == string.Empty || countryComboBox.SelectedIndex == -1 || myDatePickerNext.SelectedDate == null)
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
            MainWindow.instance.PanelContent.Content = new Stranice.ListaDogadjaja();
        }

        private void CalMultiple_GotFocus(object sender, RoutedEventArgs e)
        {
            object o = new object();
            o = (e.OriginalSource as Button).DataContext.ToString();

            Console.WriteLine(o);
            bool postoji = false;
            int i = 0;
            foreach (Object ob in listaDatuma)
            {
                if ((Convert.ToDateTime(ob)).Year == (Convert.ToDateTime(o)).Year && (Convert.ToDateTime(ob)).Month == (Convert.ToDateTime(o)).Month && (Convert.ToDateTime(ob)).Day == (Convert.ToDateTime(o)).Day)
                {
                    postoji = true;
                    break;
                }
                i++;
            }
            if (postoji)
            {
                Console.WriteLine("Ima");
                listaDatuma.RemoveAt(i);
            } else
            {
                Console.WriteLine("Nema");
                listaDatuma.Add(o);
            }
            Console.WriteLine(listaDatuma);
            foreach (Object ob in listaDatuma)
            {
                CalMultiple.SelectedDates.Add(Convert.ToDateTime(ob));
                CalMultiple.DisplayMode = CalendarMode.Month;
            }
        }

        private void Help_ButtonIdDogadjaja_Click(object sender, RoutedEventArgs e)
        {
            //idHelpPopup.IsOpen = true;
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovo polje unesite id";
            modal.DrugoPoljeHelp.Text = "dogadjaja koji zelite da dodate.";
            modal.CetvrtoPoljeHelp.Text = "Unos ovog polja je obavezan!";
            modal.Show();
        }
        
        private void Help_ButtonNazivDogadjaja_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovo polje unesite naziv";
            modal.DrugoPoljeHelp.Text = "dogadjaja koji zelite da dodate.";
            modal.CetvrtoPoljeHelp.Text = "Unos ovog polja je obavezan!";
            modal.Show();
        }

        private void Help_ButtonOpisDogadjaja_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovo polje unesite opis";
            modal.DrugoPoljeHelp.Text = "dogadjaja koji zelite da dodate.";
            modal.CetvrtoPoljeHelp.Text = "Unos ovog polja je obavezan!";
            modal.Show();
        }

        private void Help_ButtonTipDogadjaja_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovom polju izaberite";
            modal.TrecePoljeHelp.Text = "koji zelite da dodate";
            modal.DrugoPoljeHelp.Text = "postojeci tip dogadjaja.";
            modal.CetvrtoPoljeHelp.Text = "Unos ovog polja je obavezan!";
            modal.Show();
        }

        private void Help_ButtonPosecenostDogadjaja_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovom polju izaberite ";
            modal.DrugoPoljeHelp.Text = "vrednost posecenosti dogadjaja ";
            modal.TrecePoljeHelp.Text = "koji zelite da dodate.";
            modal.CetvrtoPoljeHelp.Text = "Unos ovog polja je obavezan!";
            modal.Show();
        }

        private void Help_ButtonIkonicaDogadjaja_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovom polju izaberite sliku";
            modal.DrugoPoljeHelp.Text = "dogadjaja koji zelite da dodate.";
            modal.Show();
        }

        private void Help_ButtonHumanitarniDogadjaja_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovom polju oznacite ";
            modal.DrugoPoljeHelp.Text = "da li je dogadjaj koji zelite";
            modal.TrecePoljeHelp.Text = "da dodate humanitaran.";
            modal.CetvrtoPoljeHelp.Text = "Unos ovog polja je obavezan!";

            modal.Show();
        }

        private void Help_ButtonCenaDogadjaja_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovo polje unesite cenu troskova";
            modal.DrugoPoljeHelp.Text = "dogadjaja koji zelite da dodate.";
            modal.CetvrtoPoljeHelp.Text = "Unos ovog polja je obavezan!";
            modal.Show();
        }

        private void Help_ButtonDrzavaDogadjaja_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovom polju izaberite drzavu";
            modal.DrugoPoljeHelp.Text = "odrzavanja dogadjaja koji ";
            modal.TrecePoljeHelp.Text = "zelite da dodate.";
            modal.CetvrtoPoljeHelp.Text = "Unos ovog polja je obavezan!";
            modal.Show();
        }

        private void Help_ButtonGradDogadjaja_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovo polje unesite grad";
            modal.DrugoPoljeHelp.Text = "odabrane drzave u kojoj je";
            modal.TrecePoljeHelp.Text = "dogadjaj koji zelite dodati odrzava.";
            modal.CetvrtoPoljeHelp.Text = "Unos ovog polja je obavezan!";
            modal.Show();
        }

        private void Help_ButtonPrDatumDogadjaja_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U kalenadru odaberite prethodne ";
            modal.DrugoPoljeHelp.Text = "datume odrzavanja dogadjaja";
            modal.TrecePoljeHelp.Text = "koji zelite da dodate.";
            modal.CetvrtoPoljeHelp.Text = "Unos ovog polja je obavezan!";
            modal.Show();
        }

        private void Help_ButtonNxDatumDogadjaja_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U kalendaru odaberite sledeci ";
            modal.DrugoPoljeHelp.Text = "datum odrzavanja dogadjaja";
            modal.TrecePoljeHelp.Text = "koji zelite da dodate.";
            modal.CetvrtoPoljeHelp.Text = "Unos ovog polja je obavezan!";
            modal.Show();
        }

        private void Help_ButtonEtiketaDogadjaja_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovom polju odaberite ";
            modal.DrugoPoljeHelp.Text = "nula ili vise etikketa ";
            modal.TrecePoljeHelp.Text = "dogadjaja koji zelite da dodate.";
            modal.CetvrtoPoljeHelp.Text = "Unos ovog polja je obavezan!";
            modal.Show();
        }

        private void CountryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            validate();
        }

        private void MyDatePickerNext_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            validate();
        }
    }
}
