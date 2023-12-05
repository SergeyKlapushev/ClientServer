using ClientServer;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    internal class ProgramClient
    {
        static void Main(string[] args)
        {
             SentMessage(args[0], args[1]);
        }

        public static void SentMessage(string From, string ip)
        {
          
            UdpClient udpClient = new UdpClient();
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), 12345);
            
            while(true)
            {
                string messageText;
                do
                {
                    Console.Clear();
                    Console.WriteLine("Input your message:");
                    messageText = Console.ReadLine();
                }
                while (string.IsNullOrEmpty(messageText));

                Message message = new Message() { Text = messageText, NicknameFrom = From, NicknameTo = "Server", DateTime = DateTime.Now };
                string json = message.SerializeToJson();
                byte[] data = Encoding.UTF8.GetBytes(json);
                udpClient.Send(data, data.Length, iPEndPoint);

                byte[] ansver = udpClient.Receive(ref iPEndPoint);
                
                if (ansver == null)
                {
                    break;
                }
                var ansverText = Encoding.UTF8.GetString(ansver);
                message = Message.DeserializeFromJson(ansverText);
                message.Print();
                Console.ReadLine();
            }
        }
    }
}
