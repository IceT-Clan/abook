using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using __ClientSocket__;

namespace Adressbuch
{
    enum ServerCommand
    {
        NONE,
        FINDPERSONS,
        GETFILE,
        ADDPERSON,
        EDITPERSON,
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
				// Person loeschen
                case 4:
                    loeschePerson();
                    break;
				// Person bearbeiten
				case 5:
					bearbeitePerson();
					break;
                case 9:
                    break;

                default:
                    break;
            } // Ende switch

            return eingabe;
        }

        private void bearbeitePerson()
		{
            uint id = 0;

            // Suchbegriff abfragen
            Console.Write("ID> ");

            try
            {
                id = Convert.ToUInt32(Console.ReadLine());
				if (id == 0) return;
            }
            catch
            {
                return;
            }

            // Hier müsste eine Ausnahmebehandlung erfolgen
            // falls keine Verbindung möglich ist
            client = new ClientSocket(host, port);

            // Verbindung mit Server herstellen
            client.connect();

            // Kommando senden
            client.write((int)ServerCommand.EDITPERSON);

            // ID senden
            client.write(id.ToString() + "\n");

			// Person empfangen
			string pers = client.readLine();
			Console.WriteLine(pers);
			Person p = Person.FromString(pers);

			// Person bearbeiten
			p = view.bearbeitePerson(p);
			
			// Bearbeitete Person senden
			client.write(p.ToString() + "\n");
		}

        private void loeschePerson()
        {
            uint id = 0;

            // Suchbegriff abfragen
            Console.Write("ID> ");

            try
            {
                id = Convert.ToUInt32(Console.ReadLine());
				if (id == 0) return;
            }
            catch
            {
                return;
            }

            // Hier müsste eine Ausnahmebehandlung erfolgen
            // falls keine Verbindung möglich ist
            client = new ClientSocket(host, port);

            // Verbindung mit Server herstellen
            client.connect();

            // Kommando senden
            client.write((int)ServerCommand.DELETEPERSON);

            // ID senden
            client.write(id.ToString() + "\n");

            // Bestätigung empfangen
            bool ans = Convert.ToBoolean(client.readLine());

            if (ans)
            {
                Console.WriteLine("Person mit ID " + id.ToString() + " erfolgreich geloescht.");
            } else
            {
                Console.WriteLine("Keine Person mit dieser ID vorhanden.");
            }
            return;
        }

        private int menue()
        {
            int auswahl = 0;

            view.zeigeMenue();

            // Auswahl lesen
            do
            {
                try
                {
                    auswahl = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {

                }
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
            int anzahl = Convert.ToInt32(client.readLine());

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

			// Verbindung mit Server herstellen
			client.connect();

			// Kommando senden
			client.write((int)ServerCommand.GETFILE);

			// Groeße der Datei empfangen
			int size = client.read();

			Console.WriteLine("Dateigroesse: {0}", size);

			Console.Write("Speicherort> ");
			string filename = Console.ReadLine();

			if (size > 0)
			{
				try
				{
					byte[] file = new byte[size];
					client.read(file, size);
					File.WriteAllBytes(filename, file);
				}
				catch
				{
					Console.WriteLine("Adressbuchdatei konnte nicht empfangen werden.");
					return;
				}
			}

			Console.WriteLine("Adressbuchdatei erfolgreich gespeichert.\n");
			client.close();
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

            client.write(person + "\n");
            client.close();
        }

    }
}
