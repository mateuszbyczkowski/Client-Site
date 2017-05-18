using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private static List<Users> user = new List<Users>();
        private static List<Packets> packets = new List<Packets>();
        private static string MyIP = "";
        private static List<string> UniqueIP = new List<string>();

        static void Main(string[] args)
        {
            //Podanie adresu serwera i klienta
            //Console.WriteLine("Podaj IP serwera");
            //string ServerIP = Console.ReadLine();
            //Console.WriteLine("Podaj IP urządzenia");
            string ServerIP = "192.168.43.201";
            Console.WriteLine("Adres serwera: " + ServerIP);

            IPAddress[] ipv4Addresses = Array.FindAll(
            Dns.GetHostEntry(string.Empty).AddressList,
            a => a.AddressFamily == AddressFamily.InterNetwork);
            

            try
            {
                Console.WriteLine("Adres urządzenia: " + ipv4Addresses[0].ToString());
                 MyIP = ipv4Addresses[0].ToString();
                //Console.WriteLine(MyIP);
            }
            catch
            {
                Console.WriteLine("Nie znaleziono adresu IP");
                Console.ReadLine();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            client = new Client(ServerIP, 1000);
            try
            {
               Process.Start("MJSniff.exe");
            }
            catch
            {
                Console.WriteLine("Nie znaleziono programu MJSniff");
            }

            if (client.ConnectWithServer(MyIP))
            {
                Task t1 = new Task(new Action(SendCom)); //wysyłanie komunikatów (chat)
                Task t2 = new Task(new Action(GetIP)); //odbieranie adresów IP użytkowników połączonych do serwera
               // Task t3 = new Task(new Action(TCPdump)); //nieużywane, odniesienie trzeci watek z dumpem (task)

                //t1.Start();
                t2.Start();
               // t3.Start();
                Task.WaitAny( t2);
            }
            else
            {
                Console.WriteLine("Nie znaleziono serwera");
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

                    if (communique == "###") //Wyświetlanie adresow IP użytkownikow
                    {
                        foreach (Users us in user)
                        {
                            Console.WriteLine(us.IPaddr);
                        }

                    }
                    else //Wysyłanie komunikatu podanego przez użytkownika pozniej dump
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
                        string comm = us.IPaddr + "#" + pinging.ToString();
                        client.SendCommunique(comm);    //Odesłanie do serwera informacji czy udało się pingować
                    }
                }
                else if(addr == "DUMP")
                {
                    packets.Clear();
                    string path = "date.txt";

                    string[] readText = File.ReadAllLines(path);
                    System.IO.File.WriteAllText("date.txt", string.Empty);
                    foreach (string s in readText)
                    {
                        packets.Add(new Packets(s));
                        //client.SendCommunique("*"+s);
                    }
                    foreach (Packets p in packets)
                    {
                        
                        client.SendCommunique("*"+p.Line);
                    }

                    TCPdump();
                }
                else if (addr == "EXIT")
                {

                    //Process.Stop("MJSniff.exe");
                    Process[] processes = Process.GetProcessesByName("MJSniff");
                    foreach (var process in processes)
                    {
                        process.Kill();
                    }
                    System.Diagnostics.Process.GetCurrentProcess().Kill();

                }
                else if (addr == "")
                {
                }
                else
                {
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

        private static void TCPdump()                           //wysyła do serwera unikalne adresy z którymi się komunikował 
        {

            Thread.Sleep(100);
            
            
             foreach (Packets pack in packets)
             {
                 if ((UniqueIP.Contains(pack.DestIP) == false) && (pack.DestIP!="0.0.0.0") && (pack.DestIP != "255.255.255.255") && (pack.DestIP!=MyIP) && (pack.DestIP != client.Gate))                                    
                 {
                          UniqueIP.Add(pack.DestIP);
                 }
             }
            foreach (Packets pack in packets)
            {
                if ((UniqueIP.Contains(pack.SourIP) == false) && (pack.SourIP != "0.0.0.0") && (pack.SourIP != "255.255.255.255") && (pack.SourIP != MyIP) && (pack.SourIP != client.Gate))
                {
                    UniqueIP.Add(pack.DestIP);
                }
            }
            //TUTAJ TRZECI WĄTEK Z DUMPEM

            foreach (string ip in UniqueIP)
            {
                client.SendCommunique("^"+ip);
            }

            client.SendCommunique("@@@@@");
        }



        
    }
}
