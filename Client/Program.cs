using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        private static Client client;
        private static string MyIP;
        private static List<Users> user = new List<Users>();

        static void Main(string[] args)
        {
            Console.WriteLine("Podaj IP serwera");
            string ServerIP = Console.ReadLine();
            Console.WriteLine("Podaj IP urządzenia");
            string MyIP = Console.ReadLine();
            client = new Client(ServerIP, 1000);
            
           
         

            if (client.ConnectWithServer(MyIP))
            {
                Task t1 = new Task(new Action(SendCom));
                Task t2 = new Task(new Action(GetIP));
                Task t3 = new Task(new Action(TCPdump));

                t1.Start();
                t2.Start();
                t3.Start();
                Task.WaitAny(t1, t2,t3);
            }
            Console.WriteLine("Nie znaleziono serwera");
            Console.ReadLine();
        }

        private static void SendCom()
        {
            while (true)
            {
                Console.WriteLine("Podaj komunikat");
                string comumunique = Console.ReadLine();
                
                if(comumunique=="###") ///Wyświetlanie adresow IP użytkownikow
                {
                    foreach (Users us in user)
                    {
                        Console.WriteLine(us.IPaddr);
                    }
                }
                else
                {
                    client.SendCommunique(comumunique);
                }
            }
        }
      

        private static void GetIP()
        {
            while (true)
            {
                string addr = client.GetCommunique();
                bool temp=true;
                
                foreach (Users us in user)
                {
                    if (us.IPaddr == addr)
                    {
                        temp = false;
                        break;
                    }    
                }
                
                if(temp==true)
                {
                    user.Add(new Users(addr));
                }
            }
        }

        private static void TCPdump()
        {
            while (true)
            {
                //TUTAJ TRZECI WĄTEK Z TCPDUMPEM

            }
         }


        
    }
}
