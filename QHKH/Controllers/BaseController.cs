using KHQH.Controllers;
using QHKH.Interface;
using QHKH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QHKH.Controllers
{
    public class BaseController : Controller,IBase
    {
        protected  KHQHEntities dbEF = new KHQHEntities();


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            base.OnActionExecuting(filterContext);
            if (LoginInfo == null)
            {
                string actionName = ControllerContext.RouteData.Values["action"].ToString();
                string homeName = ControllerContext.RouteData.Values["controller"].ToString();
                if (homeName == "Home" && actionName == "Index")
                {

                }
                else if(homeName == "Home" && actionName == "SignIn")
                {

                }
                else
                {
                    filterContext.Result = Redirect("/");

                }
            }


        }





        public USERTABLE LoginInfo
        {
            get
            {
                string userName = "";

                if (System.Web.HttpContext.Current.Session["UserName"] != null)
                {
                    userName = Convert.ToString(System.Web.HttpContext.Current.Session["UserName"]);
                    return dbEF.USERTABLEs.FirstOrDefault(n => n.TENDANGNHAP == userName);
                }
                else
                    return null;
            }
        }





        protected void GhiLog(string NoiDung, string KEY)
        {

            using (var dbEF2 = new KHQHEntities())
            {
                USERTABLE_LOG log = new USERTABLE_LOG();
                log.THOIGIAN = DateTime.Now;
                log.NOIDUNG = NoiDung;
                log.KEY = KEY;
                log.ID_USER = LoginInfo.ID;
                log.ROWID = Guid.NewGuid();
                dbEF2.USERTABLE_LOG.Add(log);
                dbEF2.SaveChanges();
                if (dbEF2.Database.Connection.State != System.Data.ConnectionState.Open)
                {
                    dbEF2.Database.Connection.Close();
                }
            }
               
        }

        void IBase.GhiLogAPI(string NoiDung, string KEY)
        {
            GhiLog(NoiDung, KEY);

        }
        public void ClearSesstion(string Sessionkey)
        {
            if (System.Web.HttpContext.Current.Session[Sessionkey] != null)
                System.Web.HttpContext.Current.Session.Remove(Sessionkey);
        }
        protected string MaHuyen
        {
            get
            {
                // get cookie value
                string HUYEN = "";
                if (Session["MaHuyen"] != null) HUYEN = Convert.ToString(Session["MaHuyen"]);
                return HUYEN;
            }
        }

        protected string MaXa
        {
            get
            {
                // get cookie value
                string XA = "";
                if (Session["MaXa"] != null) XA = Convert.ToString(Session["MaXa"]);
                return XA;
            }
        }

        protected bool ISCAPTINH
        {
            get
            {
                // get cookie value
                bool ISCAPTINH = false;
                if (Session["ISCAPTINH"] != null) ISCAPTINH = Convert.ToBoolean(Session["ISCAPTINH"]);
                return ISCAPTINH;
            }
        }
    }
}