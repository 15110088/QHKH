using KHQH.Models;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace KHQH.Common
{
    public class ExportToExcelHelper
    {
        public static Stream UpdateDataIntoExcelTemplate<T>(List<T> data, FileInfo path)
        {
            Stream stream = new MemoryStream();
            if (path.Exists)
            {
                using (ExcelPackage p = new ExcelPackage(path))
                {
                    var excelApp = new Excel.Application();
                    var workbook = excelApp.Workbooks.Add();

                    // single worksheet
                    //Excel._Worksheet wsEstimate = excelApp.ActiveSheet;
                    ExcelWorksheet wsEstimate = p.Workbook.Worksheets["data"];
                    wsEstimate.Cells[4, 6].Value = "nghia";
                    //wsEstimate.Cells["F6:E8"].LoadFromCollection(data);
                    p.SaveAs(stream);
                    stream.Position = 0;
                }
            }
            return stream;
        }
    }
}