using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QHKH.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        protected string LoginName
        {
            get
            {
                // get cookie value
                string userName = "";
                if (Session["UserName"] != null) userName = Convert.ToString(Session["UserName"]);
                return userName;
            }
        }

        //protected string MaHuyen
        //{
        //    get
        //    {
        //        // get cookie value
        //        string userName = "";
        //        if (Session["MaHuyen"] != null) userName = Convert.ToString(Session["MaHuyen"]);
        //        return userName;
        //    }
        //}

        //protected string MaXa
        //{
        //    get
        //    {
        //        // get cookie value
        //        string userName = "";
        //        if (Session["MaXa"] != null) userName = Convert.ToString(Session["MaXa"]);
        //        return userName;
        //    }
        //}
    }
}