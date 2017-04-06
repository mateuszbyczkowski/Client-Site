using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Client
    {
        public string Serwer { get; private set; }
        public int Port { get; private set; }
        public TcpClient Tcp { get; private set; }
        public Client(string server, int port)
        {
            Serwer = server;
            Port = port;
        }
        public bool ConnectWithServer(string IP)
        {
            try
            {
                Tcp = new TcpClient(Serwer, Port);
                if (Tcp.Connected)
                {
                    SendCommunique(IP);
                    Console.WriteLine("Polaczono z centrala");
                    return true;
                }
                else
                {
                    Console.WriteLine("Nie uzyskano polaczenia z centrala");
                    return false;
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }
            return false;
        }
        
        public string GetCommunique()
        {
            BinaryReader reader = new BinaryReader(Tcp.GetStream());
            byte message = reader.ReadByte();
            string data = Convert.ToString(message, 2);
            return data.PadLeft(8, '0');
        }
        public void SendCommunique(string communique)
        {
            try
            {
                BinaryWriter writer = new BinaryWriter(Tcp.GetStream());
                // Byte[] data = Encoding.ASCII.GetBytes(communique);
                //System.Console.WriteLine("SendCommunique " + communique);
                writer.Write(Convert.ToString(communique));
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }
        }

        public string GetIP()
        {
            string strHostName = "";
            strHostName = System.Net.Dns.GetHostName();

            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

            IPAddress[] addr = ipEntry.AddressList;

            return addr[addr.Length - 1].ToString();
        }
    }
}
    
