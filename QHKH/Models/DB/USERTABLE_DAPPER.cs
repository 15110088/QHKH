using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KHQH.Models.DB
{
    [Table("USERTABLE")]
    public class USERTABLE_DAPPER
    {
        [Dapper.Contrib.Extensions.Key]
        public int ID { get; set; }
        public string HOTEN { get; set; }
        public string TENDANGNHAP { get; set; }
        public string MATKHAU { get; set; }
        public string TENPHONGBAN { get; set; }
        public int MAPHONGBAN { get; set; }
        public int MAPHONGBANCHA { get; set; }
        public int ENABLE { get; set; }
        public string GHICHU { get; set; }

    }
}