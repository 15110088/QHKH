using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QHKH.Models.Excel
{
    public class BM09CT
    {
        public Int64 ID { get; set; }

        public string LOAIHANGMUC { get; set; }
        public string CHIMUC { get; set; }
        public string TENCONGTRINH { get; set; }
        public string TENDIADIEM { get; set; }
        public Decimal NAMTH { get; set; }
        public decimal DIENTICH_KH { get; set; }
        public decimal DIENTICH_HT { get; set; }
        public decimal DIENTICH_TT { get; set; }


    }
}