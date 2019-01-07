using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using TaxiSluzba.Models;

namespace TaxiSluzba.App_Code
{
    public static class Global
    {
        private static Dictionary<string, Dispecer> dispeceri = new Dictionary<string, Dispecer>();
        private static Dictionary<string, Korisnik> korisnici = new Dictionary<string, Korisnik>();
        private static Dictionary<string, Vozac> vozaci = new Dictionary<string, Vozac>();
        private static Dictionary<string, Musterija> musterije = new Dictionary<string, Musterija>();
        private static Dictionary<string, Korisnik> blokirani = new Dictionary<string, Korisnik>();
        private static Dictionary<string, Voznja> voznje = new Dictionary<string, Voznja>();
        private static Dictionary<string, Automobil> automobili = new Dictionary<string, Automobil>();

        public static Dictionary<string, Dispecer> Dispeceri
        {
            get
            {
                return dispeceri;
            }

            set
            {
                dispeceri = value;
            }
        }

        public static Dictionary<string, Korisnik> Korisnici
        {
            get
            {
                return korisnici;
            }

            set
            {
                korisnici = value;
            }
        }

        public static Dictionary<string, Korisnik> Blokirani
        {
            get
            {
                return blokirani;
            }

            set
            {
                blokirani = value;
            }
        }

        public static Dictionary<string, Vozac> Vozaci
        {
            get
            {
                return vozaci;
            }

            set
            {
                vozaci = value;
            }
        }

        public static Dictionary<string, Musterija> Musterije
        {
            get
            {
                return musterije;
            }

            set
            {
                musterije = value;
            }
        }

        public static Dictionary<string, Voznja> Voznje
        {
            get
            {
                return voznje;
            }

            set
            {
                voznje = value;
            }
        }

        public static Dictionary<string, Automobil> Automobili
        {
            get
            {
                return automobili;
            }

            set
            {
                automobili = value;
            }
        }

        public static void DodajVozaca(Vozac vozac)
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Vozaci.txt");

            using (var sw = new StreamWriter(path, append:true))
            {
                var jsonObj = JsonConvert.SerializeObject(vozac, new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                }); ;
                sw.WriteLine(jsonObj);
            }

