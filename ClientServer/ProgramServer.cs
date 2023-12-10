using ClientServer;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    internal class ProgramServer
    {
        static UdpClient udpClient;
        static CancellationTokenSource cts = new CancellationTokenSource();
        static CancellationToken token = cts.Token;

        static async Task Main(string[] args)
        {
            udpClient = new UdpClient(12345);

            Task serverStart = new Task(ExpectMessage, token);
            Task sendMessage = new Task(SendMessage, token);
            


            serverStart.Start();
            sendMessage.Start();
            try
            {
                Task.WaitAll(serverStart, sendMessage);
            }
            catch (AggregateException ex)
            {
                foreach (var innerException in ex.InnerExceptions)
                {
                    if (innerException is OperationCanceledException)
                    {
                        Console.WriteLine("Задача была отменена.");
                    }
                    else
                    {
                        Console.WriteLine("Произошло исключение: " + innerException.Message);
                    }
                }
            }


            cts.Cancel();

           

        }

        public static void ExpectMessage()
        {
            byte[] buffer;
            IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);
            

            while (!token.IsCancellationRequested)
            {
                buffer = udpClient.Receive(ref remoteEndpoint);
                var messageText = Encoding.UTF8.GetString(buffer);
                Message? message = Message.DeserializeFromJson(messageText);
                if (message.Text == @"\Exit")
                {
                    cts.Cancel();
                    token.ThrowIfCancellationRequested();
                }
                message.Print();
            }
        }

        public static void SendMessage()
        {
            while (!token.IsCancellationRequested)
            {
                token.ThrowIfCancellationRequested(); 
                string mess = Console.ReadLine();
                if (token.IsCancellationRequested == true)
                {
                    break;
                }
                IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 321);
                Message message = new Message() { Text = mess, NicknameFrom = "Server", DateTime = DateTime.Now };
                string json = message.SerializeToJson();
                byte[] data = Encoding.UTF8.GetBytes(json);
                udpClient.Send(data, data.Length, remoteEndpoint);
                Console.WriteLine("Сообщение отправлено клиенту");
            }
        }
    }
}