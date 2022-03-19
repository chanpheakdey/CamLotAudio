using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ACEAppAPI.Utilities
{
    public class Connection
    {
        //public static string GetConnectionString(string Database)
        //{
        //    return "Server=tcp:acecambodia.database.windows.net;Database="+Database+";User Id=sqlserver;Password=SQLvb$92";
        //}
        public static string GetConnectionString(string Database)
        {
            if (Database.Equals("EDB"))
            {
                return "Server=tcp:acecambodia.database.windows.net;Database=EDB_Test;User Id=sqlserver;Password=SQLvb$92";
            }
            else
            {
                return $"Server=tcp:acecambodia.database.windows.net;Database={Database};User Id=sqlserver;Password=SQLvb$92";
            }
        }
    }
}
