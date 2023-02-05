using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices;
using System.IO;

namespace FreemodeHanlder
{


    internal class IniParsing
    {
        private string path;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public IniParsing(string INIPath)
        {
            path = INIPath;
        }

        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }

        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.path);
            return temp.ToString();
        }
    }

    class Utils
    {

        public static IniParsing LoadedIni;

        public static string BytesToString(byte[] Buffer)
        {
            string str = "";
            for (int i = 0; i < Buffer.Length; i++) str = str + Buffer[i].ToString("X2");
            return str;
        }

        public static string GetSqlHostName()
        {
            return LoadedIni.IniReadValue("mysql", "host");
        }

        public static string GetSqlUserName()
        {
            return LoadedIni.IniReadValue("mysql", "username");
        }

        public static string GetSqlPassword()
        {
            return LoadedIni.IniReadValue("mysql", "password");
        }

        public static string GetSqlDatabase()
        {
            return LoadedIni.IniReadValue("mysql", "database");
        }

        public static short NumberOfClients()
        {
            return Convert.ToInt16(LoadedIni.IniReadValue("config", "NumberOfClients"));
        }

        public static int NumberOfSeconds()
        {
            return Convert.ToInt32(LoadedIni.IniReadValue("config", "NumberOfSeconds"));
        }

        public static long GetTimeStamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        public static void AppendText(string str, ConsoleColor color)
        {
            StreamWriter log;
            try
            {
                string time = string.Format("{0:hh:mm:ss tt}", DateTime.Now.ToUniversalTime().ToLocalTime());
                Console.ForegroundColor = color;
                Console.WriteLine(string.Concat(new object[] { "[", time, "]", " ", str, "" }));
                if (!File.Exists("Freemode.log")) { log = new StreamWriter("Freemode.log"); } else { log = File.AppendText("Freemode.log"); }
                log.WriteLine(string.Concat(new object[] { "[", time, "]", " ", str, "" })); log.Close();
            }
            catch
            {
                string time = string.Format("{0:hh:mm:ss tt}", DateTime.Now.ToUniversalTime().ToLocalTime());
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(string.Concat(new object[] { "[", time, "]", "An Error Has Occured" }));
                if (!File.Exists("Freemode.log")) { log = new StreamWriter("Freemode.log"); } else { log = File.AppendText("Freemode.log"); }
                log.WriteLine(string.Concat(new object[] { "[", time, "]", "An Error Has Occured" })); log.Close();
            }
        }

    }
}
