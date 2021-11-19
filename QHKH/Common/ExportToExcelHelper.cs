using KHQH.Models;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace KHQH.Common
{
    public class ExportToExcelHelper
    {
        public static Stream UpdateDataIntoExcelTemplate(List<Certification> cList, FileInfo path)
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
                    ExcelWorksheet wsEstimate = p.Workbook.Worksheets["Certifications"];
                    wsEstimate.Cells["B5:E8"].LoadFromCollection(cList);
                    p.SaveAs(stream);
                    stream.Position = 0;
                }
            }
            return stream;
        }
    }
}