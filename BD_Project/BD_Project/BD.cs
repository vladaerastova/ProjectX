using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SQLite;
using System.Security.Cryptography;

namespace BD_Project
{
    class BD
    {
        private SQLiteConnection connection;
        const string databaseName = @"D:\ProjectXDatebase.db";

        public BD() {
            connection = new SQLiteConnection(string.Format("Data Source={0};", databaseName));

        }
        public void BDOpen()
        {
            try
            {
                connection.Open();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void BDClose()
        {
            connection.Close();
        }

        
        public bool Authorization(string name,string password)
        {
            string hash;
            using (MD5 md5Hash = MD5.Create())
            {
                hash = GetMd5Hash(md5Hash, password);
            }
            SQLiteCommand command = new SQLiteCommand("SELECT count(Name) FROM 'Users' WHERE Name = '" + name + "' and Password = '"+hash+"'", connection);
            int countRows = Convert.ToInt32(command.ExecuteScalar());

            if (countRows != 0)
                return true;//Ok
            else
                return false;//wrong login or password

        }

        public bool AddPerson(string name,string password)
        {

            SQLiteCommand command = new SQLiteCommand("SELECT count(Name) FROM 'Users' WHERE Name = '" + name + "'", connection);
            int countRows = Convert.ToInt32(command.ExecuteScalar());

            if (countRows != 0)
                return false;//name is used
            else
            {
                string hash;
                using (MD5 md5Hash = MD5.Create())
                {
                    hash = GetMd5Hash(md5Hash, password);
                }
                string sql = "insert into Users (Name, Password,CntAll,CntWin,CntLose) values ('" + name + "','" + hash + "',0,0,0)";
                SQLiteCommand command1 = new SQLiteCommand(sql, connection);
                command1.ExecuteNonQuery();
                return true;

            }

        }


        private static string GetMd5Hash(MD5 md5Hash, string input)
        {

            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public string ShowInfo(string name)
        {
            string result;
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'Users' WHERE Name = '"+name+"'", connection);
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                string id = reader["Id"].ToString();
                string value = reader["Name"].ToString();
                int win = Convert.ToInt32(reader["CntWin"]);
                int lose = Convert.ToInt32(reader["CntLose"]);
                int num = Convert.ToInt32(reader["CntAll"]);
                //Console.Write("Id: {0}    Name: {1}   Number of Games: {2}   Win: {3}   Lose: {4}", id, value, num, win, lose);
                result = "Id:" + id + " " + "Name: " + value + " " + "Namber of games: " + num + " " + "Win: " + win + " " + "Lose: " + lose;
                return result;
            }
            else
            {
                result = "Error";
                return result;
            }

            
        }
        public bool Update(string Name,bool win)
        {
            if (win)
            {
                SQLiteCommand command = new SQLiteCommand("UPDATE Player SET NumberOfGames = NumberOfGames + 1,Win = Win + 1 Where Name = '"+Name+"'", connection);
                command.ExecuteNonQuery();
                return true;

            }
            else
            {
                SQLiteCommand command = new SQLiteCommand("UPDATE Player SET NumberOfGames = NumberOfGames + 1,Lose = Lose + 1 Where Name = '" + Name + "'", connection);
                command.ExecuteNonQuery();
                return true;
            }

        }

        public static int GetNumberOfGames(string name)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", databaseName));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'Player' WHERE Name = '" + name + "' ", connection);
            SQLiteDataReader reader = command.ExecuteReader();
            reader.Read();
            int num = Convert.ToInt32(reader["NumberofGames"]);
            Console.Write("Number of Games: {0}", num);
            connection.Close();
            return num;
        }
        public static int GetNumberOfWins(string name)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", databaseName));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'Player' WHERE Name = '" + name + "' ", connection);
            SQLiteDataReader reader = command.ExecuteReader();
            reader.Read();
            int win = Convert.ToInt32(reader["Win"]);
            Console.Write("Win: {0}", win);
            connection.Close();
            return win;
        }
        public static int GetNumberOfLosses(string name)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", databaseName));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'Player' WHERE Name = '" + name + "' ", connection);
            SQLiteDataReader reader = command.ExecuteReader();
            reader.Read();
            int lose = Convert.ToInt32(reader["Lose"]);
            Console.Write("Lose: {0}", lose);
            connection.Close();
            return lose;
        }


    }
}
