using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;

namespace BD_Project
{
    class BD
    {
        private static SQLiteConnection connection;
        const string databaseName = @"ProjectXDatebase.db";

        static BD() {
            connection = new SQLiteConnection(string.Format("Data Source={0};", databaseName));

        }
        private BD() { }

        public static void BDOpen()
        {
            connection.Open();
        }

        public static void BDClose()
        {
            connection.Close();
        }

        
        public static bool Authorization(string @name,string @password)
        {
            string hash;
            hash = GetMd5Hash(password);

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT count(Name) FROM 'Users' WHERE Name =@name and Password =@password";
            command.Parameters.Add(new SQLiteParameter("@name", @name));
            command.Parameters.Add(new SQLiteParameter("@password", @hash));
            int countRows = Convert.ToInt32(command.ExecuteScalar());

            if (countRows != 0)
                return true;//Ok
            else
                return false;//wrong login or password

        }

        public static bool AddPerson(string @name,string @password)
        {

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT count(Name) FROM 'Users' WHERE Name =@name";
            command.Parameters.Add(new SQLiteParameter("@name", @name));
            int countRows = Convert.ToInt32(command.ExecuteScalar());

            if (countRows != 0)
                return false;//name is used
            else
            {
                string hash;
                hash = GetMd5Hash(password);
                SQLiteCommand command1 = connection.CreateCommand();
                command1.CommandText = "insert into Users (Name, Password,CntAll,CntWin,CntLose) values (?,?,?,?,?)";
                command1.Parameters.Add(new SQLiteParameter("par1", @name));
                command1.Parameters.Add(new SQLiteParameter("par2", @hash));
                command1.Parameters.Add(new SQLiteParameter("par3", "0"));
                command1.Parameters.Add(new SQLiteParameter("par4", "0"));
                command1.Parameters.Add(new SQLiteParameter("par5", "0"));
                command1.ExecuteNonQuery();
                return true;

            }

        }


        private static string GetMd5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

        public static string ShowInfo(string name)
        {
            string result;
            Statistic stat = new Statistic();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM 'Users' WHERE Name =@name";
            command.Parameters.Add(new SQLiteParameter("@name", @name));
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                string id = reader["Id"].ToString();
                string value = reader["Name"].ToString();
                int win = Convert.ToInt32(reader["CntWin"]);
                int lose = Convert.ToInt32(reader["CntLose"]);
                int num = Convert.ToInt32(reader["CntAll"]);
                result = "Id:" + id + " " + "Name: " + value + " " + "Namber of games: " + num + " " + "Win: " + win + " " + "Lose: " + lose;
                //serialization
                stat.id = id;
                stat.name = value;
                stat.all = num;
                stat.win = win;
                stat.lose = lose;
                MemoryStream stream1 = new MemoryStream();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Statistic));
                ser.WriteObject(stream1, stat);
                stream1.Position = 0;
                StreamReader sr = new StreamReader(stream1);
                Console.Write("JSON form of Statistic object: ");
                Console.WriteLine(sr.ReadToEnd());
                return result;
            }
            else
            {
                result = "Error";
                return result;
            }

            
        }
        public static bool Update(string Name,bool win)
        {
            if (win)
            {
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE Users SET CntAll = Cntall + 1,CntWin = CntWin + 1 Where Name =@name";
                command.Parameters.Add(new SQLiteParameter("@name", @Name));
                command.ExecuteNonQuery();
                return true;

            }
            else
            {
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE Users SET CntAll = Cntall + 1,CntLose = CntLose + 1 Where Name =@name";
                command.Parameters.Add(new SQLiteParameter("@name", @Name));
                command.ExecuteNonQuery();
                return true;
            }

        }

    }
}
