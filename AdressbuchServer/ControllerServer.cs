using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using __ServerSocket__;
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

    class ControllerServer
    {
        private Model model;
        private ServerSocket server;
		private string addrbook_file;

        public ControllerServer(int _port, string _addrbook_file)
        {
			addrbook_file = _addrbook_file;

            model = new Model(addrbook_file);

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
                    case ServerCommand.GETFILE:
                        holeAdressbuch(client);
                        break;
                    case ServerCommand.ADDPERSON:
                        fügeHinzuNeuePerson(client);
                        break;
					case ServerCommand.EDITPERSON:
						bearbeitePerson(client);
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
			Console.WriteLine("Suche nach: " + suchbegriff);

            // Speichere die Ergebnisse in einer Liste
            List<Person> ergebnis = model.suchePersonen(suchbegriff);

            // Sende Client die Anzahl der gefundenen Personen
            _c.write(ergebnis.Count + "\n");

            // Sende nun die Personendaten
            if (ergebnis.Count > 0)
            {
                string separator = ";";

                foreach (Person p in ergebnis)
                {
                    string data = p.ToString();

                    if (data.Contains('\n'))
                        throw new InvalidOperationException();
                    _c.write(data + "\n");
                }
            }
        }

		// Sende die Adressbuchdatei
        private void holeAdressbuch(ClientSocket _c)
        {
			System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
			byte[] fileData = File.ReadAllBytes(addrbook_file);

            // Sende Client die Große der Datei
            _c.write(fileData.Length);

			// Sende Datei
			_c.write(fileData);
        }

        private void bearbeitePerson(ClientSocket _c)
		{
			// ID empfangen
            uint id = Convert.ToUInt32(_c.readLine());

			// Person mit dieser ID senden
			string pers = model.suchePersonMitID(id).ToString() + "\n";
			_c.write(pers);

			// Bearbeitete Person empfangen
			string new_pers = _c.readLine();

			// Alte Person löschen
            if (!model.löschePerson(Convert.ToUInt32(id)))
			{
				return;
			}

			// Neue Person hinzufügen
			model.fügeHinzuNeuePerson(new_pers);

			return;
		}

        private void fügeHinzuNeuePerson(ClientSocket _c)
        {
            string person = _c.readLine();

			// Nächste freie ID hinzufügen
			person += holeFreieID().ToString();

            this.model.fügeHinzuNeuePerson(person);
        }

		private uint holeFreieID()
		{
			uint new_id = 0;

			for (uint i = 1; i <= model.personen.Count; i++)
			{
                if(model.personen.Find(pers => pers.ID == i) == null)
                {
                    new_id = i;
                    break;
                }
			}

			Console.WriteLine("Neue ID: " + new_id.ToString());

			return new_id;
		}

        private void loeschePerson(ClientSocket _c)
        {
            var id = _c.readLine();
            var res = model.löschePerson(Convert.ToUInt32(id));
            _c.write(res.ToString() + "\n");      
        }
    }
}
