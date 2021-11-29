using KEHOACHQH.DAL;
using KHQH.Models.DB;
using KHQH.Models.JSONDATA;
using QHKH.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KHQH.Controllers
{
    public class TienIchController : BaseController
    {
        KHQHEntities dbEF = new KHQHEntities();

        // GET: TienIch
        public ActionResult CanhBao()
        {
            if (LoginInfo == null)
            {
                return Redirect("/");
            }
            return View();
        }
        public ActionResult ImportData()
        {
            if (LoginInfo == null)
            {
                return Redirect("/");
            }
            CombineHienTrang cb = new CombineHienTrang();


            var dataKVHC = dbEF.DM_KVHC.Where(n => (n.DELETED == null || n.DELETED == false)).ToList();
            cb.LstPage = PageDB.data.Where(n => n.TYPE == 1 || n.TYPE == 2 || n.TYPE == 4).ToList();
            cb.LstHuyen = dataKVHC.Where(n => n.ID_CAP_KVHC == 2).ToList();
            cb.LstXa = dataKVHC.Where(n => n.ID_CAP_KVHC == 1).ToList();
            return View(cb);
        }
        public ActionResult XuatBieuMau()
        {
            if (LoginInfo == null)
            {
                return Redirect("/");
            }
            CombineHienTrang cb = new CombineHienTrang();
            cb.LstPage = PageDB.data.Where(n => n.TYPE >10&& n.TYPE<=31).ToList();

            return View(cb);
        }

        public ActionResult KhoaKYQH()
        {
            if (LoginInfo == null)
            {
                return Redirect("/");
            }
            return View();
        }
        public ActionResult MapConfig()
        {
            if (LoginInfo == null)
            {
                return Redirect("/");
            }
            var dataKVHC = dbEF.DM_KVHC.Where(n => (n.DELETED == null || n.DELETED == false)).ToList();
            CombineHienTrang cb = new CombineHienTrang();
            cb.LstPage = PageDB.data.Where(n => n.TYPE > 30 && n.TYPE <= 43).ToList();
            cb.LstHuyen = dataKVHC.Where(n => n.ID_CAP_KVHC == 2).ToList();
            cb.LstXa = dataKVHC.Where(n => n.ID_CAP_KVHC == 1).ToList();
            return View(cb);

        }

        public ActionResult XuatBieuMauCapHuyen()
        {
            if (LoginInfo == null)
            {
                return Redirect("/");
            }
            var dataKVHC = dbEF.DM_KVHC.Where(n => (n.DELETED == null || n.DELETED == false)).ToList();

            CombineHienTrang cb = new CombineHienTrang();
            cb.LstPage = PageDB.data.Where(n => n.TYPE > 30&&n.TYPE<=43).ToList();
            cb.LstHuyen = dataKVHC.Where(n => n.ID_CAP_KVHC == 2).ToList();
            cb.LstXa = dataKVHC.Where(n => n.ID_CAP_KVHC == 1).ToList();
            return View(cb);
        }

    }
}