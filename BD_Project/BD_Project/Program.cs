using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SQLite;

namespace BD_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            BD myBD = new BD();
            myBD.BDOpen();
            bool tmp = myBD.AddPerson("masha", "12345");
            Console.WriteLine(tmp);
            Console.WriteLine(myBD.ShowInfo("masha"));
            //BD.Authorization("Masya", "bdc23");
            //BD.Authorization("Masya", "bdc123");
            //BD.AddPerson("Masya","bdc123");
            //BD.ShowInfo("Vasya");
            //BD.Update("Vasya", true);
            //BD.Update("Vasya", false);
            //BD.GetNumberOfGames("Vasya");
            Console.ReadKey(true);
        }
    }
}
