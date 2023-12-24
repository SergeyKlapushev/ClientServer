using ClientServer;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    internal class ProgramClient1
    {
        private static UdpClient _udpClient;
        private static Client lient;

        static void Main(string[] args)
        {
            client = new Client1("Sergey", 322, "127.0.0.1");
            StartClient(client.name, client.port, client.ip);
        }

        public static void StartClient(string From, int port, string ip)
        {
            _udpClient = new UdpClient(port);

            Task.Run(() => { ExpectMessage(); });

            while (true)
            {
                SendMessage(From, ip);
            }
        }
        public static void ExpectMessage()
        {
            byte[] buffer;
            IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);


            while (true)
            {
                buffer = _udpClient.Receive(ref remoteEndpoint);
                var messageText = Encoding.UTF8.GetString(buffer);
                Message? message = Message.DeserializeFromJson(messageText);
                if (message.Text == @"\Exit")
                {
                    CompletionWork();
                }
                message.Print();
            }

        }

        public static void SendMessage(string From, string ip)
        {
            string mess = Console.ReadLine();
            IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Parse(ip), 12345);
            Message message = new Message() { Text = mess, NicknameFrom = From, DateTime = DateTime.Now };
            string json = message.SerializeToJson();
            byte[] data = Encoding.UTF8.GetBytes(json);
            _udpClient.Send(data, data.Length, remoteEndpoint);
            Console.WriteLine("Сообщение отправлено серверу");
        }

        public static void CompletionWork()
        {
            Console.WriteLine("Завершение работы клиента");
            _udpClient.Close();
            Environment.Exit(0);
        }
    }
}