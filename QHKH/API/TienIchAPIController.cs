using ExcelDataReader;
using KEHOACHQH.DAL;
using KHQH.Common;
using KHQH.Models.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public object DATA()
        {
            DBOracleHelper db = new DBOracleHelper();
                           DataTable dt = DBOracleHelper.ExecuteProcedure("ST_BieuMau01CT");
            return dt.Columns.Count;
        }
    }
}
