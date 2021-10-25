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
    /// Interaction logic for Pretraga.xaml
    /// </summary>
    public partial class Pretraga : Page, INotifyPropertyChanged
    {
        public static Pretraga instance;
        private string humanitarnost="";
        private string _selektovanTip;
        private string _pretragaCena;
        private bool searchEmpty = true;
        private List<object> listaDatuma = new List<object>();
        public event PropertyChangedEventHandler PropertyChanged;
        private List<Etiketa> selektovaneEtikete = new List<Etiketa>();

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
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
                }
            }
        }

        public string PretragaCena
        {
            get
            {
                return _pretragaCena;
            }
            set
            {
                if (value != _pretragaCena)
                {
                    _pretragaCena = value;
                    OnPropertyChanged("PretragaCena");
                }
            }
        }

        public Pretraga()
        {
            if(instance == null)
            {
                instance = this;
            }
            InitializeComponent();
            this.Title = "Dodaj Dogadjaj";
            this.DataContext = this;
            countryComboBox.ItemsSource = GetCountries();
            PrikaziTipove();
            PrikaziEtikete();
            CalendarDateRange cdr = new CalendarDateRange(DateTime.Today, DateTime.MaxValue);
            /*CalMultiple.BlackoutDates.Add(cdr);
            PretragamyDatePickerNext.BlackoutDates.AddDatesInPast();*/
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

        private void PrikaziTipove()
        {
            ListaComboBoxPretragaTip.ItemsSource = getListaTipova();
            ListaComboBoxPretragaTip.DisplayMemberPath = "Naziv";
        }

        private void PrikaziEtikete()
        {
            ListaSvihEtiketa_ListBox.ItemsSource = getListaEtiketa();
            ListaSvihEtiketa_ListBox.SelectedValue = "Id";
        }
        private void ListaComboBoxaTipovaPretraga_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelektovanTip = (sender as ComboBox).SelectedValue.ToString();
        }

        /*private void CalMultiple_GotFocus(object sender, RoutedEventArgs e)
        {
            object o = new object();
            o = (e.OriginalSource as Button).DataContext.ToString();

            Console.WriteLine(o);
            bool postoji = false;
            int i = 0;
            foreach (Object ob in listaDatuma)
            {
                if ((Convert.ToDateTime(ob)).Year == (Convert.ToDateTime(o)).Year && (Convert.ToDateTime(ob)).Month == (Convert.ToDateTime(o)).Month &&
                    (Convert.ToDateTime(ob)).Day == (Convert.ToDateTime(o)).Day)
                {
                    postoji = true;
                    break;
                }
                i++;
            }
            if (postoji)
            {
                listaDatuma.RemoveAt(i);
            }
            else
            {
                listaDatuma.Add(o);
            }
            Console.WriteLine(listaDatuma);
            foreach (Object ob in listaDatuma)
            {
                CalMultiple.SelectedDates.Add(Convert.ToDateTime(ob));
            }
        }*/

        private void IzadjiButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.instance.PanelContent.Content = new Stranice.ListaDogadjaja();
        }

        private void HumanitarnostDa_Click(object sender, RoutedEventArgs e)
        {
            humanitarnost = "Da";
        }

        private void HumanitarnostNe_Click(object sender, RoutedEventArgs e)
        {
            humanitarnost = "Ne";
        }

        private void ListaSvihEtiketa_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Etiketa et in e.AddedItems)
            {
                selektovaneEtikete.Add(et);
            }
            foreach (Etiketa et in e.RemovedItems)
            {
                selektovaneEtikete.Remove(et);
            }
        }
        private void Help_ButtonPretragaNaziv_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovo polje unesite naziv";
            modal.DrugoPoljeHelp.Text = "dogadjaja koji zelite da pronadjete";
            modal.Show();
        }

        private void Help_ButtonPretragaOpis_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovo polje unesite opis";
            modal.DrugoPoljeHelp.Text = "dogadjaja koji zelite da pronadjete";
            modal.Show();
        }

        private void Help_ButtonPretragaPosecenost_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovom polju odaberite posecenost";
            modal.DrugoPoljeHelp.Text = "dogadjaja koji zelite da pronadjete";
            modal.Show();
        }

        private void Help_ButtonPretragaHumanitarni_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovom polju odaberite karakter";
            modal.DrugoPoljeHelp.Text = "dogadjaja koji zelite da pronadjete";
            modal.Show();
        }

        private void Help_ButtonPretragaCena_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovo polje unesite cenu";
            modal.DrugoPoljeHelp.Text = "dogadjaja koji zelite da pronadjete";
            modal.Show();
        }

        private void Help_ButtonPretragaDrzava_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovom polju odaberite drzavu";
            modal.DrugoPoljeHelp.Text = "dogadjaja koji zelite da pronadjete";
            modal.Show();
        }

        private void Help_ButtonPretragaGrad_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovo polje unesite grad";
            modal.DrugoPoljeHelp.Text = "dogadjaja koji zelite da pronadjete";
            modal.Show();
        }

        private void Help_ButtonPretragaPrDatum_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovom polju odaberite datume";
            modal.DrugoPoljeHelp.Text = "dogadjaja koji zelite da pronadjete";
            modal.Show();
        }

        private void Help_ButtonPretragaNxDatum_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovom polju odaberite datum";
            modal.DrugoPoljeHelp.Text = "dogadjaja koji zelite da pronadjete";
            modal.Show();
        }

        private void Help_ButtonPretragaEtiketa_Click(object sender, RoutedEventArgs e)
        {
            HelpModal modal = new HelpModal();
            modal.PrvoPoljeHelp.Text = "U ovom polje odaberite etikete";
            modal.DrugoPoljeHelp.Text = "dogadjaja koji zelite da pronadjete";
            modal.Show();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            searchEmpty = true;
            List<Dogadjaj> listaSvihDogadjaja = new List<Dogadjaj>();
            XmlSerializer desr = new XmlSerializer(typeof(List<Dogadjaj>));
            using (StreamReader sr = new StreamReader(MainWindow.instance.putanjaDogadjaja))
            {
                List<Dogadjaj> desrLista = (List<Dogadjaj>)desr.Deserialize(sr);
                listaSvihDogadjaja = desrLista;
                sr.Close();
            }
            List<Dogadjaj> listaPretrazenihDogadjaja = new List<Dogadjaj>();
            foreach (Dogadjaj d in listaSvihDogadjaja)
            {
                if (isNazivValid(d.Naziv) && isOpisValid(d.Opis) && isGradValid(d.Grad) && isDrzavaValid(d.Drzava) && isPosecenostValid(d.Posecenost)
                    && isHumanitaranValid(d.Humanitaran) && isCenaValid(d.CenaTroskova) && isTipValid(d.Tip) && isEtiketaValid(d.Etiketa))
                {
                    listaPretrazenihDogadjaja.Add(d);
                }
            }
            if (searchEmpty)
            {
                listaPretrazenihDogadjaja = listaSvihDogadjaja;
            }
            MainWindow.instance.PanelContent.Content = new Stranice.ListaDogadjaja(listaPretrazenihDogadjaja);
        }

        /*private bool isIstorijaValid(List<Object> datumi)
        {
            if (CalMultiple.SelectedDates.Count != 0)
            {
                searchEmpty = false;
                List<object> selectedIstorija = new List<object>();
                foreach (object o in CalMultiple.SelectedDates)
                {
                    selectedIstorija.Add(o);
                }
                foreach(object o1 in selectedIstorija)
                {
                    Console.WriteLine(o1.ToString());
                    foreach(object o2 in datumi)
                    {
                        if (o1 == o2)
                            return true;
                    }
                }
                return false;
            }
            return true;
        }*/

        private bool isEtiketaValid(List<Etiketa> etikete)
        {
            if (selektovaneEtikete.Count != 0)
            {
                searchEmpty = false;
                int valid = 0;
                foreach(Etiketa e in etikete)
                {
                    foreach(Etiketa e1 in selektovaneEtikete)
                    {
                        if (e.Id == e1.Id)
                        {
                            valid++;
                        }
                    }
                }
                if (valid == selektovaneEtikete.Count)
                    return true;
                return false;
            }
            return true;
        }

        private bool isPosecenostValid(int posecenost)
        {
            if (PretragaPosecenost.SelectedIndex != -1)
            {
                searchEmpty = false;
                if (posecenost == PretragaPosecenost.SelectedIndex)
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        private bool isTipValid(Tip tip)
        {
            if(!Object.ReferenceEquals((Tip)ListaComboBoxPretragaTip.SelectedItem, null))
            {
                searchEmpty = false;
                if (tip.Naziv == ((Tip)ListaComboBoxPretragaTip.SelectedItem).Naziv)
                {
                    return true;
                }
                return false;
            }
                return true;
        }

        private bool isCenaValid(double db)
        {
            if (!isNullOrEmpty(_pretragaCena))
            {
                if (db < Convert.ToDouble(_pretragaCena))
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        private bool isHumanitaranValid(string str)
        {
            if (!isNullOrEmpty(humanitarnost.ToString().ToLower()))
            {
                if (str.ToLower() == (humanitarnost.ToString().ToLower()))
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        private bool isDrzavaValid(string str)
        {
            if (!isNullOrEmpty(countryComboBox.SelectedItem == null?"": countryComboBox.SelectedItem.ToString())) { 
                if (str.ToLower() == countryComboBox.SelectedItem.ToString().ToLower())
                {
                    return true;
                }
                searchEmpty = false;
                Console.Write("IsDrzava valid" + str);
                return false;
            }
            return true;

        }

        private bool isGradValid(string str)
        {
            if (!isNullOrEmpty(PretragaGrad.Text.ToLower()))
            {
                if (str.ToLower().Contains(PretragaGrad.Text.ToLower()))
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        private bool isNazivValid(string str)
        {
            if (!isNullOrEmpty(PretragaNaziv.Text.ToLower()))
            {
                if (str.ToLower().Contains(PretragaNaziv.Text.ToLower()))
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        private bool isOpisValid(string str)
        {
            if (!isNullOrEmpty(PretragaOpis.Text.ToLower()))
            {
                if (str.ToLower().Contains(PretragaOpis.Text.ToLower()))
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        private bool isNullOrEmpty(string str)
        {
            if (str != null && str!="") {
                searchEmpty = false;
                return false;
            }
            return true;
        }
    }
}
