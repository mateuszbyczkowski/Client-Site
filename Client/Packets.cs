using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Packets
    {
        public string Line { get; set; }
        public int Nr { get; }
        public string Time { get; }
        public string SourIP { get;  }
        public string DestIP { get;  }
        public string SourMAC { get; }
        public string DestMAC { get; }
        public string Protocol { get; set; }
        public string CheckSum { get; set; }
        public string Identification { get; set; }

        public Packets(string line)
        {
            Line = line;

            string phrase = line;
            string[] words;

            words = phrase.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
            
            try
            {
                Time = words[0];
                SourMAC = words[1];
                DestMAC = words[3];
                if (words[9] == "arp")
                {
                    SourIP = words[11];
                    DestIP = words[13];
                }
                else
                {
                    SourIP = words[9];
                    DestIP = words[11];
                }


                Line = SourIP + '#' + DestIP + '#' + SourMAC + '#' + DestMAC;


                display_information();
            }
            catch
            {

            }



        }

        public void display_information()
        {
            Console.WriteLine(Line);
            //Console.WriteLine( SourIP + " " + DestIP + " " + SourMAC + " " + DestMAC + "\n");
        }
    }
}
