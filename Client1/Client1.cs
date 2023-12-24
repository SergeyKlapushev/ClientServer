using ClientServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Client1
{
    internal class Client1
    {
        public string name { get; set; }
        public int port { get; set; }
        public string ip { get; set; }
        private static UdpClient _udpClient { get; set; }

        public Client1(string name, int port, string ip)
        {
            this.name = name;
            this.port = port;
            this.ip = ip;
        }

        public void ExpectMessage()
        {

        }

        public void SendMessage()
        {
            string mess = Console.ReadLine();
            IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Parse(ip), 12345);
            Message message = new Message() { Text = mess, NicknameFrom = this.name, DateTime = DateTime.Now };
            string json = message.SerializeToJson();
            byte[] data = Encoding.UTF8.GetBytes(json);
            _udpClient.Send(data, data.Length, remoteEndpoint);
        }
    }
}
