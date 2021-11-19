using KHQH.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KHQH.Models.JSONDATA
{
    public static class PageDB
    {
        public static List<PageData> data = new List<PageData>()
        {
            new PageData
            {
                TITLE="HIỆN TRẠNG ",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=1,
                TableSDE="SDE.HienTrang_vung",

            },
             new PageData
            {
                TITLE="KẾ HOẠCH ",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=2,
                TableSDE="SDE.KEHOACH_vung",

            },
             new PageData
            {
                TITLE="KHU CHỨC NĂNG ",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=3,
                TableSDE="SDE.KHUCHUCNANG_VUNG",
            },
             new PageData
            {


                  TITLE="QUY HOẠCH ",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=4,
                TableSDE="SDE.QUYHOACH_VUNG"

            },
              new PageData
            {
                TITLE="CÔNG TRÌNH",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=5,
                TableSDE="SDE.CONGTRINH_VUNG"

            },

           new PageData
            {
                TITLE="DIỆN TÍCH PHÂN BỐ XÁC ĐỊNH",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=6,
                TableSDE="SDE.CHUYENDE_VUNG"

            },

            new PageData
            {
                TITLE="KẾ HOẠCH CHUYỂN MỤC ĐÍCH",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=7,
                TableSDE="SDE.CHUYENDE_VUNG"

            },

           new PageData
            {
                TITLE="KẾ HOẠCH CSD",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=8,
                TableSDE="SDE.CHUYENDE_VUNG"

            },

            new PageData
            {
                TITLE="KẾ HOẠCH THU HỒI",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=9,
                TableSDE="SDE.CHUYENDE_VUNG"

            },


        };
    }
}