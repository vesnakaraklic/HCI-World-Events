using Projekat2020.Modali;
using Projekat2020.Modeli;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for ListaTipova.xaml
    /// </summary>
    public partial class ListaTipova : Page
    {
        private int brojac = 0;
        private List<Tip> listaSvihTipova = new List<Tip>();
        public ListaTipova()
        {
            InitializeComponent();
            TriTipa.ItemsSource = getListaTipova();
        }

        private List<Tip> getListaTipova()
        {
            XmlSerializer desr = new XmlSerializer(typeof(List<Tip>));
            using (StreamReader sr = new StreamReader(MainWindow.instance.putanjaTipa))
            {
                List<Tip> desrLista = (List<Tip>)desr.Deserialize(sr);
                listaSvihTipova = desrLista;
                sr.Close();
            }
            if (listaSvihTipova.Count() < 4)
                ArrowRight.IsEnabled = false;
            List<Tip> listaTipovaZaPrikaz = new List<Tip>();
            for (int i = brojac; i < (listaSvihTipova.Count() < 3 ? listaSvihTipova.Count() : brojac + 3); i++)
            {
                Tip pomocniTip = new Tip();
                pomocniTip.Id = listaSvihTipova[i].Id;
                pomocniTip.Opis = listaSvihTipova[i].Opis;
                pomocniTip.Ikonica = listaSvihTipova[i].Ikonica;
                pomocniTip.Naziv = listaSvihTipova[i].Naziv;
                listaTipovaZaPrikaz.Add(pomocniTip);
            }
            return listaTipovaZaPrikaz;

        }

        private void ArrowLeft_Click(object sender, RoutedEventArgs e)
        {
            brojac = brojac - 1;
            TriTipa.ItemsSource = getListaTipova();
            if (brojac == 0)
                ArrowLeft.IsEnabled = false;
            if (!ArrowRight.IsEnabled)
                ArrowRight.IsEnabled = true;
        }

        private void ArrowRight_Click(object sender, RoutedEventArgs e)
        {
            if (brojac == 0)
                ArrowLeft.IsEnabled = true;
            if (brojac == listaSvihTipova.Count() - 4)
                ArrowRight.IsEnabled = false;
            brojac = brojac + 1;
            TriTipa.ItemsSource = getListaTipova();
        }

        private void AddType_PageClick(object sender, RoutedEventArgs e)
        {
            MainWindow.instance.PanelContent.Content = new Stranice.DodajTip();
        }

        private void DeleteType_Click(object sender, RoutedEventArgs e)
        {
            YesNoModal modal = new YesNoModal((Tip)(e.OriginalSource as Button).DataContext,"Tip");
            modal.p1.Text = "Ukoliko obrisete tip, svi dogadjaji koji koriste ovaj tip ce biti takodje obrisani.";
            modal.p2.Text = "Da li ste sigurni da zelite da obrisete tip?";
            modal.ShowDialog();
        }

        private void EditType_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.instance.PanelContent.Content = new Stranice.DodajTip((Tip)(e.OriginalSource as Button).DataContext);
        }
        private void PrikaziTipa_Click(object sender, RoutedEventArgs e)
        {
            //PrikazTipa modal = new PrikazTipa((Tip)(e.OriginalSource as Button).DataContext);
            //modal.ShowDialog();
            MainWindow.instance.PanelContent.Content = new Stranice.PrikazTipa((Tip)(e.OriginalSource as Button).DataContext);
        }
    }
}
