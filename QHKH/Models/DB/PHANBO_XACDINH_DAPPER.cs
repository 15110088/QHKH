using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KHQH.Models.DB
{
    [Table("PHANBO_XACDINH")]
    public class PHANBO_XACDINH_DAPPER
    {

        [Dapper.Contrib.Extensions.Key]
        public int ?ID { get; set; }

        [Required]
        public int? ID_KYQH { get; set; }
     
        public int? ID_MDSD { get; set; }
    
        public decimal? DT_PHANBO { get; set; }
      
        public decimal? DT_XACDINH { get; set; }

        public int? ID_KHUCHUCNANG { get; set; }
        public bool? CAPTINH { get; set; }

        public string MAHUYEN { get; set; }


        public string MAXA { get; set; }
    }
}