using KEHOACHQH.DAL;
using KHQH.Models.DB;
using KHQH.Models.JSONDATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KHQH.Controllers
{
    public class TienIchController : Controller
    {
        KHQHEntities dbEF = new KHQHEntities();

        // GET: TienIch
        public ActionResult CanhBao()
        {
            return View();
        }
        public ActionResult ImportData()
        {
            CombineHienTrang cb = new CombineHienTrang();


            var dataKVHC = dbEF.DM_KVHC.Where(n => (n.DELETED == null || n.DELETED == false)).ToList();
            cb.LstPage = PageDB.data.Where(n => n.TYPE == 1 || n.TYPE == 2 || n.TYPE == 4).ToList();
            cb.LstHuyen = dataKVHC.Where(n => n.ID_CAP_KVHC == 2).ToList();
            cb.LstXa = dataKVHC.Where(n => n.ID_CAP_KVHC == 1).ToList();
            return View(cb);
        }
        public ActionResult XuatBieuMau()
        {
            CombineHienTrang cb = new CombineHienTrang();
            cb.LstPage = PageDB.data.Where(n => n.TYPE >10).ToList();

            return View(cb);
        }

    }
}