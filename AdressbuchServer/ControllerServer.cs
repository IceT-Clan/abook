using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using __ServerSocket__;
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

    class ControllerServer
    {
        private Model model;
        private ServerSocket server;

        public ControllerServer(int _port, string _addrbook_file)
        {
            model = new Model(_addrbook_file);

            // Hier sollte eine Ausnahmebehandlung stattfinden
            // für den Fall, dass der Port bereits anderweitig
            // vergeben ist
            server = new ServerSocket(_port);

            // Testzwecke
            // List<Person> liste = model.suchePersonen("6");
            // Console.WriteLine(liste.Count);
        }

        // Mit dieser Methode wird der Controller gestartet
        // und damit auch der Serverdienst
        public void start()
        {
            Console.WriteLine("Server gestartet!");

            // Server kann nicht gestoppt werden
            while (true)
            {
                // ServerSocket in listen-Modus
                ClientSocket client = new ClientSocket(server.accept());

                Console.WriteLine("Verbindung hergestellt!");

                // Der folgende Teil würde in einen separaten Thread ausgelagert,
                // um den Server wieder für neue Verbindungen zu öffnen
                // Dieser Thread würde den Client-Socket als Parameter
                // für die weitere Kommnikation erhalten

                // Client-Socket liest Kommando vom Client
                ServerCommand command = (ServerCommand)client.read();

                // Kommando wird ausgewertet
                switch (command)
                {
                    case ServerCommand.NONE:
                        break;

                    case ServerCommand.FINDPERSONS:
                        suchePersonen(client);
                        break;

                    case ServerCommand.GETALLPERSONS:
                        holeAllePersonen(client);
                        break;

                    case ServerCommand.ADDPERSON:
                        fügeHinzuNeuePerson(client);
                        break;

                    case ServerCommand.DELETEPERSON:
                        loeschePerson(client);
                        break;

                    default:
                        break;
                } // Ende switch

                client.close();
                client = null;
                Console.WriteLine("Verbindung geschlossen!");
                Console.WriteLine("=======================");

            } // Ende while

        }

        private void suchePersonen(ClientSocket _c)
        {
            // Lese Suchstring vom Client
            string suchbegriff = _c.readLine();

            // Speichere die Ergebnisse in einer Liste
            List<Person> ergebnis = model.suchePersonen(suchbegriff);

            // Sende Client die Anzahl der gefundenen Personen
            _c.write(ergebnis.Count);

            // Sende nun die Personendaten
            if (ergebnis.Count > 0)
            {
                string separator = ";";

                foreach (Person p in ergebnis)
                {
                    string data = p.Vorname + separator + p.Name + separator;
                    data += p.Plz + separator + p.Geburtstag.Date.ToShortDateString() + separator;
					data += p.ID;

                    // Testausgabe
                    Console.WriteLine(data);
                    if (data.Contains('\n'))
                        throw new InvalidOperationException();
                    _c.write(data + "\n");
                    Thread.Sleep(250);
                }
            }
        }

        private void holeAllePersonen(ClientSocket _c)
        {
            // Sende Client die Anzahl der gefundenen Personen
            _c.write(model.personen.Count);

            // Sende nun die Personendaten
            if (model.personen.Count > 0)
            {
                string separator = ";";

                foreach (Person p in model.personen)
                {
                    string data = p.Vorname + separator + p.Name + separator;
                    data += p.Plz + separator + p.Geburtstag.Date.ToShortDateString() + separator;
					data += p.ID;

                    // Testausgabe
                    Console.WriteLine(data);
                    if (data.Contains('\n'))
                        throw new InvalidOperationException();
                    _c.write(data + "\n");
                    Thread.Sleep(250);
                }
            }
        }

        private void fügeHinzuNeuePerson(ClientSocket _c)
        {
            string person = _c.readLine();

			// TODO ID-Berechnung
			int id = 0;
			person += id.ToString();

            this.model.fügeHinzuNeuePerson(person);
        }

        private void loeschePerson(ClientSocket _c)
        {

        }
    }
}
