using QHKH.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KHQH.Controllers
{
    public class DanhMucController : BaseController
    {
        // GET: DanhMuc
        public ActionResult MDSD()
        {
            if (LoginInfo == null)
            {
                return Redirect("/");
            }
            return View();
        }
        public ActionResult CMD()
        {
            if (LoginInfo == null)
            {
                return Redirect("/");
            }
            return View();
        }
        public ActionResult CT()
        {
            if (LoginInfo == null)
            {
                return Redirect("/");
            }
            return View();
        }
        public ActionResult KYQH()
        {
            if (LoginInfo == null)
            {
                return Redirect("/");
            }
            return View();
        }
        public ActionResult KCN()
        {
            if (LoginInfo == null)
            {
                return Redirect("/");
            }
            return View();
        }

        public ActionResult KVHC()
        {
            return View();
        }
    }

}