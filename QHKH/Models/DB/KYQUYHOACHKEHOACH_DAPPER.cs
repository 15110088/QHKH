using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KHQH.Models.DB
{
    [Table("KYQUYHOACHKEHOACH")]
    public class KYQUYHOACHKEHOACH_DAPPER
    {
   
        public int? ID_CHA { get; set; }
       
        public int? IS_KEHOACH_DIEUCHINH { get; set; }
      
        public bool? BIKHOA { get; set; }

        [StringLength(4)]
        public short TOINAM { get; set; }

        [StringLength(4)]
        public short TUNAM { get; set; }
      
        public short? NAM { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string TEN { get; set; }

        [Dapper.Contrib.Extensions.Key]
        public int ID { get; set; }
    }
}