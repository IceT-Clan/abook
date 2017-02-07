using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Adressbuch
{
    class View
    {
        // Hierin wird alles notwenige für
        // das Anzeigen der Daten gekapselt
        // die View soll nach außen festgelegte
        // Methoden enthalten und austauschbar sein
        // z.B. durch ein Formular

        public void zeigeMenue()
        {
            // Ausgabe Menue
            Console.WriteLine("1 - Suche Personen");
            Console.WriteLine("2 - Hole Adressbuch");
            Console.WriteLine("3 - Neue Person hinzufuegen");
            Console.WriteLine("4 - Person loeschen");
            Console.WriteLine("5 - Person bearbeiten");
            Console.WriteLine("9 - Programmende");
            Console.Write("Ihre Auswahl> ");
        }

		public Person bearbeitePerson(Person p)
		{
            foreach (var pair in typeof(Person).GetProperties())
			{
				bool error = true;
				while (error)
				{
					error = true;
					if (pair.Name == "ID")
					{
						error = false;
						continue;
					}

					if (pair.Name == "Geburtstag")
					{
						DateTime g = Convert.ToDateTime(pair.GetValue(p));
						Console.Write(pair.Name + "[" + g.Day + "." + g.Month + "." + g.Year + "]" + ": ");
					}
					else
					{
						Console.Write(pair.Name + "[" + pair.GetValue(p) + "]" + ": ");
					}

					string input = Console.ReadLine();
					if (input != "")
					{
						if (pair.Name == "Geburtstag")
						{
							DateTime g = Convert.ToDateTime(input);
							pair.SetValue(p, g);
							error = false;
						}
						else if (pair.Name == "Festnetz" || pair.Name == "Mobiltelefon")
						{
							int val;
							if (int.TryParse(input, out val))
							{
								Console.WriteLine("1");
								pair.SetValue(p, input);
								error = false;
							}
							else
							{
								Console.WriteLine("2");
								error = true;
							}
						}
						else
						{
							pair.SetValue(p, input);
							error = false;
						}
					}
					else {
						error = false;
					}
				}
			}

			return p;
		}

        public void aktualisiereSicht(List<Person> _personen)
        {
            foreach (Person p in _personen)
            {
				Console.WriteLine("======================================");
				var properties = typeof(Person).GetProperties();
				foreach (var pair in properties)
				{
					if (pair.Name == "Geburtstag")
					{
						DateTime g = (DateTime)pair.GetValue(p);
						Console.WriteLine(pair.Name + ": " + g.ToString("dd.MM.yyyy"));
					}
					else
					{
						Console.WriteLine(pair.Name + ": " + pair.GetValue(p));
					}
				}
                Console.WriteLine("======================================");
                Console.WriteLine();
            }
        }

        public string fügeHinzuNeuePerson()
        {
            string person = "";

			var properties = typeof(Person).GetProperties();

            foreach (var pair in properties)
            {
				if (pair.Name != "ID") {
					Console.WriteLine("{0}: ", pair.Name);
					person += Console.ReadLine() + ";";
				}
            }

            return person;
        }
    }
}
