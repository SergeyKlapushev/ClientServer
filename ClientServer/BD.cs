using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class BD
    {
        public void CreadBD() 
        { 
            string connectionString = "Host=DESKTOP-PAU4HIB; Username=postgres; Password=example; Database=Test";
            Console.WriteLine(System.Net.Dns.GetHostName());
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Users.id, User.name, Message.message " + "FROM Users " + "JOIN Messages ON Users.id = Messages.user_id";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            int userId = reader.GetInt32(0);
                            string userName = reader.GetString(1);
                            string message = reader.GetString(2);

                            Console.WriteLine($"User ID: {userId}, User Name: {userName}, Message: {message}");
                        }
                    }
                }
            }
        }
        
 


    }
}
