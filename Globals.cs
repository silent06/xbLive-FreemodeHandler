using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreemodeHanlder
{
    class Global
    {
        public static int iConnectedClients = 0;
        public static int iPort = 4000;
        public static int iWebhookPort = 15423;
        public static int iMaximumRequestSize = 0x1000;
        public static bool bFreemode = false;
        public static int iCurrentXexVersion = 0;
        public static int iEncryptionStructSize = 44;
        public static string host;
        public static string Username;
        public static string password;
        public static string Database;
        public static short NumberOfClients;
        public static int NumberOfSeconds;
    }

    public enum ClientInfoStatus
    {
        Authed,
        NoTime,
        Banned,
        Disabled
    }

 
    public struct ClientInfo
    {
        public int iID;
        public int iTimeEnd;
        public ClientInfoStatus Status;

    }

    

    
}
