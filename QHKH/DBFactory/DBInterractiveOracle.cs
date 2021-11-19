using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KHQH.DBFactory
{
    public class DBInterractiveOracle
    {
        string ConnectionString = "";
        public DBInterractiveOracle()
        {

             ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }
    }
}