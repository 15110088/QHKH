using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KHQH.Models.DB
{
    [Table("DM_MUCDICHSUDUNG")]
    public class DM_MUCDICHSUDUNG_DAPPER
    {
        [Dapper.Contrib.Extensions.Key]
        public int ID { get; set; }
        public string KIHIEU { get; set; }
        public string TEN { get; set; }
        public string CHIMUC { get; set; }
        public DateTime NGAYTAO { get; set; }
        public int? ID_CAPTREN { get; set; }
        public bool ENABLED { get; set; }


        //public int? TT23 { get; set; }
        //public int? TT24 { get; set; }
        //public int? TT25 { get; set; }
        //public int? TT01 { get; set; }

        public bool? TT23 { get; set; }
        public bool? TT24 { get; set; }
        public bool? TT25 { get; set; }
        public bool? TT01 { get; set; }
        public bool? CAPTINH { get; set; }
        public int? STT { get; set; }

    }
}