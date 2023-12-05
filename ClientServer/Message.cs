using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientServer
{
    public class Message
    {
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public string NicknameFrom {get; set; }
        public string NicknameTo { get; set; }
        
        public string SerializeToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public static Message? DeserializeFromJson(string message)
        {
            return JsonSerializer.Deserialize<Message>(message);
        }
        public void Print()
        {
            Console.WriteLine($"Cообщение от: {this.NicknameFrom}\n {this.Text}");
        }
    }
}
