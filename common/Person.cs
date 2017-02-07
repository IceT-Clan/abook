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
        public DateTime Geburtstag { get; set; }
        public string Plz { get; set; }
        public string Adresse { get; set; }
        public string Hausnummer { get; set; }
        public string Festnetz { get; set; }
        public string Mobiltelefon { get; set; }
        public uint ID { get; set; }

		public Person() {}
        public Person(string _v, string _n, DateTime _g, string _p, string _a, string _h, string _f, string _m, uint _id)
        {
            Vorname = _v;
            Name = _n;
            Geburtstag = _g;
            Plz = _p;
            Adresse = _a;
            Hausnummer = _h;
            Festnetz = _f;
            Mobiltelefon = _m;
            ID = _id;
        }

        public override string ToString()
        {
            string person = "";

            // Hier wird ein Person-Objekt in den String umgeformt
			foreach (var pair in typeof(Person).GetProperties())
			{
				if (pair.Name == "Geburtstag")
				{
					DateTime g = (DateTime)pair.GetValue(this);
					person += g.Day + "." + g.Month + ".";
					person += g.Year + ";";
				}
				else if (pair.Name != "ID")
				{
					person += pair.GetValue(this) + ";";
				}
				else
				{
					person += pair.GetValue(this);
				}
			}

            return person;
        }

        public static Person FromString(string _p)
        {
            char[] separator = { ';' };
            string[] daten = _p.Split(separator);
			// daten[0] : Vorname
			// daten[1] : Name
			// daten[2] : Geburtstag
			// daten[3] : PLZ
			// daten[4] : Adresse
			// daten[5] : Hausnummer
			// daten[6] : Festnetz
			// daten[7] : Mobiltelefon
			// daten[8] : ID

            // Geburtsdatum umformen, um ein DateTime-Objekt
            // zu erstellen
            char[] trenner = { '.', '/' };
            string[] geburtsdatum = daten[2].Split(trenner);

            int tag = Convert.ToInt32(geburtsdatum[0]);
            int monat = Convert.ToInt32(geburtsdatum[1]);
            int jahr = Convert.ToInt32(geburtsdatum[2]);

            DateTime datum = new DateTime(jahr, monat, tag);

            // Person-Objekt erstellen und der Liste hinzufügen
            Person p = new Person(daten[0], daten[1], datum, daten[3], daten[4], daten[5], daten[6],
								  daten[7], Convert.ToUInt32(daten[8]));
            return p;
        }
    }
}
