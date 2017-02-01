using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adressbuch
{
    class Person
    {
        private string vorname;
        private string name;
        private string plz;
        private DateTime geburtstag;

        public string Vorname
        {
            get { return vorname; }
            set { vorname = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Plz
        {
            get { return plz; }
            set { plz = value; }
        }

        public DateTime Geburtstag
        {
            get { return geburtstag; }
            set { geburtstag = value; }
        }

        public Person(string _v, string _n, string _p, DateTime _g)
        {
            vorname = _v;
            name = _n;
            plz = _p;
            geburtstag = _g;
        }

    }
}
