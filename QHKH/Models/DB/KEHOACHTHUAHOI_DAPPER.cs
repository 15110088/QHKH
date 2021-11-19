using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KHQH.Models.DB
{
    [Table("KEHOACH_THUHOI")]
    public class KEHOACHTHUAHOI_DAPPER
    {
        [Required]
        public string MAHUYEN { get; set; }

        [Required]
        public string MAXA { get; set; }

        public decimal? DIENTICH { get; set; }

        [Required]
        public int? ID_MDSD { get; set; }

        public short? NAM { get; set; }

        public int? ID_KYQH { get; set; }

        [Dapper.Contrib.Extensions.Key]
        public int? ID { get; set; }
    }
}