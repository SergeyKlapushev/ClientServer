using ClientServer;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    internal class ProgramClient
    {
        private static UdpClient _udpClient;

        static void Main(string[] args)
        {
            Mailbox mailbox = new Mailbox();
            Task.Run(() => { mailbox.ExpectMessage(); }) ;

            while (true)
            {
                mailbox.InpitCommand(Console.ReadLine());
            }
        }

        public static void SendMessage()
        {
            string mess = Console.ReadLine();
            IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
            Message message = new Message() { Text = mess, NicknameFrom = "Sergey", DateTime = DateTime.Now };
            string json = message.SerializeToJson();
            byte[] data = Encoding.UTF8.GetBytes(json);
            _udpClient.Send(data, data.Length, remoteEndpoint);
            Console.WriteLine("Сообщение отправлено серверу");
        }

        public static void CompletionWork()
        {
            Console.WriteLine("Завершение работы клиента");
            _udpClient.Close(); // Закрываем UDP сокет клиента
            Environment.Exit(0);
        }
    }
}