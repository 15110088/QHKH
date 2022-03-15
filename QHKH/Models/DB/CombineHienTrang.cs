using QHKH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KHQH.Models.DB
{
    public class CombineHienTrang
    {
        public HIENTRANG_DAPPER HienTrang { get; set; }
        public IEnumerable<DM_MUCDICHSUDUNG> LstMDSD { get; set; }
        public IEnumerable<DM_KVHC> LstHuyen { get; set; }
        public IEnumerable<DM_KVHC> LstXa { get; set; }
        public IEnumerable<DM_LOAIKHUCHUCNANG> LstLoaiKCN { get; set; }
        public IEnumerable<DM_CHUYENMUCDICH> LstCMD { get; set; }
        public IEnumerable<DM_LOAICONGTRINH> LstCT { get; set; }

        public PageData SetupPage { get; set; }
        public List<PageData> LstPage { get; set; }


    }
}