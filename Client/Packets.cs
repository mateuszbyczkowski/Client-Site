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
        public bool permision { get; }
        public string CheckSum { get; set; }
        public string Identification { get; set; }

        public Packets(string line)
        {
            Line = line;

            permision = true ;
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

                for (int i=0; i<words.Count(); i++)
                {
                    if(words[i]=="id")
                    {
                        CheckSum=words[i + 1];
                    }
                    if(words[i]== "length:")
                    {
                        string value = words[i + 2];
                        Char delimiter = '.';
                        String[] substrings = new String[2];
                        substrings = value.Split(delimiter);
                        try
                        {
                            SourIP = substrings[0] + "." + substrings[1] + "." + substrings[2] + "." + substrings[3];
                        }
                        catch
                        {

                        }

                        value= words[i + 4];
                        substrings = null;
                        substrings = new String[2];
                        substrings = value.Split(delimiter);
                        try
                        {
                            DestIP = substrings[0] + "." + substrings[1] + "." + substrings[2] + "." + substrings[3];
                        }
                        catch
                        {

                        }
                    }
                }

                if (!SourMAC.Contains(':') || !DestMAC.Contains(':') || !SourIP.Contains('.') || !DestIP.Contains('.') || !CheckSum.Contains(',')||DestMAC=="ff:ff:ff:ff:ff:ff,")
                {
                    permision = false;
                }
                else
                {
                    Line = SourIP.Replace(",:", "") + '#' + DestIP.Replace(":", "") + '#' + SourMAC + '#' + DestMAC.Replace(",", "") + "#" + CheckSum;


                    display_information();
                }
            }
            catch
            {
                permision = false;
            }



        }

        public void display_information()
        {
            Console.WriteLine(Line);
            //Console.WriteLine( SourIP + " " + DestIP + " " + SourMAC + " " + DestMAC + "\n");
        }
    }
}
