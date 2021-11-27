using Dapper;
using ExcelDataReader;
using KEHOACHQH.DAL;
using KHQH.Common;
using KHQH.Models.DB;
using KHQH.Models.JSONDATA;
using OfficeOpenXml;
using Oracle.ManagedDataAccess.Client;
using QHKH.Common;
using QHKH.Models.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace KHQH.API
{
    [Route("api/TienIchAPI/{action}", Name = "TienIchAPI")]
    public class TienIchAPIController : ApiController
    {
        private KHQHEntities dbEF = new KHQHEntities();
        DBFactory.DBInteractive db = new DBFactory.DBInteractive();

        [HttpPost]
        public object CheckImportExcel(int ID)
        {
            #region Variable Declaration  
            List<int> listErrIdx = new List<int>();
            List<string> listErrDetail = new List<string>();
            List<CheckFileExcel> listErr = new List<CheckFileExcel>();
            string message = "";


            var httpRequest = HttpContext.Current.Request;
            DataSet dsexcelRecords = new DataSet();
            IExcelDataReader reader = null;
            Stream FileStream = null;
            #endregion
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {


                var DMLOAIDAT = dbEF.DM_MUCDICHSUDUNG.ToList();

                string PathFile = EnvironmentAPI.URLDOWNFILE;
                string PathFileConvert = PathFile + "\\" + Guid.NewGuid();
                Directory.CreateDirectory(PathFileConvert);
                for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
                {
                    var MAHUYEN = HttpContext.Current.Request.Form["MAHUYEN"];
                    var MAXA = HttpContext.Current.Request.Form["MAXA"];
                    var IDKYQH = HttpContext.Current.Request.Form["IDKYQH"];


                    var httpPostedFile = HttpContext.Current.Request.Files[i];
                    if (httpPostedFile != null)
                    {
                        FileStream = httpPostedFile.InputStream;
                        if (httpPostedFile.FileName.EndsWith(".xls"))
                            reader = ExcelReaderFactory.CreateBinaryReader(FileStream);
                        else if (httpPostedFile.FileName.EndsWith(".xlsx"))
                            reader = ExcelReaderFactory.CreateOpenXmlReader(FileStream);
                        else
                            message = "The file format is not supported.";

                        dsexcelRecords = reader.AsDataSet();
                        reader.Close();
                        List<HIENTRANG> lists = new List<HIENTRANG>();

                        if (dsexcelRecords != null && dsexcelRecords.Tables.Count > 0)
                        {
                            string strErr = string.Empty;

                            DataTable dtExcel = dsexcelRecords.Tables[0];
                            for (int j = 1; j < dtExcel.Rows.Count; j++)
                            {
                                int idx = 0;
                                HIENTRANG ht = new HIENTRANG();

                                String MaVung = Convert.ToString(dtExcel.Rows[j][idx++]).Trim();
                                String LoaiDat = Convert.ToString(dtExcel.Rows[j][idx++]).Trim();
                                String TenVung = Convert.ToString(dtExcel.Rows[j][idx++]).Trim();
                                if (ID == 1)
                                {
                                    // kiểm tra mã vùng
                                    if (dbEF.HIENTRANGs.FirstOrDefault(n => n.MAHT == MaVung) != null)
                                    {
                                        strErr += "Mã vũng đã tồn tại ";
                                    }
                                    //kiểm tra loại đất
                                    if (DMLOAIDAT.FirstOrDefault(n => n.KIHIEU.ToUpper().Trim() == LoaiDat.ToUpper().Trim()) == null)
                                    {
                                        strErr += "Loại đất không có danh mục";
                                    }
                                }
                                if (ID == 2)
                                {
                                    // kiểm tra mã vùng
                                    if (dbEF.KEHOACHes.FirstOrDefault(n => n.MAKH == MaVung) != null)
                                    {
                                        strErr += "Mã vũng đã tồn tại ";
                                    }
                                    //kiểm tra loại đất
                                    if (DMLOAIDAT.FirstOrDefault(n => n.KIHIEU.ToUpper().Trim() == LoaiDat.ToUpper().Trim()) == null)
                                    {
                                        strErr += "Loại đất không có danh mục";
                                    }
                                }
                                if (ID == 4)
                                {
                                    // kiểm tra mã vùng
                                    if (dbEF.QUYHOACHes.FirstOrDefault(n => n.MAQH == MaVung) != null)
                                    {
                                        strErr += "Mã vũng đã tồn tại ";
                                    }
                                    //kiểm tra loại đất
                                    if (DMLOAIDAT.FirstOrDefault(n => n.KIHIEU.ToUpper().Trim() == LoaiDat.ToUpper().Trim()) == null)
                                    {
                                        strErr += "Loại đất không có danh mục";
                                    }
                                }


                                if (!string.IsNullOrEmpty(strErr))
                                {
                                    listErrIdx.Add(j);
                                    listErrDetail.Add(strErr);
                                    listErr.Add(new CheckFileExcel(j, strErr));
                                }
                                else
                                {

                                    lists.Add(ht);
                                }
                            }
                        }


                        // Validate the uploaded image(optional)
                        if (lists.Count > 0 && listErrIdx.Count == 0)
                        {




                            try
                            {


                                return Json(new { isValid = true, result = lists.ToArray(), data = lists.ToArray(), responseText = "Imported successfully!" });
                            }
                            catch (Exception ex)
                            {
                                listErrIdx.Add(listErrIdx.Count);
                                listErrDetail.Add(ex.Message);
                                return Json(new { isValid = false, result = listErrIdx.ToArray(), data = listErrDetail.ToArray(), responseText = "Cannot import" });


                            }
                        }

                        // Get the complete file path

                        string newFileName = Guid.NewGuid() + Path.GetExtension(httpPostedFile.FileName);
                        string fname = Path.Combine(PathFileConvert, httpPostedFile.FileName);

                        httpPostedFile.SaveAs(fname);
                        // Save the uploaded file to "UploadedFiles" folder

                    }
                    return Json(new { isValid = false, result = listErrIdx.ToArray(), data = listErr.ToArray(), responseText = "Cannot import" });


                }



            }
            return true;




        }

        [HttpPost]
        public object ImportExcel(int ID)
        {
            #region Variable Declaration  
            List<int> listErrIdx = new List<int>();
            List<string> listErrDetail = new List<string>();
            List<CheckFileExcel> listErr = new List<CheckFileExcel>();
            string message = "";

            var httpRequest = HttpContext.Current.Request;
            DataSet dsexcelRecords = new DataSet();
            IExcelDataReader reader = null;
            Stream FileStream = null;
            #endregion
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {


                var DMLOAIDAT = dbEF.DM_MUCDICHSUDUNG.ToList();

                string PathFile = EnvironmentAPI.URLDOWNFILE;
                string PathFileConvert = PathFile + "\\" + Guid.NewGuid();
                Directory.CreateDirectory(PathFileConvert);
                for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
                {
                    var MAHUYEN = HttpContext.Current.Request.Form["MAHUYEN"];
                    var MAXA = HttpContext.Current.Request.Form["MAXA"];
                    var IDKYQH = HttpContext.Current.Request.Form["IDKYQH"];
                    var CAPTINH = HttpContext.Current.Request.Form["CAPTINH"];


                    var httpPostedFile = HttpContext.Current.Request.Files[i];
                    if (httpPostedFile != null)
                    {
                        FileStream = httpPostedFile.InputStream;
                        if (httpPostedFile.FileName.EndsWith(".xls"))
                            reader = ExcelReaderFactory.CreateBinaryReader(FileStream);
                        else if (httpPostedFile.FileName.EndsWith(".xlsx"))
                            reader = ExcelReaderFactory.CreateOpenXmlReader(FileStream);
                        else
                            message = "The file format is not supported.";

                        dsexcelRecords = reader.AsDataSet();
                        reader.Close();
                        List<HIENTRANG> lists = new List<HIENTRANG>();
                        List<QUYHOACH> listsQH = new List<QUYHOACH>();
                        List<KEHOACH> listsKH = new List<KEHOACH>();

                        if (dsexcelRecords != null && dsexcelRecords.Tables.Count > 0)
                        {
                            string strErr = string.Empty;

                            DataTable dtExcel = dsexcelRecords.Tables[0];
                            for (int j = 1; j < dtExcel.Rows.Count; j++)
                            {
                                int idx = 0;
                                HIENTRANG ht = new HIENTRANG();
                                QUYHOACH qh = new QUYHOACH();
                                KEHOACH kh = new KEHOACH();

                                String MaVung = Convert.ToString(dtExcel.Rows[j][idx++]).Trim();
                                String LoaiDat = Convert.ToString(dtExcel.Rows[j][idx++]).Trim();
                                String TenVung = Convert.ToString(dtExcel.Rows[j][idx++]).Trim();



                                if (ID == 1)
                                {
                                    ht.MAHT = MaVung;
                                    ht.ID_MDSD = DMLOAIDAT.FirstOrDefault(n => n.KIHIEU.ToUpper().Trim() == LoaiDat.ToUpper().Trim()).ID;
                                    ht.TENVUNG = TenVung;
                                    ht.MAHUYEN = Convert.ToString(MAHUYEN ?? "0");
                                    ht.MAXA = Convert.ToString(MAXA ?? "0");
                                    ht.ID_KYQH = Convert.ToInt32(IDKYQH ?? "0");
                                    ht.CAPTINH = Convert.ToBoolean(CAPTINH ?? "false");
                                }
                                if (ID == 2)
                                {
                                    kh.MAKH = MaVung;
                                    kh.ID_MDSD = DMLOAIDAT.FirstOrDefault(n => n.KIHIEU.ToUpper().Trim() == LoaiDat.ToUpper().Trim()).ID;
                                    kh.TENVUNG = TenVung;
                                    kh.MAHUYEN = Convert.ToString(MAHUYEN ?? "0");
                                    kh.MAXA = Convert.ToString(MAXA ?? "0");
                                    kh.ID_KYQH = Convert.ToInt32(IDKYQH ?? "0");
                                    kh.CAPTINH = Convert.ToBoolean(CAPTINH ?? "false");
                                }
                                if (ID == 4)
                                {
                                    qh.MAQH = MaVung;
                                    qh.ID_MDSD = DMLOAIDAT.FirstOrDefault(n => n.KIHIEU.ToUpper().Trim() == LoaiDat.ToUpper().Trim()).ID;
                                    qh.TENVUNG = TenVung;
                                    qh.MAHUYEN = Convert.ToString(MAHUYEN ?? "0");
                                    qh.MAXA = Convert.ToString(MAXA ?? "0");
                                    qh.ID_KYQH = Convert.ToInt32(IDKYQH ?? "0");
                                    qh.CAPTINH = Convert.ToBoolean(CAPTINH ?? "false");
                                }

                                if (!string.IsNullOrEmpty(strErr))
                                {
                                    listErrIdx.Add(j);
                                    listErrDetail.Add(strErr);
                                    listErr.Add(new CheckFileExcel(j, strErr));
                                }
                                else
                                {

                                    lists.Add(ht);
                                    listsQH.Add(qh);
                                    listsKH.Add(kh);

                                }
                            }
                        }


                        // Validate the uploaded image(optional)
                        if (lists.Count > 0 && listErrIdx.Count == 0)
                        {




                            try
                            {
                                using (var dbQH = new KHQHEntities())
                                {
                                    if (ID == 1)
                                    {
                                        foreach (HIENTRANG s in lists)
                                        {
                                            dbQH.HIENTRANGs.AddObject(s);
                                        }
                                        dbQH.SaveChanges();

                                    }
                                    if (ID == 2)
                                    {
                                        foreach (KEHOACH s in listsKH)
                                        {
                                            dbQH.KEHOACHes.AddObject(s);
                                            dbQH.SaveChanges();
                                        }
                                    }
                                    if (ID == 4)
                                    {
                                        foreach (QUYHOACH s in listsQH)
                                        {
                                            dbQH.QUYHOACHes.AddObject(s);
                                            dbQH.SaveChanges();
                                        }
                                    }

                                }

                                return Json(new { isValid = true, result = lists.ToArray(), responseText = "Imported successfully!" });
                            }
                            catch (Exception ex)
                            {
                                listErrIdx.Add(listErrIdx.Count);
                                listErrDetail.Add(ex.Message);
                                return Json(new { isValid = false, result = listErrIdx.ToArray(), data = listErrDetail.ToArray(), responseText = "Cannot import" });


                            }
                        }

                        // Get the complete file path

                        string newFileName = Guid.NewGuid() + Path.GetExtension(httpPostedFile.FileName);
                        string fname = Path.Combine(PathFileConvert, httpPostedFile.FileName);

                        httpPostedFile.SaveAs(fname);
                        // Save the uploaded file to "UploadedFiles" folder

                    }
                    return Json(new { isValid = false, result = listErrIdx.ToArray(), data = listErr.ToArray(), responseText = "Cannot import" });


                }



            }
            return true;




        }



        [HttpGet]
        public HttpResponseMessage XuatBieuMau(int ID=1,int IDKYQH=0)
        {
            DBOracleHelper db = new DBOracleHelper();
     
            List<OracleParameter> parameters = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter();
            p1.ParameterName = "IDKYQH";
            p1.Value = IDKYQH;
            p1.OracleDbType = OracleDbType.Int32;
            parameters.Add(p1);
            string SQLBIEUMAU = "";
            string TEMPLATE = "";

            switch (ID)
            {
                case 11:
                    SQLBIEUMAU = "ST_BieuMau01CT";
                    TEMPLATE = "BM01CT.xlsx";
                    break;
                case 12:
                    SQLBIEUMAU = "ST_BieuMau02CT";
                    TEMPLATE = "BM02CT.xlsx";
                    break;
                case 13:
                    SQLBIEUMAU = "ST_BieuMau03CT_KEHOACH";
                    TEMPLATE = "BM03CT.xlsx";
                    break;
                case 14:
                    SQLBIEUMAU = "ST_BieuMau04CT";
                    TEMPLATE = "BM04CT.xlsx";
                    break;
                case 15:
                    SQLBIEUMAU = "ST_BieuMau05CT";
                    TEMPLATE = "BM05CT.xlsx";
                    break;

                case 16:
                    SQLBIEUMAU = "ST_BieuMau06CT";
                    TEMPLATE = "BM06CT.xlsx";

                    break;

                case 17:
                    SQLBIEUMAU = "ST_BieuMau07CT";
                    TEMPLATE = "BM07CT.xlsx";
                
                    break;

                case 18:
                    SQLBIEUMAU = "ST_BieuMau08CT";
                    TEMPLATE = "BM08CT.xlsx";
                    break;

                case 19:
                    SQLBIEUMAU = "ST_BieuMau09CT";
                    TEMPLATE = "BM09CT.xlsx";

                    break;

                case 20:
                    SQLBIEUMAU = "ST_BieuMau10CT";
                    TEMPLATE = "BM10CT.xlsx";

                    break;

                case 21:
                    SQLBIEUMAU = "ST_BieuMau11CT";
                    break;

            }
            DataTable dt = DBOracleHelper.ExecuteProcedure(SQLBIEUMAU, parameters);
            List<BM01CT> data = new List<BM01CT>();
            data = DataHelper.ConvertDataTable<BM01CT>(dt);
            string timestamp = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToUpper().Replace(':', '_').Replace('.', '_').Replace(' ', '_').Trim();
            var path = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BIEUMAUEXCEL", TEMPLATE));
            // var stream = ExportToExcelHelper.UpdateDataIntoExcelTemplate<BM01CT>(data, path);

            Stream stream = new MemoryStream();
            if (path.Exists)
            {
                using (ExcelPackage p = new ExcelPackage(path))
                {
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    var workbook = excelApp.Workbooks.Add();

                    // single worksheet
                    //Excel._Worksheet wsEstimate = excelApp.ActiveSheet;
                    ExcelWorksheet wsEstimate = p.Workbook.Worksheets[0];
                    var DanhMucHuyen = dbEF.DM_KVHC.Where(n => n.ID_CAP_KVHC == 2).OrderBy(n=>n.MA_KVHC).ToList();
                    var DanhMucKCN = dbEF.DM_LOAIKHUCHUCNANG.Where(n => n.CAPTINH==true).OrderBy(n => n.ID).ToList();

                    if (ID==11)
                    {
                        int ColHeader = 6;

                        foreach (var item in DanhMucHuyen)
                        {
                            wsEstimate.Cells[4, ColHeader].Value = item.TEN_KVHC;

                            if (data.Where(n => n.MAHUYEN == item.MA_KVHC).FirstOrDefault() != null)
                            {
                                wsEstimate.Cells[6, ColHeader].LoadFromCollection(data.Where(n => n.MAHUYEN == item.MA_KVHC).Select(n => n.DIENTICH));
                            }

                            ColHeader++;

                        }
                    }

                    if (ID == 13)
                    {
                        DataTable dt2 = DBOracleHelper.ExecuteProcedure("ST_BieuMau03CT", parameters);

                        List<BM03CT> data2 = new List<BM03CT>();
                        data2 = DataHelper.ConvertDataTable<BM03CT>(dt2);
                        int ColHeader = 7;

                        foreach (var item in DanhMucHuyen)
                        {
                            wsEstimate.Cells[4, ColHeader].Value = item.TEN_KVHC;

                            if (data.Where(n => n.MAHUYEN == item.MA_KVHC).FirstOrDefault() != null)
                            {
                                wsEstimate.Cells[6, ColHeader].LoadFromCollection(data.Where(n => n.MAHUYEN == item.MA_KVHC).Select(n => n.DIENTICH));
                            }

                            ColHeader++;

                        }
                        wsEstimate.Cells[6, 4].LoadFromCollection(data2.Select(n => n.DT_PHANBO));
                        wsEstimate.Cells[6, 5].LoadFromCollection(data2.Select(n => n.DT_XACDINH));


                    }
                   



                    if (ID == 12)
                    {
                         wsEstimate.Cells[7, 4].LoadFromCollection(data.Select(n => n.DIENTICH));
                               
                    }

                    if (ID == 14)
                    {
                        DataTable dt2 = DBOracleHelper.ExecuteProcedure("ST_BieuMau04CT_HIENTRANG", parameters);

                        List<BM01CT> data2 = new List<BM01CT>();
                        data2 = DataHelper.ConvertDataTable<BM01CT>(dt2);
                        int ColHeader = 5;

                        // tìm danh mục theo năm kế hoạch
                        var datanam = from n in dbEF.KEHOACHes
                                      join m in dbEF.KYQUYHOACHKEHOACHes
                                      on n.ID_KYQH equals m.ID
                                      where n.NAM >= m.TUNAM && n.NAM <= m.TOINAM
                                      select new { n.NAM }
                                      ;

                        foreach (var item in datanam)
                        {
                            wsEstimate.Cells[4, ColHeader].Value = item.NAM;
                            //string nametemp = Convert.ToString(item.NAM);
                            if (data.Where(n => n.MAHUYEN == item.NAM.ToString()).FirstOrDefault() != null)
                            {
                                wsEstimate.Cells[6, ColHeader].LoadFromCollection(data.Where(n => n.MAHUYEN == item.NAM.ToString()).Select(n => n.DIENTICH));
                            }

                            ColHeader++;

                        }
                        wsEstimate.Cells[6, 4].LoadFromCollection(data2.Select(n => n.DIENTICH));


                    }

                    if (ID == 15)
                    {
                        int ColHeader = 4;

                        foreach (var item in DanhMucHuyen)
                        {
                            wsEstimate.Cells[4, ColHeader].Value = item.TEN_KVHC;

                            if (data.Where(n => n.MAHUYEN == item.MA_KVHC).FirstOrDefault() != null)
                            {
                                wsEstimate.Cells[6, ColHeader].LoadFromCollection(data.Where(n => n.MAHUYEN == item.MA_KVHC).Select(n => n.DIENTICH));
                            }

                            ColHeader++;

                        }
                    }

                    if (ID == 17)
                    {
                        int ColHeader = 5;

                        foreach (var item in DanhMucHuyen)
                        {
                            wsEstimate.Cells[4, ColHeader].Value = item.TEN_KVHC;

                            if (data.Where(n => n.MAHUYEN == item.MA_KVHC).FirstOrDefault() != null)
                            {
                                wsEstimate.Cells[6, ColHeader].LoadFromCollection(data.Where(n => n.MAHUYEN == item.MA_KVHC).Select(n => n.DIENTICH));
                            }

                            ColHeader++;

                        }
                    }

                    if (ID == 18)
                    {
                        
                        int ColHeader = 5;

                        // tìm danh mục theo năm kế hoạch
                        var datanam = from n in dbEF.KEHOACH_CSD
                                      join m in dbEF.KYQUYHOACHKEHOACHes
                                      on n.ID_KYQH equals m.ID
                                      where n.NAM >= m.TUNAM && n.NAM <= m.TOINAM
                                      select new { n.NAM }
                                      ;

                        foreach (var item in datanam)
                        {
                            wsEstimate.Cells[4, ColHeader].Value = item.NAM;
                            //string nametemp = Convert.ToString(item.NAM);
                            if (data.Where(n => n.MAHUYEN == item.NAM.ToString()).FirstOrDefault() != null)
                            {
                                wsEstimate.Cells[6, ColHeader].LoadFromCollection(data.Where(n => n.MAHUYEN == item.NAM.ToString()).Select(n => n.DIENTICH));
                            }

                            ColHeader++;

                        }


                    }

                    if (ID == 19)
                    {

                        DataTable dt2 = DBOracleHelper.ExecuteProcedure("ST_BieuMau09CT", parameters);

                        List<BM09CT> data2 = new List<BM09CT>();
                        data2 = DataHelper.ConvertDataTable<BM09CT>(dt2);
                        int ColHeader = 2;
                        int RowHeader = 6;
                        int ColChild = 3;

                        var DanhMucCongTrinh = dbEF.DM_LOAICONGTRINH.Where(n => n.CAPTINH ==true).ToList();

                        foreach (var item in DanhMucCongTrinh)
                        {
                            if (item.ID == 1)
                            {
                                wsEstimate.Cells[RowHeader, ColHeader - 1].Value = "I";

                            }
                            else if (item.ID == 2)
                            {
                                wsEstimate.Cells[RowHeader, ColHeader - 1].Value = "II";

                            }
                            else
                            {
                                wsEstimate.Cells[RowHeader, ColHeader - 1].Value = item.CHIMUC;

                            }
                            wsEstimate.Cells[RowHeader, ColHeader].Value = item.LOAIHANGMUC;
                            wsEstimate.Cells[RowHeader, ColHeader].Style.Font.Bold = true;

                            wsEstimate.Cells[++RowHeader, ColHeader].LoadFromCollection(data2.Where(n=>n.ID==item.ID).Select(n=>n.TENCONGTRINH));
                            wsEstimate.Cells[RowHeader, ColChild].LoadFromCollection(data2.Where(n => n.ID == item.ID).Select(n => new {n.DIENTICH_KH,n.DIENTICH_HT,n.DIENTICH_TT,n.TENDIADIEM,n.NAMTH}));

                            RowHeader += data2.Where(n => n.ID == item.ID).Count();
                            if(item.ID==2)
                            {
                               
                            }

                        }
                    }


                    if (ID == 20)
                    {
                        int ColHeader = 4;

                        foreach (var item in DanhMucKCN)
                        {
                            // wsEstimate.Cells[4, ColHeader].Value = item.TEN_KVHC;

                            if (data.Where(n => n.MAHUYEN == item.KYHIEU).FirstOrDefault() != null)
                            {
                                wsEstimate.Cells[6, ColHeader].LoadFromCollection(data.Where(n => n.MAHUYEN == item.KYHIEU).Select(n => n.DIENTICH));
                            }

                            ColHeader = ColHeader + 2;

                        }
                    }

                    p.SaveAs(stream);
                    stream.Position = 0;
                }
            }


            // processing the stream.

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = PageDB.data.Where(n => n.TYPE ==ID).FirstOrDefault().TableSDE+ ".xlsx";
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            return response;

            //return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CertificationsReport-" + timestamp + ".xlsx");


        }


        //ID Loai bieu mau 
        //IDKYQH Ky QHKH
        //MAHUYEN thong ke cac xa trong huyen

        [HttpGet]
        public HttpResponseMessage XuatBieuMauCapHuyen(int ID = 1, int IDKYQH = 0, string MAHUYEN = "")
        {
            DBOracleHelper db = new DBOracleHelper();

            List<OracleParameter> parameters = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter();
            p1.ParameterName = "IDKYQH";
            p1.Value = IDKYQH;
            p1.OracleDbType = OracleDbType.Int32;
            parameters.Add(p1);
            OracleParameter p2 = new OracleParameter();
            p2.ParameterName = "IDHUYEN";
            p2.Value = MAHUYEN;
            p2.OracleDbType = OracleDbType.Varchar2;
            parameters.Add(p2);

            string SQLBIEUMAU = "";
            string TEMPLATE = "";

            switch (ID)
            {
                case 31:
                    SQLBIEUMAU = "ST_BieuMau01CH";
                    TEMPLATE = "BM01CH.xlsx";
                    break;
                case 32:
                    SQLBIEUMAU = "ST_BieuMau02CH";
                    TEMPLATE = "BM02CH.xlsx";
                    break;
                case 33:
                    SQLBIEUMAU = "ST_BieuMau03CH";
                    TEMPLATE = "BM03CH.xlsx";
                    break;
                case 34:
                    SQLBIEUMAU = "ST_BieuMau04CH";
                    TEMPLATE = "BM04CH.xlsx";
                    break;
                case 35:
                    SQLBIEUMAU = "ST_BieuMau05CH";
                    TEMPLATE = "BM05CH.xlsx";
                    break;

                case 36:
                    SQLBIEUMAU = "ST_BieuMau06CH";
                    TEMPLATE = "BM06CH.xlsx";

                    break;

                case 37:
                    SQLBIEUMAU = "ST_BieuMau07CH";
                    TEMPLATE = "BM07CH.xlsx";

                    break;

                case 38:
                    SQLBIEUMAU = "ST_BieuMau08CH";
                    TEMPLATE = "BM08CH.xlsx";
                    break;

                case 39:
                    SQLBIEUMAU = "ST_BieuMau09CH";
                    TEMPLATE = "BM09CH.xlsx";

                    break;

                case 40:
                    SQLBIEUMAU = "ST_BieuMau10CH";
                    TEMPLATE = "BM10CH.xlsx";

                    break;

                case 41:
                    SQLBIEUMAU = "ST_BieuMau11CH";
                    TEMPLATE = "BM11CH.xlsx";

                    break;

                case 42:
                    SQLBIEUMAU = "ST_BieuMau12CH";
                    TEMPLATE = "BM12CH.xlsx";

                    break;

                case 43:
                    SQLBIEUMAU = "ST_BieuMau13CH";
                    TEMPLATE = "BM13CH.xlsx";

                    break;

            }
            DataTable dt = DBOracleHelper.ExecuteProcedure(SQLBIEUMAU, parameters);
            List<BM01CT> data = new List<BM01CT>();
            data = DataHelper.ConvertDataTable<BM01CT>(dt);
            string timestamp = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToUpper().Replace(':', '_').Replace('.', '_').Replace(' ', '_').Trim();
            var path = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BIEUMAUEXCEL", TEMPLATE));
            // var stream = ExportToExcelHelper.UpdateDataIntoExcelTemplate<BM01CT>(data, path);

            Stream stream = new MemoryStream();
            if (path.Exists)
            {
                using (ExcelPackage p = new ExcelPackage(path))
                {
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    var workbook = excelApp.Workbooks.Add();

                    // single worksheet
                    //Excel._Worksheet wsEstimate = excelApp.ActiveSheet;
                    ExcelWorksheet wsEstimate = p.Workbook.Worksheets[0];
                    var DanhMucHuyen = dbEF.DM_KVHC.Where(n => n.ID_CAP_KVHC == 1 && n.MA_KVHC_CHA==MAHUYEN).OrderBy(n => n.MA_KVHC).ToList();
                    var DanhMucKCN = dbEF.DM_LOAIKHUCHUCNANG.OrderBy(n => n.ID).ToList();

                    if (ID == 31 || ID == 34 || ID == 35 || ID==36 || ID == 37 || ID == 38 || ID == 39)
                    {
                        int ColHeader = 5;

                        foreach (var item in DanhMucHuyen)
                        {
                            wsEstimate.Cells[4, ColHeader].Value = item.TEN_KVHC;

                            if (data.Where(n => n.MAHUYEN == item.MA_KVHC).FirstOrDefault() != null)
                            {
                                wsEstimate.Cells[6, ColHeader].LoadFromCollection(data.Where(n => n.MAHUYEN == item.MA_KVHC).Select(n => n.DIENTICH));
                            }

                            ColHeader++;

                        }
                    }
                    if (ID == 32)
                    {
                        wsEstimate.Cells[7, 4].LoadFromCollection(data.Select(n => n.DIENTICH));

                    }

                    if (ID == 33)
                    {
                        int ColHeader = 7;

                        
                          DataTable dt2 = DBOracleHelper.ExecuteProcedure("ST_BieuMau03CH_PHANBO", parameters);

                        List<BM03CT> data2 = new List<BM03CT>();
                        data2 = DataHelper.ConvertDataTable<BM03CT>(dt2);

                        wsEstimate.Cells[6, 5].LoadFromCollection(data2.Select(n => n.DT_PHANBO));
                        wsEstimate.Cells[6, 4].LoadFromCollection(data2.Select(n => n.DT_XACDINH));


                        foreach (var item in DanhMucHuyen)
                        {
                            wsEstimate.Cells[4, ColHeader].Value = item.TEN_KVHC;

                            if (data.Where(n => n.MAHUYEN == item.MA_KVHC).FirstOrDefault() != null)
                            {
                                wsEstimate.Cells[6, ColHeader].LoadFromCollection(data.Where(n => n.MAHUYEN == item.MA_KVHC).Select(n => n.DIENTICH));
                            }

                            ColHeader++;

                        }
                    }

                  

                    if (ID == 15)
                    {
                        int ColHeader = 4;

                        foreach (var item in DanhMucHuyen)
                        {
                            wsEstimate.Cells[4, ColHeader].Value = item.TEN_KVHC;

                            if (data.Where(n => n.MAHUYEN == item.MA_KVHC).FirstOrDefault() != null)
                            {
                                wsEstimate.Cells[6, ColHeader].LoadFromCollection(data.Where(n => n.MAHUYEN == item.MA_KVHC).Select(n => n.DIENTICH));
                            }

                            ColHeader++;

                        }
                    }

                    if (ID == 17)
                    {
                        int ColHeader = 5;

                        foreach (var item in DanhMucHuyen)
                        {
                            wsEstimate.Cells[4, ColHeader].Value = item.TEN_KVHC;

                            if (data.Where(n => n.MAHUYEN == item.MA_KVHC).FirstOrDefault() != null)
                            {
                                wsEstimate.Cells[6, ColHeader].LoadFromCollection(data.Where(n => n.MAHUYEN == item.MA_KVHC).Select(n => n.DIENTICH));
                            }

                            ColHeader++;

                        }
                    }

                    if (ID == 40)
                    {

                        DataTable dt2 = DBOracleHelper.ExecuteProcedure("ST_BieuMau10CH", parameters);

                        List<BM10CH> data2 = new List<BM10CH>();
                        data2 = DataHelper.ConvertDataTable<BM10CH>(dt2);
                        int ColHeader = 2;
                        int RowHeader = 6;
                        int ColChild = 3;

                        var DanhMucCongTrinh = dbEF.DM_LOAICONGTRINH.Where(n => n.CAPTINH == false).ToList();

                        foreach (var item in DanhMucCongTrinh)
                        {
                            
                            wsEstimate.Cells[RowHeader, ColHeader - 1].Value = item.CHIMUC;

                            
                            wsEstimate.Cells[RowHeader, ColHeader].Value = item.LOAIHANGMUC;
                            wsEstimate.Cells[RowHeader, ColHeader].Style.Font.Bold = true;

                            wsEstimate.Cells[++RowHeader, ColHeader].LoadFromCollection(data2.Where(n => n.ID == item.ID).Select(n => n.TENCONGTRINH));
                            wsEstimate.Cells[RowHeader, ColChild].LoadFromCollection(data2.Where(n => n.ID == item.ID).Select(n => new { n.DIENTICH_KH, n.DIENTICH_HT, n.DIENTICH_TT,n.LOAIDAT, n.TENDIADIEM, n.VITRITRENBD }));

                            RowHeader += data2.Where(n => n.ID == item.ID).Count();
                            if (item.ID == 2)
                            {

                            }

                        }
                    }

                    if (ID == 41)
                    {
                        int ColHeader = 4;

                        foreach (var item in DanhMucKCN)
                        {
                            // wsEstimate.Cells[4, ColHeader].Value = item.TEN_KVHC;

                            if (data.Where(n => n.MAHUYEN == item.KYHIEU).FirstOrDefault() != null)
                            {
                                wsEstimate.Cells[6, ColHeader].LoadFromCollection(data.Where(n => n.MAHUYEN == item.KYHIEU).Select(n => n.DIENTICH));
                            }

                            ColHeader = ColHeader + 2;

                        }
                    }

                    p.SaveAs(stream);
                    stream.Position = 0;
                }
            }


            // processing the stream.

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = PageDB.data.Where(n => n.TYPE == ID).FirstOrDefault().TableSDE + ".xlsx";
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            return response;

            //return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CertificationsReport-" + timestamp + ".xlsx");


        }
    }
}
