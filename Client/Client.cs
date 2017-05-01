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
    class Format
    {
        public string Name { get; set; }
        public string Information { get; set; }
        public Format()
        {
            Name = string.Empty;
            Information = string.Empty;
        }
        public Format(string name, string information)
        {
            Name = name;
            Information = information;
        }
    }
    class Client
    {
     
        public string Serwer { get; private set; }
        public int Port { get; private set; }
        public TcpClient Tcp { get; private set; }

        public string Gate { get; private set; }
        public Client(string server, int port)
        {
            Serwer = server;
            Port = port;
            Gate = GetDefaultGateway().ToString();
        }
        public bool ConnectWithServer(string IP)
        {
            try
            {
                Tcp = new TcpClient(Serwer, Port);
                if (Tcp.Connected)
                {
                    SendCommunique(IP+"#"+Gate);
                    Console.WriteLine("Polaczono z serwerem");
                    return true;
                }
                else
                {
                    Console.WriteLine("Nie uzyskano polaczenia z serwerem");
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
            try
            {
                BinaryReader reader = new BinaryReader(Tcp.GetStream());
                string message = reader.ReadString();
                string data = Convert.ToString(message);
                return data;
            }
            catch
            {
                return("");
            }
        }
        public void SendCommunique(string communique)
        {
            try
            {
                BinaryWriter writer = new BinaryWriter(Tcp.GetStream());

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

        public static IPAddress GetDefaultGateway()
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up)
                .SelectMany(n => n.GetIPProperties()?.GatewayAddresses)
                .Select(g => g?.Address)
                .FirstOrDefault(a => a != null);
        }

    }
}
    