            //Vozaci.Add(vozac.KorisnickoIme, vozac);
        }

        public static void DodajMusteriju(Musterija musterija)
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Musterije.txt");

            using (var sw = new StreamWriter(path, append: true))
            {
                var jsonObj = JsonConvert.SerializeObject(musterija, new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                }); ;
                sw.WriteLine(jsonObj);
            }

            Musterije.Add(musterija.KorisnickoIme, musterija);
        }

        public static void DodajVoznju(Voznja voznja)
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Voznje.txt");

            using (var sw = new StreamWriter(path, append: true))
            {
                var jsonObj = JsonConvert.SerializeObject(voznja, new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                });
                sw.WriteLine(jsonObj);
            }

            Voznje.Add(voznja.VremePorudzbine.Ticks.ToString(), voznja);
        }

        public static void DodajAutomobil(Automobil automobil)
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Automobili.txt");

            using (var sw = new StreamWriter(path, append: true))
            {
                var jsonObj = JsonConvert.SerializeObject(automobil, new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                });
                sw.WriteLine(jsonObj);
            }

            Automobili.Add(automobil.TaxiOznaka, automobil);
        }

        public static void BlokirajKorisnika(Korisnik korisnik)
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Banovani.txt");

            using (var sw = new StreamWriter(path, append: true))
            {
                var jsonObj = JsonConvert.SerializeObject(korisnik, new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                });
                sw.WriteLine(jsonObj);
            }

            Blokirani.Add(korisnik.KorisnickoIme, Korisnici[korisnik.KorisnickoIme]);
        }

        public static void OdblokirajKorisnika(Korisnik korisnik)
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Banovani.txt");

            string line_to_delete = JsonConvert.SerializeObject(korisnik, new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            });

            if (File.Exists(path))
            {
                var txtLines = File.ReadAllLines(path).ToList();
                int index = txtLines.IndexOf(line_to_delete);
                txtLines.RemoveAt(index);
                File.WriteAllLines(path, txtLines);
            }

            Blokirani.Remove(korisnik.KorisnickoIme);
        }

        public static void IzmeniKorisnika(Korisnik k)
        {
            string line_to_delete = String.Empty;
            string line_to_insert = String.Empty;

            if (Dispeceri.Keys.Contains(k.KorisnickoIme))
            {
                k.Uloga = Uloga.DISPECER;
                line_to_delete = JsonConvert.SerializeObject(Dispeceri[k.KorisnickoIme]);
                Dispeceri[k.KorisnickoIme].Lozinka = k.Lozinka;
                Dispeceri[k.KorisnickoIme].Ime = k.Ime;
                Dispeceri[k.KorisnickoIme].Prezime = k.Prezime;
                Dispeceri[k.KorisnickoIme].Pol = k.Pol;
                Dispeceri[k.KorisnickoIme].Jmbg = k.Jmbg;
                Dispeceri[k.KorisnickoIme].KontaktTelefon = k.KontaktTelefon;
                Dispeceri[k.KorisnickoIme].Email = k.Email;
                line_to_insert = JsonConvert.SerializeObject(Dispeceri[k.KorisnickoIme], new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                });
            }
            else if (Vozaci.Keys.Contains(k.KorisnickoIme))
            {
                k.Uloga = Uloga.VOZAC;
                line_to_delete = JsonConvert.SerializeObject(Vozaci[k.KorisnickoIme]);
                Vozaci[k.KorisnickoIme].Lozinka = k.Lozinka;
                Vozaci[k.KorisnickoIme].Ime = k.Ime;
                Vozaci[k.KorisnickoIme].Prezime = k.Prezime;
                Vozaci[k.KorisnickoIme].Pol = k.Pol;
                Vozaci[k.KorisnickoIme].Jmbg = k.Jmbg;
                Vozaci[k.KorisnickoIme].KontaktTelefon = k.KontaktTelefon;
                Vozaci[k.KorisnickoIme].Email = k.Email;
                Korisnici[k.KorisnickoIme] = (Korisnik)Vozaci[k.KorisnickoIme];
                line_to_insert = JsonConvert.SerializeObject(Vozaci[k.KorisnickoIme], new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                });
            }
            else if (Musterije.Keys.Contains(k.KorisnickoIme))
            {
                k.Uloga = Uloga.MUSTERIJA;
                line_to_delete = JsonConvert.SerializeObject(Musterije[k.KorisnickoIme]);
                Musterije[k.KorisnickoIme].Lozinka = k.Lozinka;
                Musterije[k.KorisnickoIme].Ime = k.Ime;
                Musterije[k.KorisnickoIme].Prezime = k.Prezime;
                Musterije[k.KorisnickoIme].Pol = k.Pol;
                Musterije[k.KorisnickoIme].Jmbg = k.Jmbg;
                Musterije[k.KorisnickoIme].KontaktTelefon = k.KontaktTelefon;
                Musterije[k.KorisnickoIme].Email = k.Email;
                Korisnici[k.KorisnickoIme] = (Korisnik)Musterije[k.KorisnickoIme];
                line_to_insert = JsonConvert.SerializeObject(Musterije[k.KorisnickoIme], new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                });
            }

            string path = "noPath";
            if (k.Uloga == Uloga.DISPECER)
            {
                path = HttpContext.Current.Server.MapPath("~/App_Data/Dispeceri.txt");
            }
            else if (k.Uloga == Uloga.VOZAC)
            {
                path = HttpContext.Current.Server.MapPath("~/App_Data/Vozaci.txt");
            }
            else if (k.Uloga == Uloga.MUSTERIJA)
            {
                path = HttpContext.Current.Server.MapPath("~/App_Data/Musterije.txt");
            }

            if (File.Exists(path))
            {
                var txtLines = File.ReadAllLines(path).ToList();
                int index = txtLines.IndexOf(line_to_delete);
                txtLines.RemoveAt(index);
                txtLines.Insert(index, line_to_insert);
                File.WriteAllLines(path, txtLines);
            }

        }
    }
}