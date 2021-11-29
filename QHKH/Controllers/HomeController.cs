using KEHOACHQH.DAL;
using KHQH.Common;

using KHQH.Models.DB;
using QHKH.Controllers;
using QHKH.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace KHQH.Controllers {
    public class HomeController : BaseController
    {
        private KHQHEntities dbEF = new KHQHEntities();

        public ActionResult Index()
        {

            CombineHienTrang cb = new CombineHienTrang();
            var dataKVHC = dbEF.DM_KVHC.Where(n => (n.DELETED == null || n.DELETED == false)).ToList();
            cb.LstHuyen = dataKVHC.Where(n => n.ID_CAP_KVHC == 2).ToList();
            cb.LstXa = dataKVHC.Where(n => n.ID_CAP_KVHC == 1).ToList();
            return View(cb);
        }

        public ActionResult Map(string Huyen,string Xa,bool IsCapTinh)
        {
            if(LoginInfo==null)
            {
                return Redirect("/");
            }
            Session["MaHuyen"] = Huyen;
            Session["MaXa"] = Xa;
            Session["TenHuyen"] = dbEF.DM_KVHC.FirstOrDefault(n=>n.MA_KVHC==Huyen).TEN_KVHC;
            Session["TenXa"] = dbEF.DM_KVHC.FirstOrDefault(n => n.MA_KVHC == Xa).TEN_KVHC; ;
            Session["ISCAPTINH"] = IsCapTinh;

            GhiLog("Đăng nhập HUYEN="+Huyen+" Xa="+Xa, "LOGIN");
            CombineHienTrang cb = new CombineHienTrang();
            var dataKVHC = dbEF.DM_KVHC.Where(n => (n.DELETED == null || n.DELETED == false)).ToList();
            cb.LstHuyen = dataKVHC.Where(n => n.ID_CAP_KVHC == 2).ToList();
            cb.LstXa = dataKVHC.Where(n => n.ID_CAP_KVHC == 1).ToList();
            if (dbEF.MAP_CONFIG.Where(n => n.MAKVHC == Xa).FirstOrDefault() != null)
            {
                ViewData["URL"] = dbEF.MAP_CONFIG.Where(n => n.MAKVHC == Xa).FirstOrDefault().MAP_SERVICES;

            }
            else
            {
                ViewData["URL"] = dbEF.MAP_CONFIG.Where(n => n.DEFAULT_VIEW == true).FirstOrDefault().MAP_SERVICES;

            }
            //ViewData["URL"] = "http://192.169.3.157:6080/arcgis/rest/services/GD/Ky1Tong26680V6/MapServer";
            return View(cb);
        }
        [HttpPost]
        public JsonResult SignIn(USERTABLE_DAPPER uSERTABLE)
        {
            try
            {
                if (uSERTABLE.TENDANGNHAP == null)
                {
                    return Json(ResultAPI<USERTABLE_DAPPER>.DATA(null, null, false, "Đăng nhập thất bại", 500, ""));

                }
                string strPass = FormsAuthentication.HashPasswordForStoringInConfigFile(uSERTABLE.MATKHAU, "MD5");
                //  var CheckUser = db.EGetAll<USERTABLE_DAPPER>().FirstOrDefault(n => n.TENDANGNHAP.ToLower() == uSERTABLE.TENDANGNHAP.ToLower() && n.MATKHAU.ToLower() == strPass.ToLower());
                var CheckUser = dbEF.USERTABLE.FirstOrDefault(n => n.TENDANGNHAP.ToLower() == uSERTABLE.TENDANGNHAP.ToLower() && n.MATKHAU.ToLower() == strPass.ToLower());

                if (CheckUser != null)
                {
                    //  HttpContext.Current.Session["LogOut"] = "Đăng Xuất";
                    Session.Clear();
                    Session["UserName"] = CheckUser.TENDANGNHAP;
              

                    Session["LogOut"] = "ĐĂNG XUẤT";
                    return Json(ResultAPI<USERTABLE_DAPPER>.DATA(null, null, true, "Đăng nhập thành công", 200, ""));
                }
                // Set cookie value

                return Json(ResultAPI<USERTABLE_DAPPER>.DATA(null, null, false, "Đăng nhập thất bại", 500, ""));
            }
            catch (Exception e)
            {
                return Json(ResultAPI<USERTABLE_DAPPER>.DATA(null, null, false, " Lõi kết noi DB ", 500, ""));

            }


        }

        [HttpPost]
        public JsonResult SignOut(USERTABLE_DAPPER uSERTABLE)
        {
            Session.Clear();


            return Json(ResultAPI<USERTABLE_DAPPER>.DATA(null, null, true, "Đăng xuất thành công", 200, ""));

        }
        public ActionResult DownloadXlsxReport()
        {
            //List<DM_CHUYENMUCDICH_DAPPER> dm = db.EGetAll<DM_CHUYENMUCDICH_DAPPER>().ToList();
            //List<Certification> cList = new List<Certification> {
            //    new Certification {
            //        EmployeeName = "Anupam Maiti", CertificationName = "AWS Certified Cloud Practitioner", CertificationCode = "CLF-C01", CertificateIssuingAuthority = "AWS"
            //    },
            //    new Certification {
            //        EmployeeName = "Anupam Maiti", CertificationName = "Microsoft Certified: DevOps Engineer Expert", CertificationCode = "AZ-400", CertificateIssuingAuthority = "Microsoft"
            //    },
            //    new Certification {
            //        EmployeeName = "Anupam Maiti", CertificationName = "Microsoft Certified: Azure Developer Associate", CertificationCode = "AZ-204", CertificateIssuingAuthority = "Microsoft"
            //    }
            //    };
            //string timestamp = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToUpper().Replace(':', '_').Replace('.', '_').Replace(' ', '_').Trim();
            //var templateFileInfo = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Template", "CertificationsExportTemplate.xlsx"));
            //var stream = ExportToExcelHelper.UpdateDataIntoExcelTemplate(cList, templateFileInfo);
            //  return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CertificationsReport-" + timestamp + ".xlsx");
            return null;
        }
    }
}