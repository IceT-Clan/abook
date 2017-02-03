using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using __ClientSocket__;

namespace Adressbuch
{
    enum ServerCommand
    {
        NONE,
        FINDPERSONS,
        GETALLPERSONS,
        ADDPERSON,
        DELETEPERSON
    }

    enum ClientInfo
    {
        NOMOREDATA,
        MOREDATA
    }

    class ControllerClient
    {
        private ClientSocket client;
        private View view;
        private string host;
        private int port;

        public ControllerClient(string _host, int _port)
        {
            host = _host;
            port = _port;
            // Zugriff auf die View
            view = new Adressbuch.View();
        }

        // Hiermit wird der Client gestartet
        public int start()
        {
            // Hier erfolgt die Interaktion mit dem Benutzer
            // Die Ausgaben können in einem View-Objekt erfolgen

            int eingabe = 0;

            // Menü ausgeben und Auswahl treffen
            eingabe = menue();

            switch (eingabe)
            {
                // Suche Personen
                case 1:
                    suchePersonen();
                    break;

                // Hole Adressbuch
                case 2:
                    holeAdressbuch();
                    break;
                // Person hinzufuegen
                case 3:
                    fügeHinzuNeuePerson();
                    break;
                case 9:
                    break;

                default:
                    break;
            } // Ende switch

            return eingabe;
        }

        private int menue()
        {
            int auswahl = 0;

            view.zeigeMenue();

            // Auswahl lesen
            do
            {
                auswahl = Convert.ToInt32(Console.ReadLine());
            } while (auswahl < 1 || auswahl > 9);

            Console.WriteLine();

            // auswahl zurück liefern
            return auswahl;
        }

        private void suchePersonen()
        {
            // Suchbegriff abfragen
            Console.Write("Suchbegriff> ");
            string suchbegriff = Console.ReadLine();

            // Hier müsste eine Ausnahmebehandlung erfolgen
            // falls keine Verbindung möglich ist
            client = new ClientSocket(host, port);

            // Verbindung mit Server herstellen
            client.connect();
            
            // Kommando senden
            client.write((int)ServerCommand.FINDPERSONS);

            // Suchstring senden
            client.write(suchbegriff + "\n");

            // Anzahl gefundener Personen lesen
            int anzahl = client.read();

            Console.WriteLine("Anzahl gefundener Personen: {0}", anzahl);

            if (anzahl > 0)
            {
                List<Person> ergebnis = new List<Person>();

                for (int i = 0; i < anzahl; i++)
                {
                    string person = client.readLine();

                    // Testausgabe
                    // Console.WriteLine(person);

                    // Person-Objekt aus empfangenem String
                    Person p = Person.FromString(person);

                    // Person-Objekt in die Liste für die Anzeige
                    ergebnis.Add(p);
                } // Ende for

                // Daten anzeigen
                view.aktualisiereSicht(ergebnis);

            } // End if

            client.close();


        }

        private void holeAdressbuch()
        {
            // Hier müsste eine Ausnahmebehandlung erfolgen
            // falls keine Verbindung möglich ist
            client = new ClientSocket(host, port);
            try
            {
                // Verbindung mit Server herstellen
                client.connect();

                // Kommando senden
                client.write((int)ServerCommand.GETALLPERSONS);

                // Anzahl gefundener Personen lesen
                int anzahl = client.read();

                Console.WriteLine("Anzahl gefundener Personen: {0}", anzahl);

                if (anzahl > 0)
                {
                    List<Person> ergebnis = new List<Person>();

                    for (int i = 0; i < anzahl; i++)
                    {
                        string person = client.readLine();

                        // Testausgabe
                        // Console.WriteLine(person);

                        // Person-Objekt aus empfangenem String
                        Person p = Person.FromString(person);

                        // Person-Objekt in die Liste für die Anzeige
                        ergebnis.Add(p);
                    } // Ende for

                    // Daten anzeigen
                    view.aktualisiereSicht(ergebnis);

                } // End if

                client.close();

            }
            catch (Exception)
            {
                throw;
            }
        }

		private void fügeHinzuNeuePerson()
        {
            // Hier müsste eine Ausnahmebehandlung erfolgen
            // falls keine Verbindung möglich ist
            client = new ClientSocket(host, port);
            // Verbindung mit Server herstellen
            client.connect();

            // Kommando senden
            client.write((int)ServerCommand.ADDPERSON);

            // Hole Kontaktdaten für neue Person
            string person = view.fügeHinzuNeuePerson();

            client.write(person);
            client.close();
        }

    }
}
