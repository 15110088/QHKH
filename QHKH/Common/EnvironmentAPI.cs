using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KHQH.Common
{
    public static class EnvironmentAPI
    {
        public static string UserSDE = System.Configuration.ConfigurationManager.AppSettings["USERSDE"];
        public static string PassSDE = System.Configuration.ConfigurationManager.AppSettings["PASSSDE"];
        public static string PortSDE = System.Configuration.ConfigurationManager.AppSettings["PORTSDE"];
        public static string ServerSDE = System.Configuration.ConfigurationManager.AppSettings["SERVERNAME"];
        public static string DbSDE = System.Configuration.ConfigurationManager.AppSettings["DBNAME"];
        public static string StrConnectSDE = System.Configuration.ConfigurationManager.AppSettings["conStrDataBaseSDE"];
        public static string URLDOWNFILE = AppDomain.CurrentDomain.BaseDirectory + "\\Uploads";

    }
}