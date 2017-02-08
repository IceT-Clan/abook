using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adressbuch
{
    // Das ist der Client
    class Program
    {
        static void Main(string[] args)
        {
            // Standardhost ist localhost
            string host = "127.0.0.1";

            // Standardport ist 55555
            int port = 55555;

            // Eventuelle Argumente durchlaufen
            if (args.Length > 0)
            {
                foreach (string arg in args)
                {
                    // Argumente: /host:10.2.210.21 /port:12345
                    char[] separator = { ':' };
                    string[] argument = arg.Split(separator);

                    switch (argument[0])
                    {
                        case "/port":
                            port = Convert.ToInt32(argument[1]);
                            break;
                        case "/host":
                            host = Convert.ToString(argument[1]);
                            break;
                        case "/help":
                            Console.WriteLine("Kommando" + "\t" + "Beschreibung          " + "\t" + "Standardwert");
                            Console.WriteLine("--------" + "\t" + "----------------------" + "\t" + "------------");
                            Console.WriteLine("/port   " + "\t" + "Angabe des Serverports" + "\t" + "5555        ");
                            Console.WriteLine("/host   " + "\t" + "IP des Servers        " + "\t" + "127.0.0.1");
                            Console.WriteLine("");
                            Console.WriteLine("----------------------------------------------------");
                            //Console.WriteLine("");
                            Console.WriteLine("Tipp: Eine leere Suche gibt alle Personen im Adressbuch zurück");
                            return;
                        default:
                            break;
                    } // Ende switch
                } // Ende foreach
            } // Ende if


            int eingabe = 0;

            do
            {
                // Controller-Objekt erstellen
                ControllerClient controller = new ControllerClient(host, port);
                eingabe = controller.start();
            } while (eingabe != 9);

        }
    }
}
