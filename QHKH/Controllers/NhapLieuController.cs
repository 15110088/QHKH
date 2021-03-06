using KHQH.Models.DB;
using KHQH.Models.JSONDATA;
using QHKH.Controllers;
using QHKH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KHQH.Controllers
{
    public class NhapLieuController : BaseController
    {
        KHQHEntities dbEF = new KHQHEntities();


        int a = 0;
        // GET: NhapLieu
        public ActionResult NL_HIENTRANG(int ID=1)
        {
            if (LoginInfo == null)
            {
                return Redirect("/");
            }
            CombineHienTrang cb = new CombineHienTrang();
            HIENTRANG_DAPPER ht = new HIENTRANG_DAPPER();


            
                var dataMDSD = dbEF.DM_MUCDICHSUDUNG.Where(n => n.ENABLED == true).ToList();
                var dataKVHC = dbEF.DM_KVHC.Where(n =>  (n.DELETED == null || n.DELETED == false)).ToList();


                cb.LstMDSD = dataMDSD;
                cb.LstHuyen = dataKVHC.Where(n=>n.ID_CAP_KVHC==2).ToList();
                cb.LstXa = dataKVHC.Where(n => n.ID_CAP_KVHC == 1).ToList();
                cb.LstLoaiKCN = dbEF.DM_LOAIKHUCHUCNANG.ToList();
                cb.SetupPage = PageDB.data.Where(n=>n.TYPE==ID).FirstOrDefault();

            ViewData["MaXa"] = MaXa;
            ViewData["MaHuyen"] = MaHuyen;
            ViewData["ISCAPTINH"] = ISCAPTINH;
            cb.HienTrang = ht;
               return View(cb);
        }

        public ActionResult NL_QUYHOACH()
        {
            return View();
        }

        public ActionResult NL_CONGTRINH(int ID=5)
        {
            if (LoginInfo == null)
            {
                return Redirect("/");
            }
            CombineHienTrang cb = new CombineHienTrang();
            HIENTRANG_DAPPER ht = new HIENTRANG_DAPPER();



            var dataCT = dbEF.DM_LOAICONGTRINH.ToList();
            var dataKVHC = dbEF.DM_KVHC.Where(n => (n.DELETED == null || n.DELETED == false)).ToList();


            cb.LstCT = dataCT;
            cb.LstHuyen = dataKVHC.Where(n => n.ID_CAP_KVHC == 2).ToList();
            cb.LstXa = dataKVHC.Where(n => n.ID_CAP_KVHC == 1).ToList();
            cb.LstLoaiKCN = dbEF.DM_LOAIKHUCHUCNANG.ToList();
            cb.SetupPage = PageDB.data.Where(n => n.TYPE == ID).FirstOrDefault();


            cb.HienTrang = ht;
            ViewData["MaXa"] = MaXa;
            ViewData["MaHuyen"] = MaHuyen;
            ViewData["ISCAPTINH"] = ISCAPTINH;
            return View(cb);
        }

        public ActionResult NL_KEHOACH(int ID=7)
        {
            if (LoginInfo == null)
            {
                return Redirect("/");
            }
            CombineHienTrang cb = new CombineHienTrang();
            HIENTRANG_DAPPER ht = new HIENTRANG_DAPPER();

            var dataMDSD = dbEF.DM_MUCDICHSUDUNG.Where(n => n.ENABLED == true).ToList();
            var dataKVHC = dbEF.DM_KVHC.Where(n => (n.DELETED == null || n.DELETED == false)).ToList();

            
            cb.LstMDSD = dataMDSD;
            cb.LstHuyen = dataKVHC.Where(n => n.ID_CAP_KVHC == 2).ToList();
            cb.LstXa = dataKVHC.Where(n => n.ID_CAP_KVHC == 1).ToList();
            cb.LstCMD = dbEF.DM_CHUYENMUCDICH.ToList();
            cb.SetupPage = PageDB.data.Where(n => n.TYPE == ID).FirstOrDefault();
            cb.HienTrang = ht;
            ViewData["MaXa"] = MaXa;
            ViewData["MaHuyen"] = MaHuyen;
            ViewData["ISCAPTINH"] = ISCAPTINH;
            return View(cb);

        }



        public ActionResult NL_DIENTICHPHANBOXACDINH()
        {
            if (LoginInfo == null)
            {
                return Redirect("/");
            }
            CombineHienTrang cb = new CombineHienTrang();
            HIENTRANG_DAPPER ht = new HIENTRANG_DAPPER();



            var dataMDSD = dbEF.DM_MUCDICHSUDUNG.Where(n => n.ENABLED == true).ToList();
            var dataKVHC = dbEF.DM_KVHC.Where(n => (n.DELETED == null || n.DELETED == false)).ToList();


            cb.LstMDSD = dataMDSD;
            cb.LstHuyen = dataKVHC.Where(n => n.ID_CAP_KVHC == 2).ToList();
            cb.LstXa = dataKVHC.Where(n => n.ID_CAP_KVHC == 1).ToList();
            cb.LstLoaiKCN = dbEF.DM_LOAIKHUCHUCNANG.ToList();
            cb.SetupPage = PageDB.data.Where(n => n.TYPE == 6).FirstOrDefault();


            cb.HienTrang = ht;
            ViewData["MaXa"] = MaXa;
            ViewData["MaHuyen"] = MaHuyen;
            ViewData["ISCAPTINH"] = ISCAPTINH;

            return View(cb);
        }
    }
}