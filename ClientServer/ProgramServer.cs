using ClientServer;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Server
{
    internal class ProgramServer
    {
        static UdpClient udpClient;
        static void Main(string[] args)
        {
            ServerStart();
        }

        public static void ServerStart()
        {
            byte[] buffer;

            udpClient = new UdpClient(12345);
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
                if(message.Text == @"\Exit")
                {
                    CompletionWork();
                }
                message.Print();
            }
            
        }


        public static void SendMessage()
        {
            string mess = Console.ReadLine();
            IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 321);
            Message message = new Message() { Text = mess, NicknameFrom = "Server", DateTime = DateTime.Now };
            string json = message.SerializeToJson();
            byte[] data = Encoding.UTF8.GetBytes(json);
            udpClient.Send(data, data.Length, remoteEndpoint);
            Console.WriteLine("Сообщение отправлено клиенту");
        }

        public static void CompletionWork()
        {
            Console.WriteLine("Завершение работы сервера");
            udpClient.Close();
            Environment.Exit(0);
        }

    }
}
