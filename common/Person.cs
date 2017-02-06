﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adressbuch
{
    class Person
    {
        public string Vorname { get; set; }
        public string Name { get; set; }
        public string Plz { get; set; }
        public DateTime Geburtstag { get; set; }

        public uint ID { get; set; }

        public Person(string _v, string _n, string _p, DateTime _g, uint _id)
        {
            Vorname = _v;
            Name = _n;
            Plz = _p;
            Geburtstag = _g;
            this.ID = _id;
        }

        public override string ToString()
        {
            string person = "";

            // Hier wird ein Person-Objekt in den String umgeformt
            var g = this.Geburtstag;
            person = this.Vorname + ";" + this.Name + ";" + this.Plz + ";" + g.Day + "." + g.Month + "." + g.Year;
			person += ";" + this.ID;

            return person;
        }

        public static Person FromString(string _p)
        {
            char[] separator = { ';' };
            string[] daten = _p.Split(separator);

            // Geburtsdatum umformen, um ein DateTime-Objekt
            // zu erstellen
            Console.WriteLine(_p);
            char[] trenner = { '.', '/' };
            string[] geburtsdatum = daten[3].Split(trenner);

            int tag = Convert.ToInt32(geburtsdatum[0]);
            int monat = Convert.ToInt32(geburtsdatum[1]);
            int jahr = Convert.ToInt32(geburtsdatum[2]);

            DateTime datum = new DateTime(jahr, monat, tag);

            // Person-Objekt erstellen und der Liste hinzufügen
            Person p = new Person(daten[0], daten[1], daten[2], datum, Convert.ToUInt32(daten[4]));

            return p;
        }

    }
}
