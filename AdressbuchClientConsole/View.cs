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
            Console.WriteLine("9 - Programmende");
            Console.Write("Ihre Auswahl> ");
        }
        public void aktualisiereSicht(List<Person> _personen)
        {
            foreach (Person p in _personen)
            {
				Console.WriteLine("======================================");
				var properties = typeof(Person).GetProperties();
				foreach (var pair in properties)
				{
					Console.WriteLine(pair.Name + ": " + pair.GetValue(p));
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
