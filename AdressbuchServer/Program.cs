using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adressbuch
{
    // Das ist der Server
    class Program
    {
        static void Main(string[] args)
        {
            // Standardport ist 55555
            int port = 55555;

            // Standarddatei
            string addrbook_file = @"adressbuch.csv";

            // Eventuelle Argumente durchlaufen
            if (args.Length > 1)
            {
                foreach (string arg in args)
                {
                    // Argument: /port:12345
                    char[] separator = { ':' };
                    string[] argument = arg.Split(separator);

                    switch (argument[0])
                    {
                        case "/port":
                            port = Convert.ToInt32(argument[1]);
                            break;
                        case "/file":
                            addrbook_file = Convert.ToString(argument[1]);
                            break;
                        default:
                            break;
                    }

                }
            }

            ControllerServer c = new ControllerServer(port, addrbook_file);
            c.start();

        }
    }
}
