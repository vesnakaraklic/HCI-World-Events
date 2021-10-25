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
    /// Interaction logic for ListaEtiketa.xaml
    /// </summary>
    public partial class ListaEtiketa : Page
    {
        private int brojac = 0;
        private List<Etiketa> listaSvihEtiketa = new List<Etiketa>();
        public ListaEtiketa()
        {
            InitializeComponent();
            TriEtikete.ItemsSource=getListaEtiketa();
        }
        

        private List<Etiketa> getListaEtiketa()
        {   
            XmlSerializer desr = new XmlSerializer(typeof(List<Etiketa>));
            using (StreamReader sr = new StreamReader(MainWindow.instance.putanjaEtikete))
            {
                List<Etiketa> desrLista = (List<Etiketa>)desr.Deserialize(sr);
                listaSvihEtiketa = desrLista;
                sr.Close();
            }
            if (listaSvihEtiketa.Count() < 4)
                ArrowRight.IsEnabled = false;
            List<Etiketa> listaEtiketaZaPrikaz = new List<Etiketa>();
            for (int i = brojac; i<(listaSvihEtiketa.Count()< 3 ? listaSvihEtiketa.Count():brojac + 3); i++)
            {
                Etiketa pomocnaEtiketa = new Etiketa();
                pomocnaEtiketa.Id = listaSvihEtiketa[i].Id;
                pomocnaEtiketa.Boja = listaSvihEtiketa[i].Boja;
                pomocnaEtiketa.Opis = listaSvihEtiketa[i].Opis;
                listaEtiketaZaPrikaz.Add(pomocnaEtiketa);
            }
            return listaEtiketaZaPrikaz;

        }

        private void ArrowLeft_Click(object sender, RoutedEventArgs e)
        {
            brojac = brojac - 1;
            TriEtikete.ItemsSource = getListaEtiketa();
            if (brojac == 0)
                ArrowLeft.IsEnabled=false;
            if (!ArrowRight.IsEnabled)
                ArrowRight.IsEnabled = true;
        }

        private void ArrowRight_Click(object sender, RoutedEventArgs e)
        {
            if (brojac == 0)
                ArrowLeft.IsEnabled = true;
            if (brojac == listaSvihEtiketa.Count()-4)
                ArrowRight.IsEnabled = false;
            brojac = brojac + 1;
            TriEtikete.ItemsSource = getListaEtiketa();
        }

        private void AddLabel_PageClick(object sender, RoutedEventArgs e) {
            MainWindow.instance.PanelContent.Content = new Stranice.DodajEtiketu();
        }

        private void DeleteLabel_Click(object sender, RoutedEventArgs e)
        {
            YesNoModal modal = new YesNoModal((Etiketa)(e.OriginalSource as Button).DataContext, "Etiketa");
            modal.p1.Text = "Ukoliko obrisete etiketu koja se vec koristi u nekom dogadjaju, etiketa ce biti obrisana iz dogadjaja";
            modal.p2.Text = "Da li ste sigurni da zelite da obrisete etiketu?";
            modal.ShowDialog();
        }

        private void EditLabel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.instance.PanelContent.Content = new Stranice.DodajEtiketu((Etiketa)(e.OriginalSource as Button).DataContext);
        }
        private void PrikaziEtiketu_Click(object sender, RoutedEventArgs e)
        {
            // PrikazEtikete modal = new PrikazEtikete((Etiketa)(e.OriginalSource as Button).DataContext);
            //modal.ShowDialog();
            MainWindow.instance.PanelContent.Content = new Stranice.PrikazEtikete((Etiketa)(e.OriginalSource as Button).DataContext);
        }
    }
}
