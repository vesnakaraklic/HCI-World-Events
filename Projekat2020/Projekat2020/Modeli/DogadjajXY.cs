using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Projekat2020.Modeli
{
    public class DogadjajXY
    {

        [XmlElement("Dogadjaj")]
        public Dogadjaj Dogadjaj { get; set; }

        [XmlElement("X")]
        public double X { get; set; }

        [XmlElement("Y")]
        public double Y { get; set; }

        public void delete()
        {
            XmlSerializer desr = new XmlSerializer(typeof(List<DogadjajXY>));
            StreamReader sr = new StreamReader(MainWindow.instance.putanjaDogadjajaXY);
            List<DogadjajXY> listaDogadjaja = (List<DogadjajXY>)desr.Deserialize(sr);
            sr.Close();

            int i = 0;
            foreach (DogadjajXY d in listaDogadjaja)
            {
                if (d.Dogadjaj.Id == this.Dogadjaj.Id)
                {
                    listaDogadjaja.RemoveAt(i);
                    break;
                }
                i++;
            }

            StreamWriter sw = new StreamWriter(MainWindow.instance.putanjaDogadjajaXY);
            XmlSerializer ser = new XmlSerializer(typeof(List<DogadjajXY>));
            ser.Serialize(sw, listaDogadjaja);
            sw.Close(); //pozvati repaint
            MainWindow.instance.PanelContent.Content = new Stranice.ListaDogadjaja();
            MainWindow.instance.repaint(listaDogadjaja);
        }
    }
}
