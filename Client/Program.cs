using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Client
{
    class Program
    {
        private static Client client;
        private static string MyIP;
        private static List<Users> user = new List<Users>();

        static void Main(string[] args)
        {
            //Podanie adresu serwera i klienta
            Console.WriteLine("Podaj IP serwera");
            string ServerIP = Console.ReadLine();
            Console.WriteLine("Podaj IP urządzenia");
            string MyIP = Console.ReadLine();

            client = new Client(ServerIP, 1000);
            



            if (client.ConnectWithServer(MyIP))
            {
                //Task t1 = new Task(new Action(SendCom)); //wysyłanie komunikatów (chat)
                Task t2 = new Task(new Action(GetIP)); //odbieranie adresów IP użytkowników połączonych do serwera
                Task t3 = new Task(new Action(TCPdump)); //nieużywane

                //t1.Start();
                t2.Start();
                t3.Start();
                Task.WaitAny( t2,t3);
            }
            Console.WriteLine("Nie znaleziono serwera");
            //Console.ReadLine();
        }

        private static void SendCom()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("Podaj komunikat");
                    string communique = Console.ReadLine();

                    if (communique == "###") ///Wyświetlanie adresow IP użytkownikow
                    {
                        foreach (Users us in user)
                        {
                            Console.WriteLine(us.IPaddr);
                        }



                    }
                    else //Wysyłanie komunikatu podanego przez użytkownika
                    {
                        client.SendCommunique(communique);
                    }
                }
            }
            catch
            {
                Console.WriteLine("Rozłączono");
            }
        }
      

        private static void GetIP() //Odbieranie komunikatów od serwera
        {
            while (true)
            {
                string addr = client.GetCommunique();
                if (addr == "#####")                    //Jeśli komunikat to ##### to pinguj do użytkowników o zapisanych adresach
                {

                    bool pinging = false;
                    Ping isPing = new Ping();
                    foreach (Users us in user)
                    {
                        try
                        {
                            PingReply reply = isPing.Send(us.IPaddr);
                            pinging = reply.Status == IPStatus.Success;
                        }
                        catch (PingException)
                        {

                        }
                        string comm= us.IPaddr + "#" + pinging.ToString();
                        client.SendCommunique(comm);    //Odesłanie do serwera informacji czy udało się pingować
                    }
                }

                else if(addr=="EXIT")
                {
                  
                        Console.WriteLine("Serwer został wyłączony.");
                        Console.WriteLine("Nastąpi zamknięcie aplikacji klienciej");
                        Console.ReadLine();
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                  
                }
                else if (addr == "")
                {
                }
                else                                //Jeśli komunikat różny od ##### to odebrano adres IP
                {                                   //Jeśli go jeszcze nie ma to zapisuje do listy user
                    bool temp = true;
                    foreach (Users us in user)
                    {
                        if (us.IPaddr == addr)
                        {
                            temp = false;
                            break;
                        }
                    }

                    if (temp == true)
                    {
                        user.Add(new Users(addr));
                    }
                }
            }
        }

        private static void TCPdump()
        {
            while (true)
            {
                //TUTAJ TRZECI WĄTEK Z DUMPEM

            }
         }


        
    }
}
