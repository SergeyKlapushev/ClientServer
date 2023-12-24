using ClientServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Mailbox
    {
        string master = "Sergey";
        static UdpClient _udpClient = new UdpClient(321);
        static IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);
        string ipAddress = "127.0.0.1";
        static List<Message> unread = new List<Message>();
        static List<Message> readly = new List<Message>();
        public void ExpectMessage()
        {
            byte[] buffer;

            while (true)
            {
                buffer = _udpClient.Receive(ref remoteEndpoint);

                var messageText = Encoding.UTF8.GetString(buffer);
                Message? message = Message.DeserializeFromJson(messageText);

                unread.Add(message);

                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($"              Новое сообщение от {message.NicknameFrom}");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
        }


        public void InpitCommand(string command)
        {
            if (command.Equals(".surm"))
            {
                ShowUnreadMessages();
            }
        }

        public void ShowUnreadMessages()
        {
            foreach (var unreadMess in unread)
            {
                unreadMess.Print();
                readly.Add(unreadMess);
            }
            unread.Clear();
        }
        
    }
}
