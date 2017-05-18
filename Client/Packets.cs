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
        public int Nr { get; set; }
        public string SourIP { get; set; }
        public string DestIP { get; set; }
        public string Protocol { get; set; }
        public string CheckSum { get; set; }
        public string Identification { get; set; }

        public Packets(string line)
        {
            Line = line;

            string temp = "";
            int fieldnumber = 1;
            for(int i=0;i<line.Length;i++)
            {
               if(line[i]=='#')
                {
                    if(fieldnumber==1)
                    {
                        Nr = Convert.ToInt32(temp);
                    }
                    else if(fieldnumber == 2)
                    {
                        Protocol = temp;
                    }
                    else if(fieldnumber==3)
                    {
                        SourIP = temp;
                    }
                    else if (fieldnumber == 4)
                    {
                        CheckSum = temp;
                    }
                    else if (fieldnumber == 5)
                    {
                        Identification = temp;
                    }


                    //ostatnie pole jest zakończone dolarem i jest w ifie poniżej
                    else
                    { }
                    fieldnumber++;
                    temp = "";
                }
                else if(line[i] == '$')
                {
                    DestIP = temp;
                    break;
                }
               else
                {
                    temp = temp + line[i];
                }


                //TODO Parsowanie linii

            }
            display_information();
        }

        public void display_information()
        {
            Console.WriteLine(Nr + " " + Protocol + "  " + SourIP + " " + DestIP + " " + CheckSum + " " + Identification + "\n");
        }
    }
}
