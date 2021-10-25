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
    /// Interaction logic for ListaDogadjaja.xaml
    /// </summary>
    public partial class ListaDogadjaja : Page, INotifyPropertyChanged
    {
        Point startPoint = new Point();
        private Dogadjaj dndDogadjaj = new Dogadjaj();
        private Border dndAncestory = new Border();

        private int brojac = 0;
        public event PropertyChangedEventHandler PropertyChanged;
        private List<Dogadjaj> listaSvihDogadjaja = new List<Dogadjaj>();
        private List<Dogadjaj> listaFilterDogadjajaZaPrikaz = new List<Dogadjaj>();
        private string _searchText = "";
        private string _filterText = "";

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                if (value != _searchText)
                {
                    _searchText = value;
                    UpdateDogadjajList();
                    OnPropertyChanged("SearchText");
                }
            }
        }

        public string FilterText
        {
            get
            {
                return _filterText;
            }
            set
            {
                if (value != _filterText)
                {
                    _filterText = value;
                    UpdateDogadjajList();
                    OnPropertyChanged("FilterText");
                }
            }
        }
        public ListaDogadjaja()
        {
            InitializeComponent();
            this.DataContext = this;
            
            XmlSerializer desr = new XmlSerializer(typeof(List<Dogadjaj>));
            using (StreamReader sr = new StreamReader(MainWindow.instance.putanjaDogadjaja))
            {
                List<Dogadjaj> desrLista = (List<Dogadjaj>)desr.Deserialize(sr);
                listaSvihDogadjaja = desrLista;
                listaFilterDogadjajaZaPrikaz = desrLista;
                sr.Close();
            }
            TriDogadjaja.ItemsSource = getListaDogadjaja(listaFilterDogadjajaZaPrikaz);
        }

        public ListaDogadjaja(List<Dogadjaj> pretrazeniDogadjaji)
        {
            InitializeComponent();
            this.DataContext = this;

            XmlSerializer desr = new XmlSerializer(typeof(List<Dogadjaj>));
            using (StreamReader sr = new StreamReader(MainWindow.instance.putanjaDogadjaja))
            {
                List<Dogadjaj> desrLista = (List<Dogadjaj>)desr.Deserialize(sr);
                listaSvihDogadjaja = desrLista;
                listaFilterDogadjajaZaPrikaz = desrLista;
                sr.Close();
            }
            TriDogadjaja.ItemsSource = getListaDogadjaja(pretrazeniDogadjaji);
        }


        private List<Dogadjaj> getListaDogadjaja(List<Dogadjaj> listaDogadjaja)
        {
            if (listaDogadjaja.Count() < 4)
                ArrowRight.IsEnabled = false;
            List<Dogadjaj> listaDogadjajaZaPrikaz = new List<Dogadjaj>();
            for (int i = brojac; i < (listaDogadjaja.Count() < 3 ? listaDogadjaja.Count() : brojac + 3); i++)
            {
                Dogadjaj pomocniDogadjaj = new Dogadjaj();
                pomocniDogadjaj.Id = listaDogadjaja[i].Id;
                pomocniDogadjaj.Naziv = listaDogadjaja[i].Naziv;
                pomocniDogadjaj.Opis = listaDogadjaja[i].Opis;
                pomocniDogadjaj.Tip = listaDogadjaja[i].Tip;
                pomocniDogadjaj.Posecenost = listaDogadjaja[i].Posecenost;
                pomocniDogadjaj.Ikonica = listaDogadjaja[i].Ikonica;
                pomocniDogadjaj.Humanitaran = listaDogadjaja[i].Humanitaran;
                pomocniDogadjaj.Drzava = listaDogadjaja[i].Drzava;
                pomocniDogadjaj.Grad = listaDogadjaja[i].Grad;
                pomocniDogadjaj.IstorijaDatuma = listaDogadjaja[i].IstorijaDatuma;
                pomocniDogadjaj.CenaTroskova = listaDogadjaja[i].CenaTroskova;
                pomocniDogadjaj.NaredniDatum = listaDogadjaja[i].NaredniDatum;
                pomocniDogadjaj.Etiketa = listaDogadjaja[i].Etiketa;
                listaDogadjajaZaPrikaz.Add(pomocniDogadjaj);
            }
            return listaDogadjajaZaPrikaz;

        }

            private void UpdateDogadjajList() {
            listaFilterDogadjajaZaPrikaz = new List<Dogadjaj>();
            foreach (Dogadjaj dogadjaj in listaSvihDogadjaja) {
                if (dogadjaj.Naziv.ToLower().Contains(_filterText.ToLower()) || dogadjaj.Opis.ToLower().Contains(_filterText.ToLower()) 
                    || dogadjaj.Grad.ToLower().Contains(_filterText.ToLower()) || dogadjaj.Drzava.ToLower().Contains(_filterText.ToLower()) 
                    || dogadjaj.CenaTroskova.ToString().ToLower().Contains(_filterText.ToLower()) || dogadjaj.Humanitaran.ToLower().Contains(_filterText.ToLower()) 
                    || dogadjaj.Tip.Naziv.ToString().ToLower().Contains(_filterText.ToLower()) || isPosecenostValid(dogadjaj.Posecenost) || dogadjaj.NaredniDatum.ToString().ToLower().Contains(_filterText.ToLower())) {
                    listaFilterDogadjajaZaPrikaz.Add(dogadjaj);
                }
            }

            if (listaFilterDogadjajaZaPrikaz.Count() > 3) {
                ArrowRight.IsEnabled = true;
            }
            ArrowLeft.IsEnabled = false;
            brojac = 0;
            TriDogadjaja.ItemsSource = getListaDogadjaja(listaFilterDogadjajaZaPrikaz);
        }

        private bool isPosecenostValid(int posecenostId) {
            if (posecenostId == 0) { 
                if ("do 1000".Contains(_filterText.ToLower()))
                    return true;
                return false;
            }
            else if (posecenostId == 1) { 
                if ("1000-5000".Contains(_filterText.ToLower()))
                    return true;
                return false;
            }
            else if (posecenostId == 2) { 
                if ("5000-10000".Contains(_filterText.ToLower()))
                    return true;
                return false;
            }
            else if (posecenostId == 3) { 
                if ("preko 10000".Contains(_filterText.ToLower()))
                    return true;
                return false;
            }
            else 
                return false;
        }
        private void ArrowLeft_Click(object sender, RoutedEventArgs e)
        {
            brojac = brojac - 1;
            TriDogadjaja.ItemsSource = getListaDogadjaja(listaFilterDogadjajaZaPrikaz);
            if (brojac == 0)
                ArrowLeft.IsEnabled = false;
            if (!ArrowRight.IsEnabled)
                ArrowRight.IsEnabled = true;
        }

        private void ArrowRight_Click(object sender, RoutedEventArgs e)
        {
            if (brojac == 0)
                ArrowLeft.IsEnabled = true;
            if (brojac == listaFilterDogadjajaZaPrikaz.Count() - 4)
                ArrowRight.IsEnabled = false;
            brojac = brojac + 1;
            TriDogadjaja.ItemsSource = getListaDogadjaja(listaFilterDogadjajaZaPrikaz);
        }

        private void AddEvent_PageClick(object sender, RoutedEventArgs e)
        {
            MainWindow.instance.PanelContent.Content = new Stranice.DodajDogadjaj();
        }

        private void DeleteEvent_Click(object sender, RoutedEventArgs e) {

            YesNoModal modal = new YesNoModal((Dogadjaj)(e.OriginalSource as Button).DataContext, "Dogadjaj");
            modal.p1.Text = "Da li ste sigurni da zelite da obrisete dogadjaj?";
            modal.ShowDialog();
        }

        private void EditEvent_Click(object sender, RoutedEventArgs e) {
            MainWindow.instance.PanelContent.Content = new Stranice.DodajDogadjaj((Dogadjaj)(e.OriginalSource as Button).DataContext);
        }

        private void PrikaziDogadjaj_Click(object sender, RoutedEventArgs e)
        {
            //PrikazDogadjaja modal = new PrikazDogadjaja((Dogadjaj)(e.OriginalSource as Button).DataContext);
            // modal.ShowDialog();
            MainWindow.instance.PanelContent.Content = new Stranice.PrikazDogadjaja((Dogadjaj)(e.OriginalSource as Button).DataContext);
        }

        private void TriDogadjaja_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
            dndDogadjaj = ((sender as Border).DataContext) as Dogadjaj;
            dndAncestory = sender as Border;
        }

        private void TriDogadjaja_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Initialize the drag & drop operation
                DataObject dragData = new DataObject("dndDogadjaj", dndDogadjaj);
                DragDrop.DoDragDrop(dndAncestory, dragData, DragDropEffects.Move);
            }
        }
    }
}
