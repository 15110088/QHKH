using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KHQH.Models.DB
{
    public class HIENTRANG_DAPPER
    {

        [Dapper.Contrib.Extensions.Key]
        public int ID { get; set; }

        public bool? CAPTINH { get; set; }
        
        [Required]
         public string TENVUNG { get; set; }

        [StringLength(4)]
        public short? NAM { get; set; }

        [Required]
        public string MAHUYEN { get; set; }

     
        public string MAXA { get; set; }

        [Required]
        public decimal? DIENTICH { get; set; }
        [Required]
        public decimal? DIENTICH_HT { get; set; }
        [Required]
        public decimal? DIENTICH_KH { get; set; }


        [Required]
        public int? ID_MDSD { get; set; }

        [Required]
        public string MAHT { get; set; }

        [Required]
        public int? ID_KYQH { get; set; }

        [Required]
        public int? ID_LOAIKCN { get; set; }


        [Required]
        public int? ID_LOAICT { get; set; }


        [Required]
        public string TENCONGTRINH { get; set; }
        [Required]
        public string SUDUNGVAOLD { get; set; }
        [Required]
        public string TENDIADIEM { get; set; }
        [Required]
        public string VITRITRENBD { get; set; }


    }
}