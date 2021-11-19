using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KHQH.Models.DB
{
    [Table("DM_CHUYENMUCDICH")]
    public class DM_CHUYENMUCDICH_DAPPER
    {
        [Dapper.Contrib.Extensions.Key]
        public int ID { get; set; }
        public string KYHIEU { get; set; }
        public string TEN { get; set; }
        public int ENABLED { get; set; }
        public int CAPTINH { get; set; }
        public int ID_CAPTREN { get; set; }
        public string CHIMUC { get; set; }
        public string CHIMUCTINH { get; set; }

    }
}