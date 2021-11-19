using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KHQH.Models.DB
{
    public class PageData
    {
        public int TYPE { get; set; }
        public string URLPOST { get; set; }
        public string URLGET { get; set; }
        public string URLPUT { get; set; }
        public string URLDELETE { get; set; }
        public string TITLE { get; set; }
        public string TableSDE { get; set; }


    }
}