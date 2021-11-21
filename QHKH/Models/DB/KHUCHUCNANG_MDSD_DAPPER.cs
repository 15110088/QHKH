using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KHQH.Models.DB
{
    public class KHUCHUCNANG_MDSD_DAPPER
    {
        [Required]
        public string ID_MDSD { get; set; }

        [Required]
        public decimal? DIENTICH { get; set; }

        [Required]
        public int? ID_KHUCN { get; set; }

        [Dapper.Contrib.Extensions.Key]
        public int? ID { get; set; }
    }
}