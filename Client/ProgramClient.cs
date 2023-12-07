using ClientServer;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    internal class ProgramClient
    {
        private static UdpClient udpClient;

        static void Main(string[] args)
        {
            StartClient("Sergey", "127.0.0.1");
        }

        public static void StartClient(string From, string ip)
        {

            udpClient = new UdpClient(321);
            Task.Run(() => { ExpectMessage(); });
            while (true)
            {
                SendMessage();
            }
        }
        public static void ExpectMessage()
        {
            byte[] buffer;
            IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);
            

            while (true)
            {
                buffer = udpClient.Receive(ref remoteEndpoint);
                var messageText = Encoding.UTF8.GetString(buffer);
                Message? message = Message.DeserializeFromJson(messageText);
                if (message.Text == @"\Exit")
                {
                    CompletionWork();
                }
                message.Print();
            }

        }

        public static void SendMessage()
        {
            string mess = Console.ReadLine(); 
            IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
            Message message = new Message() { Text = mess, NicknameFrom = "Server", DateTime = DateTime.Now };
            string json = message.SerializeToJson();
            byte[] data = Encoding.UTF8.GetBytes(json);
            udpClient.Send(data, data.Length, remoteEndpoint);
            Console.WriteLine("Сообщение отправлено серверу");
        }

        public static void CompletionWork()
        {
            Console.WriteLine("Завершение работы клиента");
            udpClient.Close(); // Закрываем UDP сокет клиента
            Environment.Exit(0);
        }
    }
}
