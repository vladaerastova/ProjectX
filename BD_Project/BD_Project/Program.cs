using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;


namespace BD_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            BD.BDOpen();
            //bool tmp = myBD.AddPerson("vovaa", "12345dfvvg");
            bool tmp = BD.Update("vovaa", false);
            Console.WriteLine(tmp);
            Console.WriteLine(BD.ShowInfo("vovaa"));
            //BD.Authorization("Masya", "bdc23");
            //BD.Authorization("Masya", "bdc123");
            //BD.AddPerson("Masya","bdc123");
            //BD.ShowInfo("Vasya");
            //BD.Update("Vasya", true);
            //BD.Update("Vasya", false);
            //BD.GetNumberOfGames("Vasya");
            Statistic stat = new Statistic();

            Console.ReadKey(true);
        }
    }
}
