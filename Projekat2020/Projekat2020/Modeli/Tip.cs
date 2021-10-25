using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Projekat2020.Modeli
{
    public class Tip
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Naziv")]
        public string Naziv { get; set; }

        [XmlElement("Ikonica")]
        public string Ikonica { get; set; }

        [XmlElement("Opis")]
        public string Opis { get; set; }

        public void delete()
        {
            //brisanje svih dogadjaja ovog tipa
            XmlSerializer desr1 = new XmlSerializer(typeof(List<Dogadjaj>));
            StreamReader sr1 = new StreamReader(MainWindow.instance.putanjaDogadjaja);
            List<Dogadjaj> listaDogadjaja = (List<Dogadjaj>)desr1.Deserialize(sr1);
            sr1.Close();
            foreach (Dogadjaj d in listaDogadjaja)
            {
                if (d.Tip.Id == Id)
                {
                    d.deleteFromXML();
                }
                
            }
            //brisanje svih dogadjajaXY ovog tipa (dogadjaja sa mape)
            XmlSerializer desr2 = new XmlSerializer(typeof(List<DogadjajXY>));
            StreamReader sr2 = new StreamReader(MainWindow.instance.putanjaDogadjajaXY);
            List<DogadjajXY> listaDogadjajaXY = (List<DogadjajXY>)desr2.Deserialize(sr2);
            sr2.Close();
            foreach (DogadjajXY d in listaDogadjajaXY)
            {
                if (d.Dogadjaj.Tip.Id == Id)
                {
                    d.delete();
                }
            }
            XmlSerializer desr3 = new XmlSerializer(typeof(List<DogadjajXY>));
            StreamReader sr3 = new StreamReader(MainWindow.instance.putanjaDogadjajaXY);
            List<DogadjajXY> listaDogadjajaXY1 = (List<DogadjajXY>)desr3.Deserialize(sr3);
            sr3.Close();
            MainWindow.instance.repaint(listaDogadjajaXY1);
            //brisanje tipa
            XmlSerializer desr = new XmlSerializer(typeof(List<Tip>));
            StreamReader sr = new StreamReader(MainWindow.instance.putanjaTipa);
            List<Tip> listaTipova = (List<Tip>)desr.Deserialize(sr);
            sr.Close();

            int i = 0;
            Tip tipZaBrisanje = new Tip();
            tipZaBrisanje.Id = this.Id;
            foreach (Tip t in listaTipova)
            {
                if (t.Id == tipZaBrisanje.Id)
                {
                    listaTipova.RemoveAt(i);
                    break;
                }
                i++;
            }

            StreamWriter sw = new StreamWriter(MainWindow.instance.putanjaTipa);
            XmlSerializer ser = new XmlSerializer(typeof(List<Tip>));
            ser.Serialize(sw, listaTipova);
            sw.Close();
            MainWindow.instance.PanelContent.Content = new Stranice.ListaTipova();
        }

        public void edit()
        {
            //izmena svih dogadjaja ovog tipa
            XmlSerializer desr1 = new XmlSerializer(typeof(List<Dogadjaj>));
            StreamReader sr1 = new StreamReader(MainWindow.instance.putanjaDogadjaja);
            List<Dogadjaj> listaDogadjaja = (List<Dogadjaj>)desr1.Deserialize(sr1);
            sr1.Close();
            foreach (Dogadjaj d in listaDogadjaja)
            {
                if (d.Tip.Id == Id)
                {
                    d.Tip.Ikonica = Ikonica;
                    d.Tip.Naziv = Naziv;
                    d.Tip.Opis = Opis;
                }

            }
            StreamWriter sw1 = new StreamWriter(MainWindow.instance.putanjaDogadjaja);
            XmlSerializer ser1 = new XmlSerializer(typeof(List<Dogadjaj>));
            ser1.Serialize(sw1, listaDogadjaja);
            sw1.Close();
            //izmena svih dogadjajaXY ovog tipa (dogadjaja sa mape)
            XmlSerializer desr2 = new XmlSerializer(typeof(List<DogadjajXY>));
            StreamReader sr2 = new StreamReader(MainWindow.instance.putanjaDogadjajaXY);
            List<DogadjajXY> listaDogadjajaXY = (List<DogadjajXY>)desr2.Deserialize(sr2);
            sr2.Close();
            foreach (DogadjajXY d in listaDogadjajaXY)
            {
                if (d.Dogadjaj.Tip.Id == Id)
                {
                    d.Dogadjaj.Tip.Ikonica = Ikonica;
                    d.Dogadjaj.Tip.Naziv = Naziv;
                    d.Dogadjaj.Tip.Opis = Opis;
                }
            }
            StreamWriter sw2 = new StreamWriter(MainWindow.instance.putanjaDogadjajaXY);
            XmlSerializer ser2 = new XmlSerializer(typeof(List<DogadjajXY>));
            ser2.Serialize(sw2, listaDogadjajaXY);
            sw2.Close();
            MainWindow.instance.repaint(listaDogadjajaXY);
            //izmena tipa
            XmlSerializer desr = new XmlSerializer(typeof(List<Tip>));
            StreamReader sr = new StreamReader(MainWindow.instance.putanjaTipa);
            List<Tip> listaTipova = (List<Tip>)desr.Deserialize(sr);
            sr.Close();
            
            foreach (Tip t in listaTipova)
            {
                if (t.Id == Id)
                {
                    t.Ikonica = Ikonica;
                    t.Naziv = Naziv;
                    t.Opis = Opis;
                    break;
                }
            }

            StreamWriter sw = new StreamWriter(MainWindow.instance.putanjaTipa);
            XmlSerializer ser = new XmlSerializer(typeof(List<Tip>));
            ser.Serialize(sw, listaTipova);
            sw.Close();
            MainWindow.instance.PanelContent.Content = new Stranice.ListaTipova();
        }
    
    }
}
