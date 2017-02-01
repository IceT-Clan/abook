using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Console.WriteLine("9 - Programmende");
            Console.Write("Ihre Auswahl> ");
        }
        public void aktualisiereSicht(List<Person> _personen)
        {
            foreach (Person p in _personen)
            {
                Console.WriteLine("======================================");
                Console.WriteLine(p.Vorname + " " + p.Name);
                Console.WriteLine(p.Plz);
                Console.WriteLine(p.Geburtstag.Date.ToShortDateString());
                Console.WriteLine("======================================");
                Console.WriteLine();
            }
        }

        public string fügeHinzuNeuePerson()
        {
            string person = "";

            // Erstelle Personenobjekt mit Standartwerten
            Person p = new Person("v", "n", "p", DateTime.Today);

            var stringPropertyNamesAndValues = p.GetType()
            .GetProperties()
            .Where(pi => pi.GetGetMethod() != null)
            .Select(pi => new
            {
                Name = pi.Name,
                Value = pi.GetGetMethod().Invoke(p, null)
            });

            foreach (var pair in stringPropertyNamesAndValues)
            {
                Console.WriteLine("{0}: ", pair.Name);
                person += Console.ReadLine() + ";";
            }

            return person;
        }
    }
}
