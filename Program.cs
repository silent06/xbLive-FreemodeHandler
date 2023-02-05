using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace FreemodeHanlder
{
    class Program
    {

        static void Main(string[] args)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\MySQLconfig.ini";
            if (!File.Exists(filePath))
            {
                throw new Exception("MySQLconfig file not found");
            }
            File.WriteAllText("Freemode.log", String.Empty);
            Utils.LoadedIni = new IniParsing(filePath);
            Global.Database = Utils.GetSqlDatabase();
            Global.host = Utils.GetSqlHostName();
            Global.password = Utils.GetSqlPassword();
            Global.Username = Utils.GetSqlUserName();
            Global.NumberOfClients = Utils.NumberOfClients();
            Global.NumberOfSeconds = Utils.NumberOfSeconds();
            Utils.AppendText(string.Concat(new object[] { DateTime.Now.ToLongDateString(), "******************[Running FreemodeHandler]******************", "\n" }), ConsoleColor.Green);
            Utils.AppendText(string.Concat(new object[] { DateTime.Now.ToLongDateString(), " NumberOfClients: ", Global.NumberOfClients, "\n" }), ConsoleColor.Green);
            for (int i = 0; i < Global.NumberOfClients; i++)
            {
                ClientInfo info = new ClientInfo();
                Utils.AppendText(string.Concat(new object[] { DateTime.Now.ToLongDateString(), " Checking... ID: ", i, "\n" }), ConsoleColor.Green);
                if (MySQL.ClientData(i.ToString(), ref info))
                {
                    int secondsAdded = Global.NumberOfSeconds;//in seconds adding one day 
                    MySQL.UpdateClientFreemode(i.ToString(), ref secondsAdded);
                    Utils.AppendText(string.Concat(new object[] { DateTime.Now.ToLongDateString(), " Adding Seconds: ", Global.NumberOfSeconds.ToString(), " to Client... ", "\n" }), ConsoleColor.Green);
                    Thread.Sleep(5000);
                }
                else {
                    Utils.AppendText(string.Concat(new object[] { DateTime.Now.ToLongDateString(), "  :(  ", "not found for ID: ", i.ToString(), "\n" }), ConsoleColor.Red);
                }
            }
            Console.WriteLine("Done! \n");
            Console.ReadKey();
        }
    }
}
