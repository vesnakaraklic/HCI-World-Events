using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Projekat2020.Modeli
{
    public class Etiketa
    {

        
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Boja")]
        public string Boja { get; set; }

        [XmlElement("Opis")]
        public string Opis { get; set; }

        public void delete()
        {
            //brisanje etikete u dogadjajima
            XmlSerializer desr1 = new XmlSerializer(typeof(List<Dogadjaj>));
            StreamReader sr1 = new StreamReader(MainWindow.instance.putanjaDogadjaja);
            List<Dogadjaj> listaDogadjaja = (List<Dogadjaj>)desr1.Deserialize(sr1);
            sr1.Close();
            foreach(Dogadjaj d in listaDogadjaja)
            {
                int index = 0;
                foreach(Etiketa e in d.Etiketa)
                {
                    if(e.Id == Id)
                    {
                        d.Etiketa.RemoveAt(index);
                        break;
                    }
                    index++;
                }
            }
            StreamWriter sw1 = new StreamWriter(MainWindow.instance.putanjaDogadjaja);
            XmlSerializer ser1 = new XmlSerializer(typeof(List<Dogadjaj>));
            ser1.Serialize(sw1, listaDogadjaja);
            sw1.Close();
            MainWindow.instance.PanelContent.Content = new Stranice.ListaEtiketa();
            //brisanje etikete u dogadjajimaXY
            XmlSerializer desr2 = new XmlSerializer(typeof(List<DogadjajXY>));
            StreamReader sr2 = new StreamReader(MainWindow.instance.putanjaDogadjajaXY);
            List<DogadjajXY> listaDogadjajaXY = (List<DogadjajXY>)desr2.Deserialize(sr2);
            sr2.Close();
            foreach (DogadjajXY d in listaDogadjajaXY)
            {
                int index = 0;
                foreach (Etiketa e in d.Dogadjaj.Etiketa)
                {
                    if (e.Id == Id)
                    {
                        d.Dogadjaj.Etiketa.RemoveAt(index);
                        break;
                    }
                    index++;
                }
            }
            StreamWriter sw2 = new StreamWriter(MainWindow.instance.putanjaDogadjajaXY);
            XmlSerializer ser2 = new XmlSerializer(typeof(List<DogadjajXY>));
            ser2.Serialize(sw2, listaDogadjajaXY);
            sw2.Close();
            MainWindow.instance.repaint(listaDogadjajaXY);
            MainWindow.instance.PanelContent.Content = new Stranice.ListaEtiketa();
            //brisanje etikete
            XmlSerializer desr = new XmlSerializer(typeof(List<Etiketa>));
            StreamReader sr = new StreamReader(MainWindow.instance.putanjaEtikete);
            List<Etiketa> listaEtiketa = (List<Etiketa>)desr.Deserialize(sr);
            sr.Close();

            int i = 0;
            Etiketa etiketaZaBrisanje = new Etiketa();
            etiketaZaBrisanje.Id = this.Id;
            foreach (Etiketa e in listaEtiketa)
            {
                if (e.Id == etiketaZaBrisanje.Id)
                {
                    listaEtiketa.RemoveAt(i);
                    break;
                }
                i++;
            }

            StreamWriter sw = new StreamWriter(MainWindow.instance.putanjaEtikete);
            XmlSerializer ser = new XmlSerializer(typeof(List<Etiketa>));
            ser.Serialize(sw, listaEtiketa);
            sw.Close();
            MainWindow.instance.PanelContent.Content = new Stranice.ListaEtiketa();
        }
        public void edit()
        {
            //editovanje etikete u dogadjajima
            XmlSerializer desr1 = new XmlSerializer(typeof(List<Dogadjaj>));
            StreamReader sr1 = new StreamReader(MainWindow.instance.putanjaDogadjaja);
            List<Dogadjaj> listaDogadjaja = (List<Dogadjaj>)desr1.Deserialize(sr1);
            sr1.Close();
            foreach (Dogadjaj d in listaDogadjaja)
            {
                int index = 0;
                foreach (Etiketa e in d.Etiketa)
                {
                    if (e.Id == Id)
                    {
                        e.Id = this.Id;
                        e.Boja = this.Boja;
                        e.Opis = this.Opis;
                        break;
                    }
                    index++;
                }
            }
            StreamWriter sw1 = new StreamWriter(MainWindow.instance.putanjaDogadjaja);
            XmlSerializer ser1 = new XmlSerializer(typeof(List<Dogadjaj>));
            ser1.Serialize(sw1, listaDogadjaja);
            sw1.Close();
            //izmena etikete u dogadjajimaXY
            XmlSerializer desr2 = new XmlSerializer(typeof(List<DogadjajXY>));
            StreamReader sr2 = new StreamReader(MainWindow.instance.putanjaDogadjajaXY);
            List<DogadjajXY> listaDogadjajaXY = (List<DogadjajXY>)desr2.Deserialize(sr2);
            sr2.Close();
            foreach (DogadjajXY d in listaDogadjajaXY)
            {
                int index = 0;
                foreach (Etiketa e in d.Dogadjaj.Etiketa)
                {
                    if (e.Id == Id)
                    {
                        e.Id = this.Id;
                        e.Boja = this.Boja;
                        e.Opis = this.Opis;
                        break;
                    }
                    index++;
                }
            }
            StreamWriter sw2 = new StreamWriter(MainWindow.instance.putanjaDogadjajaXY);
            XmlSerializer ser2 = new XmlSerializer(typeof(List<DogadjajXY>));
            ser2.Serialize(sw2, listaDogadjajaXY);
            sw2.Close();
            MainWindow.instance.repaint(listaDogadjajaXY);
            //izmena etikete
            XmlSerializer desr = new XmlSerializer(typeof(List<Etiketa>));
            StreamReader sr = new StreamReader(MainWindow.instance.putanjaEtikete);
            List<Etiketa> listaEtiketa = (List<Etiketa>)desr.Deserialize(sr);
            sr.Close();

            int i = 0;
            Etiketa etiketaZaBrisanje = new Etiketa();
            etiketaZaBrisanje.Id = this.Id;
            foreach (Etiketa e in listaEtiketa)
            {
                if (e.Id == etiketaZaBrisanje.Id)
                {
                    e.Id = this.Id;
                    e.Boja = this.Boja;
                    e.Opis = this.Opis;
                    break;
                }
                i++;
            }

            StreamWriter sw = new StreamWriter(MainWindow.instance.putanjaEtikete);
            XmlSerializer ser = new XmlSerializer(typeof(List<Etiketa>));
            ser.Serialize(sw, listaEtiketa);
            sw.Close();
            MainWindow.instance.PanelContent.Content = new Stranice.ListaEtiketa();
        }
    }
}
