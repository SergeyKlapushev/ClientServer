using ClientServer;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    internal class ProgramServer
    {
        static void Main(string[] args)
        {
            server();
        }

        public static void server()
        {
            byte[] buffer;
            
            UdpClient udpClient = new UdpClient(12345);
            IPEndPoint ipePOINT = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("Server wait a message from client");
            
            while (true)
            {
                buffer = udpClient.Receive(ref ipePOINT);

                if (buffer == null)
                {
                    ErrorNotification(udpClient, ipePOINT);
                    break;
                }
                Receiptnotification(udpClient, ipePOINT);

                var messageText = Encoding.UTF8.GetString(buffer);
                Message message = Message.DeserializeFromJson(messageText);
                message.Print();

            }
        }

        public static void Receiptnotification(UdpClient udpClient, IPEndPoint ipePOINT)
        {
            Message message = new Message() { Text = "Сообщение получено", NicknameFrom = "Server", DateTime = DateTime.Now };
            string json = message.SerializeToJson();
            byte[] data = Encoding.UTF8.GetBytes(json);
            udpClient.Send(data, data.Length, ipePOINT);
        }

        public static void ErrorNotification(UdpClient udpClient, IPEndPoint ipePOINT)
        {
            Message message = new Message() { Text = "Ошибка отправки", NicknameFrom = "Server", DateTime = DateTime.Now };
            string json = message.SerializeToJson();
            byte[] data = Encoding.UTF8.GetBytes(json);
            udpClient.Send(data, data.Length, ipePOINT);

        }

    }
}

