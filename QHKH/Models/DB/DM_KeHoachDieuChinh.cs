using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KHQH.Models.DB
{
    public class DM_KeHoachDieuChinh
    {
        public DM_KeHoachDieuChinh(int ID ,string Ten)
        {
            this.ID = ID;
            this.Ten = Ten;
        }
       public int ID { get; set; }
        public string Ten { get; set; }

    }
}