using Projekat2020.Stranice;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Controls;
using System.Xml.Serialization;
using Projekat2020.Modeli;

namespace Projekat2020.Validacija
{
    public class StringValidationRule : ValidationRule
    {
        public string Id { get; set; } = "";
        public string Objekat { get; set; } = "";

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {

                var s = value as string;
                bool containsInt = s.Any(char.IsDigit);
                if (!containsInt)
                {
                    switch (Objekat)
                    {
                        case "Etiketa":
                            DodajEtiketu.instance.ValidateTrue(Id);
                            break;
                        case "Dogadjaj":
                            DodajDogadjaj.instance.ValidateTrue(Id);
                            break;
                        case "Tip":
                            DodajTip.instance.ValidateTrue(Id);
                            break;
                        default:
                            break;
                    }
                    Console.WriteLine("4");
                    return new ValidationResult(true, null);
                }
                switch (Objekat)
                {
                    case "Etiketa":
                        DodajEtiketu.instance.ValidateFalse(Id);
                        break;
                    case "Dogadjaj":
                        DodajDogadjaj.instance.ValidateFalse(Id);
                        break;
                    case "Tip":
                        DodajTip.instance.ValidateFalse(Id);
                        break;
                    default:
                        break;
                }
                Console.WriteLine("5");
                return new ValidationResult(false, "Mozete koristiti samo slova.");

            }
            catch
            {
                switch (Objekat)
                {
                    case "Etiketa":
                        DodajEtiketu.instance.ValidateFalse(Id);
                        break;
                    case "Dogadjaj":
                        DodajDogadjaj.instance.ValidateFalse(Id);
                        break;
                    case "Tip":
                        DodajTip.instance.ValidateFalse(Id);
                        break;
                    default:
                        break;
                }
                Console.WriteLine("6");
                return new ValidationResult(false, "Nepoznata greska se pojavila");
            }
        }
    }

    public class NumberValidationRule : ValidationRule
    {
        public string Id { get; set; } = "";
        public string Objekat { get; set; } = "";

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                Console.WriteLine(Id);
                Console.WriteLine(Objekat);
                Console.WriteLine(value);
                var s = value as string;
                bool isInt = s.All(char.IsDigit);
                if (isInt)
                {
                    Console.WriteLine(Id);
                    if(Id == "EtiketaId" || Id == "TipId" || Id == "DogadjajId")
                    {
                        switch (Id)
                        {
                            case "EtiketaId":
                                XmlSerializer desrEtiketa = new XmlSerializer(typeof(List<Etiketa>));
                                StreamReader srEtiketa = new StreamReader(MainWindow.instance.putanjaEtikete);
                                List<Etiketa> listaEtiketa = (List<Etiketa>)desrEtiketa.Deserialize(srEtiketa);
                                srEtiketa.Close();
                                foreach (Etiketa e in listaEtiketa)
                                {
                                    if (e.Id.ToString() == value.ToString())
                                    {
                                        return new ValidationResult(false, "Id vec postoji.");
                                    }
                                }
                                break;
                            case "TipId":
                                XmlSerializer desrTip = new XmlSerializer(typeof(List<Tip>));
                                StreamReader srTip = new StreamReader(MainWindow.instance.putanjaTipa);
                                List<Tip> listTipova = (List<Tip>)desrTip.Deserialize(srTip);
                                srTip.Close();
                                foreach (Tip t in listTipova)
                                {
                                    if (t.Id.ToString() == value.ToString())
                                    {
                                        return new ValidationResult(false, "Id vec postoji.");
                                    }
                                }
                                break;
                            case "DogadjajId":
                                XmlSerializer desrDogadjaj = new XmlSerializer(typeof(List<Dogadjaj>));
                                StreamReader srDogadjaj = new StreamReader(MainWindow.instance.putanjaDogadjaja);
                                List<Dogadjaj> listaDogadjaja = (List<Dogadjaj>)desrDogadjaj.Deserialize(srDogadjaj);
                                srDogadjaj.Close();
                                foreach (Dogadjaj d in listaDogadjaja)
                                {
                                    if (d.Id.ToString() == value.ToString())
                                    {
                                        return new ValidationResult(false, "Id vec postoji.");
                                    }
                                }
                                break;
                            default:
                                break;

                        }
                        
                    }
                    try
                    {
                        switch (Objekat)
                        {
                            case "Etiketa":
                                DodajEtiketu.instance.ValidateTrue(Id);
                                break;
                            case "Dogadjaj":
                                DodajDogadjaj.instance.ValidateTrue(Id);
                                break;
                            case "Tip":
                                DodajTip.instance.ValidateTrue(Id);
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                    }

                    return new ValidationResult(true, null);
                }
                try
                {
                    switch (Objekat)
                    {
                        case "Etiketa":
                            DodajEtiketu.instance.ValidateFalse(Id);
                            break;
                        case "Dogadjaj":
                            DodajDogadjaj.instance.ValidateFalse(Id);
                            break;
                        case "Tip":
                            DodajTip.instance.ValidateFalse(Id);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                }
                return new ValidationResult(false, "Mozete koristiti samo brojeve.");
            }
            catch
            {
                try { 
                switch (Objekat)
                {
                    case "Etiketa":
                        DodajEtiketu.instance.ValidateFalse(Id);
                        break;
                    case "Dogadjaj":
                        DodajDogadjaj.instance.ValidateFalse(Id);
                        break;
                    case "Tip":
                        DodajTip.instance.ValidateFalse(Id);
                        break;
                    default:
                        break;
                }
                } catch(Exception e)
                {
                }
                return new ValidationResult(false, "Nepoznata greska se pojavila");
            }
        }
    }

    public class DoubleValidationRule : ValidationRule
    {
        public string Id { get; set; } = "";
        public string Objekat { get; set; } = "";

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;
                double r;
                if (double.TryParse(s, out r) || s == string.Empty)
                {
                    switch (Objekat)
                    {
                        case "Etiketa":
                            DodajEtiketu.instance.ValidateTrue(Id);
                            break;
                        case "Dogadjaj":
                            DodajDogadjaj.instance.ValidateTrue(Id);
                            break;
                        case "Tip":
                            DodajTip.instance.ValidateTrue(Id);
                            break;
                        default:
                            break;
                    }
                    return new ValidationResult(true, null);
                }
                switch (Objekat)
                {
                    case "Etiketa":
                        DodajEtiketu.instance.ValidateFalse(Id);
                        break;
                    case "Dogadjaj":
                        DodajDogadjaj.instance.ValidateFalse(Id);
                        break;
                    case "Tip":
                        DodajTip.instance.ValidateFalse(Id);
                        break;
                    default:
                        break;
                }
                return new ValidationResult(false, "Molim vas unesite cenu.");
            }
            catch
            {
                switch (Objekat)
                {
                    case "Etiketa":
                        DodajEtiketu.instance.ValidateFalse(Id);
                        break;
                    case "Dogadjaj":
                        DodajDogadjaj.instance.ValidateFalse(Id);
                        break;
                    case "Tip":
                        DodajTip.instance.ValidateFalse(Id);
                        break;
                    default:
                        break;
                }
                return new ValidationResult(false, "Nepoznata greska se pojavila.");
            }



        }
    }

    
}
