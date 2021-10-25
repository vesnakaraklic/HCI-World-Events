using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Projekat2020.Modeli
{
    public class Dogadjaj
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Naziv")]
        public string Naziv { get; set; }

        [XmlElement("Opis")]
        public string Opis { get; set; }

        [XmlElement("Tip")]
        public Tip Tip { get; set; }

        [XmlElement("Posecenost")]
        public int Posecenost { get; set; }

        [XmlElement("Ikonica")]
        public string Ikonica { get; set; }

        [XmlElement("Humanitaran")]
        public string Humanitaran { get; set; }

        [XmlElement("Drzava")]
        public string Drzava { get; set; }

        [XmlElement("Grad")]
        public string Grad { get; set; }

        [XmlElement("IstorijaDatuma")]
        public List<Object> IstorijaDatuma { get; set; }

        [XmlElement("CenaTroskova")]
        public double CenaTroskova { get; set; }

        [XmlElement("NaredniDatum")]
        public DateTime NaredniDatum { get; set; }

        [XmlElement("Etiketa")]
        public List<Etiketa> Etiketa { get; set; }

        public void deleteFromXML()
        {
            XmlSerializer desr = new XmlSerializer(typeof(List<Dogadjaj>));
            StreamReader sr = new StreamReader(MainWindow.instance.putanjaDogadjaja);
            List<Dogadjaj> listaDogadjaja = (List<Dogadjaj>)desr.Deserialize(sr);
            sr.Close();

            int i = 0;
            Dogadjaj dogadjajZaBrisanje = new Dogadjaj();
            dogadjajZaBrisanje.Id = this.Id;
            foreach (Dogadjaj d in listaDogadjaja)
            {
                if (d.Id == dogadjajZaBrisanje.Id)
                {
                    listaDogadjaja.RemoveAt(i);
                    break;
                }
                i++;
            }

            StreamWriter sw = new StreamWriter(MainWindow.instance.putanjaDogadjaja);
            XmlSerializer ser = new XmlSerializer(typeof(List<Dogadjaj>));
            ser.Serialize(sw, listaDogadjaja);
            sw.Close();
            MainWindow.instance.PanelContent.Content = new Stranice.ListaDogadjaja();
        }

        internal void edit()
        {
            throw new NotImplementedException();
        }

        public void addToXML()
        {
            XmlSerializer desr = new XmlSerializer(typeof(List<Dogadjaj>));
            StreamReader sr = new StreamReader(MainWindow.instance.putanjaDogadjaja);
            List<Dogadjaj> listaDogadjaja = (List<Dogadjaj>)desr.Deserialize(sr);
            sr.Close();

            listaDogadjaja.Add(this);

            StreamWriter sw = new StreamWriter(MainWindow.instance.putanjaDogadjaja);
            XmlSerializer ser = new XmlSerializer(typeof(List<Dogadjaj>));
            ser.Serialize(sw, listaDogadjaja);
            sw.Close();
            MainWindow.instance.PanelContent.Content = new Stranice.ListaDogadjaja();
        }
    }
}
