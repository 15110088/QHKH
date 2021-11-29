using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KHQH.Models.DB
{
    [Table("MAP_CONFIG")]
    public class MAPCONFIG_DAPPER
    {

        [Dapper.Contrib.Extensions.Key]
        public int ID { get; set; }
       
        public int? ID_KYQH { get; set; }
      
        public string MAKVHC { get; set; }
      
        public string MAP_SERVICES { get; set; }

        public bool? DEFAULT_VIEW { get; set; }
    }
}