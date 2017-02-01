using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adressbuch
{
    // Diese Klasse stellt das Datenmodell für das Adressbuch
    // und notwendige Methoden für die Datenverarbeitung
    // zur Verfügung
    class Model
    {
        // Objektvariable für Zugriff auf Liste
        public List<Person> personen { get; private set; }


        public Model(string _addrbook_file)
        {
            // Leere Liste erstellen
            personen = new List<Person>();

            // Datensätze aus adressbuch.csv lesen,
            // Person-Objekte erstellen und
            // der Liste hinzufügen

            leseAdressbuchDatei(_addrbook_file);
        }

        public List<Person> suchePersonen(string wert)
        {
            // leere Ergebnisliste erstellen
            List<Person> ergebnis = new List<Person>();

            foreach (Person p in personen)
            {
                if (p.Vorname.Contains(wert) ||
                    p.Name.Contains(wert) ||
                    p.Plz.Contains(wert)
                   )
                {
                    Person newPerson = new Person(p.Vorname,
                                                  p.Name,
                                                  p.Plz,
                                                  p.Geburtstag,
                                                  p.ID
                                                 );
                    ergebnis.Add(newPerson);
                }
            }

            return ergebnis;

        }

        // Liest die Datei adressbuch.txt und erstellt Person-Objekte
        private bool leseAdressbuchDatei(string _addrbook_file)
        {
            // Hiermit könnte Erfolg oder Misserfolg der
            // Methode zurückgemeldet werden
            // Besser wäre, bei Misserfolg eine Ausnahme zu werfen
            bool rc = true;

            // automatische Freigabe der Ressource mittels using
            StreamReader sr = new StreamReader(_addrbook_file);
            string zeile;
            // Lesen bis Dateiende, Zeile für Zeile
            while ( ( zeile = sr.ReadLine() ) != null )
            {
                // Person-Objekt erstellen anhand gelesener Zeile
                Person p = convertString2Person(zeile);

                // Person-Objekt in die Liste einfügen
                personen.Add(p);
            }
            sr.Close();

            return rc;
        }

        private Person convertString2Person(string _p)
        {
            char[] separator = { ';' };
            string[] daten = _p.Split(separator);

            // Geburtsdatum umformen, um ein DateTime-Objekt
            // zu erstellen
            char[] trenner = { '.' };
            string[] geburtsdatum = daten[3].Split(trenner);

            int tag = Convert.ToInt32(geburtsdatum[0]);
            int monat = Convert.ToInt32(geburtsdatum[1]);
            int jahr = Convert.ToInt32(geburtsdatum[2]);

            DateTime datum = new DateTime(jahr, monat, tag);

            // Person-Objekt erstellen und der Liste hinzufügen
            Person p = new Person(daten[0], daten[1], daten[2], datum, Convert.ToUInt32(daten[3]));

            return p;
        }

        private string convertPerson2String(Person _p)
        {
            string person = "";

            // Hier wird ein Person-Objekt in den String umgeformt
            var g = _p.Geburtstag;
            person = _p.Vorname + ";" + _p.Name + ";" + _p.Plz + ";" + g.Day + "." + g.Month + "." + g.Year;

            return person;
        }


        // Schreibt die Person-Objekte in die Datei adressbuch.csv
        private bool schreibeAdressbuchDatei()
        {

            try
            {
                using (var sw = new StreamWriter(@"adressbuch.csv"))
                {
                    foreach (var p in personen)
                    {
                        sw.Write(convertPerson2String(p));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler beim Schreiben der Datei: ");
                Console.WriteLine(e.Message);
            }

            return true;
        }

        // Dient nur zu Testzwecken
        // Zeigt das Adressbuch auf der Server-Konsole
        private void zeigeAdressbuch()
        {
            foreach (Person p in personen)
            {
                Console.WriteLine(p.Vorname + " : " + p.Name + " : " + p.Plz + " : " + p.Geburtstag.ToShortDateString());
            }
        }

        public void fügeHinzuNeuePerson(string person)
        {
            this.personen.Add(convertString2Person(person));
            this.schreibeAdressbuchDatei();
        }

        private uint findeFreieId()
        {
            //check first field
            if(this.personen.ElementAt(0).ID > 0)
            {
                return 0;
            }


            int offset = 1;
            int c = this.personen.Count;
            while (true)
            {
                if(c-offset <= 10)
                {
                    uint oldId = this.personen.ElementAt(offset-1).ID;
                    for(int i = offset; i < c; i++)
                    {
                        var id = this.personen.ElementAt(i).ID;
                        if (id > oldId+1)
                        {
                            return oldId + 1;
                        }
                    }
                }
                int idplace = ((c-offset) / 2) + offset;
                var pers = this.personen.ElementAt(idplace);
                if (pers.ID > idplace)
                {
                    c = idplace;
                }
                else
                {
                    offset = idplace;
                }
            }
        }
    }

}
