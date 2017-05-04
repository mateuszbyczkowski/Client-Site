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

        public Users()
        {

        }

        public Users(string addr)
        {
            IPaddr = addr;
        }
        
    }
}
