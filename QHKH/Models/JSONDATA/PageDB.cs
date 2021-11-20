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

            new PageData
            {
                TITLE="KHUC CHỨC NĂNG MỤC ĐÍCH SỬ DỤNG",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=10,
                TableSDE="SDE.CHUYENDE_VUNG"

            },

            new PageData
            {
                TITLE="Biểu 01/CT Hiện trạng sử dụng đất",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=11,
                TableSDE="Biểu 01/CT"
            },
            new PageData
            {
                TITLE="Biểu 02/CT Kết quả thực hiện kế hoạch sử dụng đất kỳ trước",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=12,
                TableSDE="Biểu 02/CT"
            },
             new PageData
            {
                TITLE="Biểu 03/CT Kế hoạch sử dụng đất",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=13,
                TableSDE="Biểu 03/CT"
            },
            new PageData
            {
                TITLE="Biểu 04/CT Kế hoạch sử dụng đất phân theo năm",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=14,
                TableSDE="Biểu 04/CT"
            },
            new PageData
            {
                TITLE="Biểu 05/CT Kế hoạch chuyển mục đích sử dụng đất phân theo đơn vị hành chính",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=15,
                TableSDE="Biểu 05/CT"
            },
            new PageData
            {
                TITLE="Biểu 06/CT Kế hoạch chuyển mục đích sử dụng đất phân theo năm",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=16,
                TableSDE="Biểu 06/CT"
            },
            new PageData
            {
                TITLE="Biểu 07/CT Kế hoạch đưa đất chưa sử dụng phân theo đơn vị hành chính",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=17,
                TableSDE="Biểu 07/CT"
            },
            new PageData
            {
                TITLE="Biểu 08/CT Kế hoạch đưa đất chưa sử dụng phân theo năm",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=18,
                TableSDE="Biểu 08/CT"
            },
            new PageData
            {
                TITLE="Biểu 09/CT Danh mục công trình, dự án thực hiện trong kỳ kế hoạch",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=19,
                TableSDE="Biểu 09/CT"
            },
            new PageData
            {
                TITLE="Biểu 10/CT Diện tích cơ cấu sử dụng đất các khu chức năng",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=20,
                TableSDE="Biểu 10/CT"
            },
            new PageData
            {
                TITLE="Biểu 11/CT Chu chuyển đất đai trong kỳ kế hoạch sử dụng đất",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=21,
                TableSDE="Biểu 11/CT"
            },
                             new PageData
            {
                TITLE="Biểu 01/CH Hiện trạng sử dụng đất",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=31,
                TableSDE="Biểu 01/CH"
            },
             new PageData
            {
                TITLE="Biểu 02/CH Kết quả thực hiện kế hoạch sử dụng đất kỳ trước",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=32,
                TableSDE="Biểu 02/CH"
            },
             new PageData
            {
                TITLE="Biểu 03/CH Quy hoạch (điều chỉnh) sử dụng đất",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=33,
                TableSDE="Biểu 03/CH"
            },
              new PageData
            {
                TITLE="Biểu 04/CH Diện tích chuyển mục đích sử dụng đất trong kỳ quy hoạch",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=34,
                TableSDE="Biểu 04/CH"
            },
               new PageData
            {
                TITLE="Biểu 05/CH Diện tích đất chưa sử dụng đưa vào sử dụng trong kỳ quy hoạch",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=35,
                TableSDE="Biểu 05/CH"
            },
                new PageData
            {
                TITLE="Biểu 06/CH Kế hoạch sử dụng đất",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=36,
                TableSDE="Biểu 06/CH"
            },

            new PageData
            {
                TITLE="Biểu 07/CH Kế hoạch chuyển mục đích sử dụng đất",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=37,
                TableSDE="Biểu 07/CH"
            },

            new PageData
            {
                TITLE="Biểu 08/CH Kế hoạch thu hồi đất",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=38,
                TableSDE="Biểu 08/CH"
            },

            new PageData
            {
                TITLE="Biểu 09/CH Kế hoạch đưa đất chưa sử dụng vào sử dụng",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=39,
                TableSDE="Biểu 09/CH"
            },

                        new PageData
            {
                TITLE="Biểu 10/CH Danh mục các công trình, dự án thực hiện trong năm",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=40,
                TableSDE="Biểu 10/CH"
            },

                            new PageData
            {
                TITLE="Biểu 11/CH Diện tích, cơ cấu sử dụng đất các khu chức năng huyện",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=41,
                TableSDE="Biểu 11/CH"
            },

                                     new PageData
            {
                TITLE="Biểu 12/CH Chu chuyển đất đai trong kỳ quy hoạch sử dụng đất 10 năm",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=42,
                TableSDE="Biểu 12/CH"
            },
                                              new PageData
            {
                TITLE="Biểu 13/CH Chu chuyển đất đai trong kế hoạch sử dụng đất theo năm",
                URLGET="",
                URLPOST="",
                URLPUT="",
                URLDELETE="",
                TYPE=43,
                TableSDE="Biểu 13/CH"
            },



        };
    }
}