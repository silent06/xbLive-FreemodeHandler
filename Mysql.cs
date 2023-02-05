using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MySql.Data.MySqlClient;

namespace FreemodeHanlder
{
    class MySQL
    {
        public static MySqlConnection Setup()
        {
            return new MySqlConnection(String.Format("Server={0};Port=3306;Database={1};Uid={2};Password={3};", Global.host, Global.Database, Global.Username, Global.password));
        }

        public static bool Connect(MySqlConnection connection)
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception.Message);
                return false;
            }
        }

        public static void Disconnect(MySqlConnection connection)
        {
            try
            {
                connection.Close();
            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        public static bool ClientData(string id, ref ClientInfo data)
        {
            using (var db = Setup())
            {
                Connect(db);
                using (var command = db.CreateCommand())
                {
                    command.CommandText = string.Format("SELECT * FROM users WHERE id = @key");
                    command.Parameters.AddWithValue("@key", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            data.iID = reader.GetInt32("id");
                            data.iTimeEnd = reader.GetInt32("time_end");
                            data.Status = (ClientInfoStatus)reader.GetInt32("status");
                            Disconnect(db);
                            return true;
                        }
                    }
                }
                Disconnect(db);
            }
            return false;
        }

        public static void UpdateClientFreemode(string id, ref int seconds)
        {
            ClientInfo info = new ClientInfo();
            if (ClientData(id, ref info))
            {
                if (info.Status != ClientInfoStatus.Banned && info.Status != ClientInfoStatus.Disabled)
                {
                    using (var db = Setup())
                    {
                        Connect(db);

                        using (var command = db.CreateCommand())
                        {
                            command.CommandText = string.Format("UPDATE users SET time_end = @time_end, `status` = 0 WHERE `id` = @id");
                            command.Parameters.AddWithValue("@id", id);

                            if (info.iTimeEnd > (int)Utils.GetTimeStamp())/*Get Current Unix Time compare to store time in SQL*/
                            {
                                command.Parameters.AddWithValue("@time_end", info.iTimeEnd + seconds);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@time_end", (int)Utils.GetTimeStamp() + seconds);
                            }

                            command.ExecuteNonQuery();
                        }

                        Disconnect(db);
                    }
                }
            }
        }
    }
}
