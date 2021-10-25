using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Projekat2020.Modali;
using System.Xml.Serialization;
using Projekat2020.Modeli;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Threading;
using Projekat2020.Stranice;

namespace Projekat2020
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        public bool demoOn = false;

        private Point dropPoint = new Point();
        private Boolean isOpen = false;
        private Boolean isOpenView = false;
        public string putanjaEtikete;
        public string putanjaTipa;
        public string putanjaDogadjaja;
        public string putanjaDogadjajaXY;
        public static MainWindow instance;
        private string _filterMapaText = "";
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public string FilterMapaText
        {
            get
            {
                return _filterMapaText;
            }
            set
            {
                if (value != _filterMapaText)
                {
                    _filterMapaText = value;
                    UpdateMapaList();
                    OnPropertyChanged("FilterMapaText");
                }
            }
        }

        public MainWindow()
        {
            if (instance == null)
            {
                instance = this;
            }
            InitializeComponent();
            this.DataContext = this;
            ProveraXmla();
            UcitajMapu();
            PanelContent.Content = new Stranice.ListaDogadjaja();
        }

        public void UcitajMapu()
        {
            var path = System.IO.Path.Combine(Environment.CurrentDirectory, "mapa.png");
            var uri = new Uri(path);
            var bitmap = new BitmapImage(uri);
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = bitmap;
            mapa.Background = ib;
            XmlSerializer desr = new XmlSerializer(typeof(List<DogadjajXY>));
            StreamReader sr = new StreamReader(MainWindow.instance.putanjaDogadjajaXY);
            List<DogadjajXY> listaDogadjaja = (List<DogadjajXY>)desr.Deserialize(sr);
            sr.Close();
            repaint(listaDogadjaja);
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
                GridSubMenu.Visibility = Visibility.Collapsed;
                isOpenView = false;
                isOpen = false;
        }

        private void MenuButtonAdd_Clicked(object sender, RoutedEventArgs e)
        {
            if (!isOpen)
            {
                GridSubMenu.Visibility = Visibility.Visible;
                AddSubMenu_Menubar.Visibility = Visibility.Visible;
                ViewSubMenu_MenuBar.Visibility = Visibility.Collapsed;
                isOpen = true;
                isOpenView = false;
            }
            else
            {
                GridSubMenu.Visibility = Visibility.Collapsed;
                AddSubMenu_Menubar.Visibility = Visibility.Collapsed;
                ViewSubMenu_MenuBar.Visibility = Visibility.Collapsed;
                isOpen = false;
            }

        }
        
        private void AddEvent_Modal(object sender, RoutedEventArgs e)
        {
            MainWindow.instance.PanelContent.Content = new Stranice.DodajDogadjaj();

        }

        private void AddLabel_Modal(object sender, RoutedEventArgs e)
        {
            MainWindow.instance.PanelContent.Content = new Stranice.DodajEtiketu();

        }

        private void AddType_Modal(object sender, RoutedEventArgs e)
        {
            MainWindow.instance.PanelContent.Content = new Stranice.DodajTip();

        }

        private void ViewEvent_Modal(object sender, RoutedEventArgs e)
        {
            MainWindow.instance.PanelContent.Content = new Stranice.ListaDogadjaja();

        }

        private void ViewLabel_Modal(object sender, RoutedEventArgs e)
        {
            MainWindow.instance.PanelContent.Content = new Stranice.ListaEtiketa();

        }

        private void ViewType_Modal(object sender, RoutedEventArgs e)
        {
            MainWindow.instance.PanelContent.Content = new Stranice.ListaTipova();

        }

        private void ButtonHelp_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.instance.PanelContent.Content = new Stranice.HelpMenu();

        }

        private void ButtonDogadjaj_Click(object sender, RoutedEventArgs e)
        {
            PanelContent.Content = new Stranice.ListaDogadjaja();
        }

        private void ButtonTip_Click(object sender, RoutedEventArgs e)
        {
            PanelContent.Content = new Stranice.ListaTipova();
        }

        private void ButtonEtiketa_Click(object sender, RoutedEventArgs e)
        {
            PanelContent.Content = new Stranice.ListaEtiketa();
        }

        private void ProveraXmla() {
            putanjaEtikete = Directory.GetCurrentDirectory() + @"\WorkspaceEtikete.xml";
            putanjaTipa = Directory.GetCurrentDirectory() + @"\WorkspaceTipa.xml";
            putanjaDogadjaja = Directory.GetCurrentDirectory() + @"\WorkspaceDogadjaja.xml";
            putanjaDogadjajaXY = Directory.GetCurrentDirectory() + @"\WorkspaceDogadjajaXY.xml";

            if (!File.Exists(putanjaEtikete))
            {
                StreamWriter sw1 = new StreamWriter(MainWindow.instance.putanjaEtikete);
                XmlSerializer ser1 = new XmlSerializer(typeof(List<Etiketa>));
                List<Etiketa> listaEtiketa = new List<Etiketa>();
                ser1.Serialize(sw1, listaEtiketa);
                sw1.Close();
            }
            if (!File.Exists(putanjaTipa))
            {
                StreamWriter sw2 = new StreamWriter(MainWindow.instance.putanjaTipa);
                XmlSerializer ser2 = new XmlSerializer(typeof(List<Tip>));
                List<Tip> listaTipova = new List<Tip>();
                ser2.Serialize(sw2, listaTipova);
                sw2.Close();
            }

            if (!File.Exists(putanjaDogadjaja))
            {
                StreamWriter sw = new StreamWriter(MainWindow.instance.putanjaDogadjaja);
                XmlSerializer ser = new XmlSerializer(typeof(List<Dogadjaj>));
                List<Dogadjaj> listaDogadjaja = new List<Dogadjaj>();
                ser.Serialize(sw, listaDogadjaja);
                sw.Close();
            }

            if (!File.Exists(putanjaDogadjajaXY))
            {
                StreamWriter sw3 = new StreamWriter(MainWindow.instance.putanjaDogadjajaXY);
                XmlSerializer ser3 = new XmlSerializer(typeof(List<DogadjajXY>));
                List<DogadjajXY> listaDogadjajaXy = new List<DogadjajXY>();
                ser3.Serialize(sw3, listaDogadjajaXy);
                sw3.Close();
            }
        }
        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private void Mapa_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("dndDogadjaj") || e.Source == sender)
            {
                e.Effects = DragDropEffects.None;
                dropPoint = e.GetPosition(mapa);
                Console.WriteLine(dropPoint);
            }
        }

        private void Mapa_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("dndDogadjaj"))
            {
                XmlSerializer desr = new XmlSerializer(typeof(List<DogadjajXY>));
                StreamReader sr = new StreamReader(MainWindow.instance.putanjaDogadjajaXY);
                List<DogadjajXY> listaDogadjaja = (List<DogadjajXY>)desr.Deserialize(sr);
                sr.Close();
                Point canvasPositon = Mouse.GetPosition(mapa);
                Dogadjaj dogadjaj = e.Data.GetData("dndDogadjaj") as Dogadjaj;
                DogadjajXY dogadjajXY = new DogadjajXY();
                dogadjajXY.Dogadjaj=dogadjaj;
                dogadjajXY.X = dropPoint.X;
                dogadjajXY.Y = dropPoint.Y;
                if (dropPoint.X > 12 && dropPoint.Y > 16 && dropPoint.X < 410 && dropPoint.Y < 323)
                {
                    if (listaDogadjaja.Count > 0)
                    {
                        List<DogadjajXY> novaLista = new List<DogadjajXY>(listaDogadjaja);
                        bool mestoValid = true;
                        foreach (DogadjajXY xy in listaDogadjaja)
                        {
                            if (Math.Abs(xy.X - dogadjajXY.X) < 30 && Math.Abs(xy.Y - dogadjajXY.Y) < 35)
                            {
                                mestoValid = false;
                            }

                        }
                        if (mestoValid) {

                            novaLista.Add(dogadjajXY);
                            StreamWriter sw = new StreamWriter(putanjaDogadjajaXY);
                            XmlSerializer ser = new XmlSerializer(typeof(List<DogadjajXY>));
                            ser.Serialize(sw, novaLista);
                            sw.Close();
                            dogadjaj.deleteFromXML();
                            repaint(novaLista);
                        } else
                        {
                            DropText modal = new DropText();
                            modal.PrvoPoljeHelp.Text = "Ikonice dogadjaja se ne mogu";
                            modal.DrugoPoljeHelp.Text = "preklapati, pomerite ikonicu";
                            modal.TrecePoljeHelp.Text = "na dozvoljenu lokaciju na mapi.";
                            modal.Show(); 
                        }
                    }
                    else {
                        listaDogadjaja.Add(dogadjajXY);
                        StreamWriter sw = new StreamWriter(putanjaDogadjajaXY);
                        XmlSerializer ser = new XmlSerializer(typeof(List<DogadjajXY>));
                        ser.Serialize(sw, listaDogadjaja);
                        sw.Close();
                        dogadjaj.deleteFromXML();
                        repaint(listaDogadjaja);
                    }
                }
                else {
                    DropText modal = new DropText();
                    modal.PrvoPoljeHelp.Text = "Ikonice dogadjaja je van";
                    modal.DrugoPoljeHelp.Text = "dozvoljenog okvira, pomerite";
                    modal.TrecePoljeHelp.Text = "ikonicu unutar okvira.";
                    modal.Show();
                }
            }
        }

        public void repaint(List<DogadjajXY> dogadjaji)
        {
            mapa.Children.Clear();
            foreach (DogadjajXY dogadjaj in dogadjaji)
            {
                Rectangle r = new Rectangle();
                r.Height = 30;
                r.Width = 30;
                r.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#700416"));
                Canvas.SetTop(r, dogadjaj.Y - 38);
                Canvas.SetLeft(r, dogadjaj.X - 15);
                mapa.Children.Add(r);
                Polygon p = new Polygon();
                p.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#700416"));
                p.Points.Add(new Point(dogadjaj.X, dogadjaj.Y));
                p.Points.Add(new Point(dogadjaj.X - 5, dogadjaj.Y - 15));
                p.Points.Add(new Point(dogadjaj.X + 5, dogadjaj.Y - 15));
                mapa.Children.Add(p);
                BitmapImage theImage = new BitmapImage
                (new Uri(dogadjaj.Dogadjaj.Ikonica, UriKind.Relative));

                ImageBrush myImageBrush = new ImageBrush(theImage);
                Canvas cv = new Canvas();
                cv.Width = 26;
                cv.Height = 26;
                cv.Background = myImageBrush;
                cv.MouseDown += prikazDogadjaja;
                cv.DataContext = dogadjaj;
                Canvas.SetTop(cv, dogadjaj.Y - 36);
                Canvas.SetLeft(cv, dogadjaj.X - 13);
                mapa.Children.Add(cv);
            }
        }

        private void prikazDogadjaja(object sender, MouseButtonEventArgs e)
        {
            
            PanelContent.Content = new Stranice.PrikazDogadjajXY(((sender as Canvas).DataContext) as DogadjajXY);
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            PanelContent.Content = new Stranice.Pretraga();
        }

        private void ButtonView_Click(object sender, RoutedEventArgs e)
        {
            if (!isOpen)
            {
                GridSubMenu.Visibility = Visibility.Visible;
                AddSubMenu_Menubar.Visibility = Visibility.Collapsed;
                ViewSubMenu_MenuBar.Visibility = Visibility.Visible;
                isOpen = true;
                isOpenView = false;
            }
            else
            {
                GridSubMenu.Visibility = Visibility.Collapsed;
                AddSubMenu_Menubar.Visibility = Visibility.Collapsed;
                ViewSubMenu_MenuBar.Visibility = Visibility.Collapsed;
                isOpen = false;
            }
        }

        private void UpdateMapaList() {
            XmlSerializer desr = new XmlSerializer(typeof(List<DogadjajXY>));
            StreamReader sr = new StreamReader(MainWindow.instance.putanjaDogadjajaXY);
            List<DogadjajXY> listaDogadjajaNaMapi = (List<DogadjajXY>)desr.Deserialize(sr);
            sr.Close();

            List<DogadjajXY> zaPrikazNaMapi = new List<DogadjajXY>();

            foreach (DogadjajXY dogadjaj in listaDogadjajaNaMapi) {
                if (dogadjaj.Dogadjaj.Naziv.ToLower().Contains(_filterMapaText.ToLower())) {
                    zaPrikazNaMapi.Add(dogadjaj);
                }
            }

            if (_filterMapaText == "") {
                zaPrikazNaMapi = listaDogadjajaNaMapi;
            }
            repaint(zaPrikazNaMapi);
        }

        private void ButtonPretragaMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.instance.PanelContent.Content = new Stranice.Pretraga();
        }

        private void ButtonDemoMenu_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(UpdateText);

            thread.Start();
        }
        private void UpdateText()
        {
            Dispatcher.Invoke(new Action(() => {
                MouseMove(Convert.ToInt16(Application.Current.MainWindow.Left) + 250, Convert.ToInt16(Application.Current.MainWindow.Top) + 300);
                HelpModal modal = new HelpModal();
                modal.PrvoPoljeHelp.Text = "Pokrenut je demonstrativni mod";
                modal.DrugoPoljeHelp.Text = "za dodavanje etikete.";
                modal.Show();
            }), DispatcherPriority.ContextIdle);
            Thread.Sleep(4000);
            Dispatcher.Invoke(new Action(() => {
                LeftMouseClick(Convert.ToInt16(Application.Current.MainWindow.Left) + 15, Convert.ToInt16(Application.Current.MainWindow.Top) + 15);
                PanelContent.Content = new Stranice.ListaEtiketa();
            }), DispatcherPriority.ContextIdle);
            Thread.Sleep(1000);
            Dispatcher.Invoke(new Action(() => {
                LeftMouseClick(Convert.ToInt16(Application.Current.MainWindow.Left) + 15, Convert.ToInt16(Application.Current.MainWindow.Top) + 15);
                PanelContent.Content = new Stranice.ListaEtiketa();
            }), DispatcherPriority.ContextIdle);
            Thread.Sleep(1000);
            Dispatcher.Invoke(new Action(() => {
                MouseMove(Convert.ToInt16(Application.Current.MainWindow.Left) + 410, Convert.ToInt16(Application.Current.MainWindow.Top) + 95);
            }), DispatcherPriority.ContextIdle);
            Thread.Sleep(1000);
            Dispatcher.Invoke(new Action(() => {
                LeftMouseClick(Convert.ToInt16(Application.Current.MainWindow.Left) + 410, Convert.ToInt16(Application.Current.MainWindow.Top) + 95);
            }), DispatcherPriority.ContextIdle);
            Thread.Sleep(1000);
            Dispatcher.Invoke(new Action(() => {
                MouseMove(Convert.ToInt16(Application.Current.MainWindow.Left) + 310, Convert.ToInt16(Application.Current.MainWindow.Top) + 140);
            }), DispatcherPriority.ContextIdle);
            Thread.Sleep(1000);
            Dispatcher.Invoke(new Action(() => {
                MouseMove(Convert.ToInt16(Application.Current.MainWindow.Left) + 310, Convert.ToInt16(Application.Current.MainWindow.Top) + 140);
                DodajEtiketu etiketa = new DodajEtiketu();
                PanelContent.Content = etiketa;
                etiketa.EtiketaId = "9";
            }), DispatcherPriority.ContextIdle);
            Thread.Sleep(500);
            Dispatcher.Invoke(new Action(() => {
                MouseMove(Convert.ToInt16(Application.Current.MainWindow.Left) + 310, Convert.ToInt16(Application.Current.MainWindow.Top) + 140);
                DodajEtiketu etiketa = new DodajEtiketu();
                PanelContent.Content = etiketa;
                etiketa.EtiketaId = "99";
            }), DispatcherPriority.ContextIdle);
            Thread.Sleep(1000);
            Dispatcher.Invoke(new Action(() => {
                MouseMove(Convert.ToInt16(Application.Current.MainWindow.Left) + 350, Convert.ToInt16(Application.Current.MainWindow.Top) + 178);
            }), DispatcherPriority.ContextIdle);
            Thread.Sleep(1000);
            Dispatcher.Invoke(new Action(() => {
                LeftMouseClick(Convert.ToInt16(Application.Current.MainWindow.Left) + 350, Convert.ToInt16(Application.Current.MainWindow.Top) + 178);
            }), DispatcherPriority.ContextIdle);
            Thread.Sleep(1000);
            Dispatcher.Invoke(new Action(() => {
                MouseMove(Convert.ToInt16(Application.Current.MainWindow.Left) + 190, Convert.ToInt16(Application.Current.MainWindow.Top) + 359);
            }), DispatcherPriority.ContextIdle);
            Thread.Sleep(1000);
            Dispatcher.Invoke(new Action(() => {
                LeftMouseClick(Convert.ToInt16(Application.Current.MainWindow.Left) + 190, Convert.ToInt16(Application.Current.MainWindow.Top) + 359);
            }), DispatcherPriority.ContextIdle);
            Thread.Sleep(1000);
            Dispatcher.Invoke(new Action(() => {
                MouseMove(Convert.ToInt16(Application.Current.MainWindow.Left) + 330, Convert.ToInt16(Application.Current.MainWindow.Top) + 215);
            }), DispatcherPriority.ContextIdle);
            Thread.Sleep(500);
            string ovoJeDemo = "Ovo je demo funkcija.";
            string zaIspis = "";
            foreach(char c in ovoJeDemo)
            {
                Dispatcher.Invoke(new Action(() => {
                    MouseMove(Convert.ToInt16(Application.Current.MainWindow.Left) + 330, Convert.ToInt16(Application.Current.MainWindow.Top) + 215);
                    zaIspis = zaIspis + c;
                    DodajEtiketu etiketa = new DodajEtiketu();
                    PanelContent.Content = etiketa;
                    etiketa.EtiketaId = "99";
                    etiketa.EtiketaBoja.SelectedColor = (Color)ColorConverter.ConvertFromString("#FFF0E68C");

                    etiketa.EtiketaOpis.Text = zaIspis;
                    etiketa.SubmitButton.IsEnabled = true;
                }), DispatcherPriority.ContextIdle);
                Thread.Sleep(200);
            }
            Dispatcher.Invoke(new Action(() => {
                MouseMove(Convert.ToInt16(Application.Current.MainWindow.Left) + 350, Convert.ToInt16(Application.Current.MainWindow.Top) + 290);
            }), DispatcherPriority.ContextIdle);
            Thread.Sleep(1000);
            Dispatcher.Invoke(new Action(() => {
                LeftMouseClick(Convert.ToInt16(Application.Current.MainWindow.Left) + 350, Convert.ToInt16(Application.Current.MainWindow.Top) + 290);
            }), DispatcherPriority.ContextIdle);
            Thread.Sleep(1000);
            Dispatcher.Invoke(new Action(() => {
                MouseMove(Convert.ToInt16(Application.Current.MainWindow.Left) + 350, Convert.ToInt16(Application.Current.MainWindow.Top) + 290);
            }), DispatcherPriority.ContextIdle);
            Thread.Sleep(2000);
            Dispatcher.Invoke(new Action(() => {
                XmlSerializer desr = new XmlSerializer(typeof(List<Etiketa>));
                StreamReader sr = new StreamReader(MainWindow.instance.putanjaEtikete);
                List<Etiketa> listaEtiketa = (List<Etiketa>)desr.Deserialize(sr);
                sr.Close();
                listaEtiketa.RemoveAt(listaEtiketa.Count - 1);
                StreamWriter sw = new StreamWriter(MainWindow.instance.putanjaEtikete);
                XmlSerializer ser = new XmlSerializer(typeof(List<Etiketa>));
                ser.Serialize(sw, listaEtiketa);
                sw.Close();
                MainWindow.instance.PanelContent.Content = new Stranice.ListaEtiketa();
            }), DispatcherPriority.ContextIdle);
            //Application.Current.Dispatcher.Invoke((Action)delegate {
            //});
            Dispatcher.Invoke(new Action(() => {
                MouseMove(Convert.ToInt16(Application.Current.MainWindow.Left) + 250, Convert.ToInt16(Application.Current.MainWindow.Top) + 300);
                HelpModal modal = new HelpModal();
                modal.PrvoPoljeHelp.Text = "Demonstrativni mod je zavrsen";
                modal.Show();
            }), DispatcherPriority.ContextIdle);
            Thread.Sleep(4000);
        }
        //This simulates a left mouse click
        public static void LeftMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }
        public static void MouseMove(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
        }


    }
}
