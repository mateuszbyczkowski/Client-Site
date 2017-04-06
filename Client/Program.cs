using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace Client
{
    class Program
    {
        public static string output;
        static private void tcpdump()
        {
            Process process = new Process();
            process.StartInfo.FileName = "C:\\users\\mateu\\downloads\\WinDump.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            // Synchronously read the standard output of the spawned process. 
            output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
        }

        private static Client client;
        private static string MyIP;

        static void Main(string[] args)
        {
            string ServerIP = "127.0.0.1";
            Console.WriteLine("Podaj IP urządzenia");
            string MyIP = Console.ReadLine();
            client = new Client(ServerIP, 1000);

            Thread thr = new Thread(tcpdump);
            thr.Start();

            if (client.ConnectWithServer(MyIP))
            {
                Console.WriteLine("Polaczono z serwerem");
                while (true)
                {
                    //Console.WriteLine("Komunikat do wyslania: docelowo TCPDump");
                    //string comumunique = Console.ReadLine();
                    //client.SendCommunique(comumunique);
                    client.SendCommunique(output);
                }
            }
            Console.WriteLine("Konczenie pracy");
            //Local IP
            //Console.WriteLine(client.GetIP().ToString());
            Console.ReadLine();
        }
    }
}
