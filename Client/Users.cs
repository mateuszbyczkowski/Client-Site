using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Users
    {
        public string IPaddr { get;  set; }
        public List<string> IPlist = new List<string>();

        public Users()
        {

        }

        public Users(string addr)
        {
            Char delimiter = '|';
            String[] substrings = new String[2];
            substrings = addr.Split(delimiter);
            foreach(String s in substrings)
            {
                if(s!=null&s!="")
                IPlist.Add(s);
            }

            IPaddr = addr;
        }
        
    }
}
