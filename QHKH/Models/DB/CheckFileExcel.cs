using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KHQH.Models.DB
{
    public class CheckFileExcel
    {
        public CheckFileExcel(int row,string err)
        {
            this.Row = row;
            this.Error = err;
        }
        public int Row { get; set; }
        public string Error { get; set; }

    }
}