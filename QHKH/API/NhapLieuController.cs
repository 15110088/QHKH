using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using DoThiBienHoa_DLL.DAL;
using KHQH.Common;
using KHQH.Models.JSONDATA;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using KEHOACHQH.DAL;
using QHKH.Interface;
using QHKH.Controllers;

namespace KHQH.API
{

    [Route("api/NhapLieu/{action}", Name = "NhapLieu")]
    public class NhapLieuController : ApiController
    {
        private DBFactory.DBInteractive db = new DBFactory.DBInteractive();
        private string uploadRootFolderInput = EnvironmentAPI.URLDOWNFILE;
        string _strConnect = EnvironmentAPI.StrConnectSDE;
        IBase Log = new BaseController();

        private KHQHEntities dbEF = new KHQHEntities();

        #region Nhập liệu hiện trạng Thêm/Xóa/Sửa
        [HttpGet]
        public HttpResponseMessage GetHienTrangByID(DataSourceLoadOptions loadOptions, int ID)
        {
            //List<DM_CHUYENMUCDICH> data = db.EGetAll<DM_CHUYENMUCDICH>().Where(n => n.ENABLED == true).ToList();
            var data = dbEF.HIENTRANG.Where(n => n.ID_KYQH == ID).Select(n => new { n.ID, n.ID_KYQH, n.ID_MDSD, n.MAHT, n.MAHUYEN, n.MAXA, n.NAM, n.DIENTICH, n.CAPTINH, n.TENVUNG }).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(data, loadOptions));
        }



        [HttpPost]
        public HttpResponseMessage PostHienTrang(FormDataCollection form, int IDKYQH)
        {
            var values = form.Get("values");

            HIENTRANG info = new HIENTRANG();
            JsonConvert.PopulateObject(values, info);
            info.ID_KYQH = IDKYQH;


            dbEF.HIENTRANG.AddObject(info);
            dbEF.SaveChanges();

            Log.GhiLogAPI("Thêm hiện trạng " + info.MAHT + " Table = HIENTRANG", "NHAPLIEU");

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut]
        public HttpResponseMessage PutHienTrang(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            HIENTRANG info = dbEF.HIENTRANG.Where(n => n.ID == key).FirstOrDefault();

            string MAHTOld = string.Copy(info.MAHT);
            JsonConvert.PopulateObject(values, info);

            //Cập nhật mã vùng
            var clsUpdateShape = new clsUpdateShapeFile(_strConnect, EnvironmentAPI.ServerSDE, EnvironmentAPI.PortSDE, EnvironmentAPI.DbSDE, EnvironmentAPI.UserSDE, EnvironmentAPI.PassSDE);
            int count = clsUpdateShape.QuerySDE("select MAVUNG from HIENTRANG_VUNG where MAVUNG='" + MAHTOld + "'").Rows.Count;
            if (count > 0)
            {
                var xulySDE = clsUpdateShape.QuerySDE("Update HIENTRANG_VUNG set MAVUNG='" + info.MAHT + "' where MAVUNG='" + MAHTOld + "'");
                Log.GhiLogAPI("Cập nhật shape " + info.MAHT+ " Table = HIENTRANG_VUNG", "SHAPE");

            }
            dbEF.SaveChanges();
            Log.GhiLogAPI("Cập nhật hiện trạng " + info.MAHT +" ID="+ info.ID+ " Table = HIENTRANG", "NHAPLIEU");

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteHienTrang(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");
            HIENTRANG info = dbEF.HIENTRANG.Where(n => n.ID == key).FirstOrDefault();

            var clsUpdateShape = new clsUpdateShapeFile(_strConnect, EnvironmentAPI.ServerSDE, EnvironmentAPI.PortSDE, EnvironmentAPI.DbSDE, EnvironmentAPI.UserSDE, EnvironmentAPI.PassSDE);

            var xulySDE = clsUpdateShape.QuerySDE("Delete from HIENTRANG_VUNG where MAVUNG='" + info.MAHT + "'");

            dbEF.HIENTRANG.DeleteObject(info);
            dbEF.SaveChanges();
            Log.GhiLogAPI("Xóa hiện trạng " + info.MAHT + " ID=" + info.ID + " Table = HIENTRANG", "NHAPLIEU");

            return Request.CreateResponse(HttpStatusCode.Created);
        }






        #endregion


        #region Nhập liệu khu chức năng

        [HttpGet]
        public HttpResponseMessage GetKhuChucNangByID(DataSourceLoadOptions loadOptions, int ID)
        {
            //List<DM_CHUYENMUCDICH> data = db.EGetAll<DM_CHUYENMUCDICH>().Where(n => n.ENABLED == true).ToList();
            var data = dbEF.KHUCHUCNANG.Where(n => n.ID_KYQH == ID).Select(n => new { n.ID, n.ID_KYQH, n.MALOAIKHUCN, n.MAKHUCN, n.MAHUYEN, n.MAXA, n.CAPTINH, n.TENVUNG }).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(data, loadOptions));
        }



        [HttpPost]
        public HttpResponseMessage PostKhuChucNang(FormDataCollection form, int IDKYQH)
        {
            var values = form.Get("values");

            KHUCHUCNANG info = new KHUCHUCNANG();
            JsonConvert.PopulateObject(values, info);
            info.ID_KYQH = IDKYQH;


            dbEF.KHUCHUCNANG.AddObject(info);
            dbEF.SaveChanges();
            Log.GhiLogAPI("Thêm Khu Chức Năng " + info.TENVUNG + " ID=" + info.ID + " Table = KHUCHUCNANG", "NHAPLIEU");

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut]
        public HttpResponseMessage PutKhuChucNang(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            KHUCHUCNANG info = dbEF.KHUCHUCNANG.Where(n => n.ID == key).FirstOrDefault();

            string MAHTOld = string.Copy(info.MAKHUCN);
            JsonConvert.PopulateObject(values, info);

            //Cập nhật mã vùng
            //  var clsUpdateShape = new clsUpdateShapeFile(_strConnect, EnvironmentAPI.ServerSDE, EnvironmentAPI.PortSDE, EnvironmentAPI.DbSDE, EnvironmentAPI.UserSDE, EnvironmentAPI.PassSDE);

            //  var xulySDE = clsUpdateShape.QuerySDE("Update HIENTRANG_VUNG set MAVUNG='" + info.MAHT + "' where MAVUNG='" + MAHTOld + "'");
            //Cập nhật mã vùng
            var clsUpdateShape = new clsUpdateShapeFile(_strConnect, EnvironmentAPI.ServerSDE, EnvironmentAPI.PortSDE, EnvironmentAPI.DbSDE, EnvironmentAPI.UserSDE, EnvironmentAPI.PassSDE);
            int count = clsUpdateShape.QuerySDE("select MAVUNG from KHUCHUCNANG_VUNG where MAVUNG='" + MAHTOld + "'").Rows.Count;
            if (count > 0)
            {
                var xulySDE = clsUpdateShape.QuerySDE("Update KHUCHUCNANG_VUNG set MAVUNG='" + info.MAKHUCN + "' where MAVUNG='" + MAHTOld + "'");
                Log.GhiLogAPI("Cập nhật shape " + info.MAKHUCN + " Table = KHUCHUCNANG_VUNG", "SHAPE");

            }
            Log.GhiLogAPI("Cập nhật Khu Chức Năng " + info.TENVUNG + " ID=" + info.ID + " Table = KHUCHUCNANG", "NHAPLIEU");


            dbEF.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteKhuChucNang(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            KHUCHUCNANG info = dbEF.KHUCHUCNANG.Where(n => n.ID == key).FirstOrDefault();

            // var clsUpdateShape = new clsUpdateShapeFile(_strConnect, EnvironmentAPI.ServerSDE, EnvironmentAPI.PortSDE, EnvironmentAPI.DbSDE, EnvironmentAPI.UserSDE, EnvironmentAPI.PassSDE);

            // var xulySDE = clsUpdateShape.QuerySDE("Delete from HIENTRANG_VUNG where MAVUNG='" + info.MAHT + "'");

            dbEF.KHUCHUCNANG.DeleteObject(info);
            dbEF.SaveChanges();
            Log.GhiLogAPI("Xóa Khu Chức Năng " + info.TENVUNG + " ID=" + info.ID + " Table = KHUCHUCNANG", "NHAPLIEU");

            return Request.CreateResponse(HttpStatusCode.Created);
        }
        #endregion


        #region Nhập liệu quy hoạch Thêm/Xóa/Sửa
        [HttpGet]
        public HttpResponseMessage GetQuyHoachByID(DataSourceLoadOptions loadOptions, int ID)
        {
            //List<DM_CHUYENMUCDICH> data = db.EGetAll<DM_CHUYENMUCDICH>().Where(n => n.ENABLED == true).ToList();
            var data = dbEF.QUYHOACH.Where(n => n.ID_KYQH == ID).Select(n => new { n.ID, n.ID_KYQH, n.ID_MDSD, n.MAQH, n.MAHUYEN, n.MAXA, n.NAM, n.DIENTICH, n.CAPTINH, n.TENVUNG, n.ID_KHUCN }).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(data, loadOptions));
        }



        [HttpPost]
        public HttpResponseMessage PostQuyHoach(FormDataCollection form, int IDKYQH)
        {
            var values = form.Get("values");

            QUYHOACH info = new QUYHOACH();
            JsonConvert.PopulateObject(values, info);
            info.ID_KYQH = IDKYQH;


            dbEF.QUYHOACH.AddObject(info);
            dbEF.SaveChanges();
            Log.GhiLogAPI("Thêm quy hoạch" + info.TENVUNG + " ID=" + info.ID + " Table = QUYHOACH", "NHAPLIEU");

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut]
        public HttpResponseMessage PutQuyHoach(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            QUYHOACH info = dbEF.QUYHOACH.Where(n => n.ID == key).FirstOrDefault();

            string MAHTOld = string.Copy(info.MAQH);
            JsonConvert.PopulateObject(values, info);

            //Cập nhật mã vùng
            var clsUpdateShape = new clsUpdateShapeFile(_strConnect, EnvironmentAPI.ServerSDE, EnvironmentAPI.PortSDE, EnvironmentAPI.DbSDE, EnvironmentAPI.UserSDE, EnvironmentAPI.PassSDE);
            //Kiểm tra mã vùng
            int count = clsUpdateShape.QuerySDE("select MAVUNG from QUYHOACH_VUNG where MAVUNG='" + MAHTOld + "'").Rows.Count;
            if (count > 0)
            {
                var xulySDE = clsUpdateShape.QuerySDE("Update QUYHOACH_VUNG set MAVUNG='" + info.MAQH + "' where MAVUNG='" + MAHTOld + "'");
                Log.GhiLogAPI("Cập nhật shape " + info.MAQH + " Table = QUYHOACH_VUNG", "SHAPE");

            }
            dbEF.SaveChanges();
            Log.GhiLogAPI("Cập nhật quy hoạch" + info.TENVUNG + " ID=" + info.ID + " Table = QUYHOACH", "NHAPLIEU");

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteQuyHoach(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            QUYHOACH info = dbEF.QUYHOACH.Where(n => n.ID == key).FirstOrDefault();

            var clsUpdateShape = new clsUpdateShapeFile(_strConnect, EnvironmentAPI.ServerSDE, EnvironmentAPI.PortSDE, EnvironmentAPI.DbSDE, EnvironmentAPI.UserSDE, EnvironmentAPI.PassSDE);

            var xulySDE = clsUpdateShape.QuerySDE("Delete from QUYHOACH_VUNG where MAVUNG='" + info.MAQH + "'");

            dbEF.QUYHOACH.DeleteObject(info);
            dbEF.SaveChanges();
            Log.GhiLogAPI("Xóa quy hoạch" + info.TENVUNG + " ID=" + info.ID + " Table = QUYHOACH", "NHAPLIEU");

            return Request.CreateResponse(HttpStatusCode.Created);
        }
        #endregion

        #region Nhập liệu kế hoạch Thêm/Xóa/Sửa
        [HttpGet]
        public HttpResponseMessage GetKeHoachByID(DataSourceLoadOptions loadOptions, int ID)
        {
            //List<DM_CHUYENMUCDICH> data = db.EGetAll<DM_CHUYENMUCDICH>().Where(n => n.ENABLED == true).ToList();
            var data = dbEF.KEHOACH.Where(n => n.ID_KYQH == ID).Select(n => new { n.ID, n.ID_KYQH, n.ID_MDSD, n.MAKH, n.MAHUYEN, n.MAXA, n.NAM, n.DIENTICH, n.CAPTINH, n.TENVUNG, n.ID_KHUCHUCNANG }).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(data, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage PostKeHoach(FormDataCollection form, int IDKYQH)
        {
            var values = form.Get("values");
            KEHOACH info = new KEHOACH();
            JsonConvert.PopulateObject(values, info);
            info.ID_KYQH = IDKYQH;

            dbEF.KEHOACH.AddObject(info);
            dbEF.SaveChanges();
            Log.GhiLogAPI("Thêm kế hoạch " + info.TENVUNG + " ID=" + info.ID + " Table = KEHOACH", "NHAPLIEU");

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut]
        public HttpResponseMessage PutKeHoach(FormDataCollection form)
        {
            var clsUpdateShape = new clsUpdateShapeFile(_strConnect, EnvironmentAPI.ServerSDE, EnvironmentAPI.PortSDE, EnvironmentAPI.DbSDE, EnvironmentAPI.UserSDE, EnvironmentAPI.PassSDE);

            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            KEHOACH info = dbEF.KEHOACH.Where(n => n.ID == key).FirstOrDefault();

            string MAHTOld = string.Copy(info.MAKH=="null"?"": info.MAKH);
            JsonConvert.PopulateObject(values, info);
            //Kiểm tra mã vùng
            int count = clsUpdateShape.QuerySDE("select MAVUNG from KEHOACH_VUNG where MAVUNG='" + MAHTOld + "'").Rows.Count;
            if(count>0)
            {
                //Cập nhật mã vùng
                var xulySDE = clsUpdateShape.QuerySDE("Update KEHOACH_VUNG set MAVUNG='" + info.MAKH + "' where MAVUNG='" + MAHTOld + "'");
                Log.GhiLogAPI("Cập nhật shape " + info.MAKH + " Table = KEHOACH_VUNG", "SHAPE");

            }

            dbEF.SaveChanges();
            Log.GhiLogAPI("Cập nhật kế hoạch " + info.TENVUNG + " ID=" + info.ID + " Table = KEHOACH", "NHAPLIEU");

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteKeHoach(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            KEHOACH info = dbEF.KEHOACH.Where(n => n.ID == key).FirstOrDefault();

            var clsUpdateShape = new clsUpdateShapeFile(_strConnect, EnvironmentAPI.ServerSDE, EnvironmentAPI.PortSDE, EnvironmentAPI.DbSDE, EnvironmentAPI.UserSDE, EnvironmentAPI.PassSDE);

            var xulySDE = clsUpdateShape.QuerySDE("Delete from KEHOACH_VUNG where MAVUNG='" + info.MAKH + "'");

            dbEF.KEHOACH.DeleteObject(info);
            dbEF.SaveChanges();
            Log.GhiLogAPI("Xóa kế hoạch " + info.TENVUNG + " ID=" + info.ID + " Table = KEHOACH", "NHAPLIEU");

            return Request.CreateResponse(HttpStatusCode.Created);
        }
        #endregion


        #region Nhập liệu DIỆN TÍCH PHÂN BỐ XÁC ĐỊNH Thêm/Xóa/Sửa
        [HttpGet]
        public HttpResponseMessage GetDienTichPBXDByID(DataSourceLoadOptions loadOptions, int ID)
        {
            //List<DM_CHUYENMUCDICH> data = db.EGetAll<DM_CHUYENMUCDICH>().Where(n => n.ENABLED == true).ToList();
            var data = dbEF.PHANBO_XACDINH.Where(n => n.ID_KYQH == ID).Select(n => new { n.ID, n.ID_KYQH, n.ID_MDSD, n.DT_PHANBO, n.DT_XACDINH,  n.CAPTINH, n.ID_KHUCHUCNANG,n.MAHUYEN,n.MAXA }).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(data, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage PostDienTichPBXD(FormDataCollection form, int IDKYQH)
        {
            var values = form.Get("values");
            PHANBO_XACDINH info = new PHANBO_XACDINH();
            JsonConvert.PopulateObject(values, info);
            info.ID_KYQH = IDKYQH;

            dbEF.PHANBO_XACDINH.AddObject(info);
            dbEF.SaveChanges();
            Log.GhiLogAPI("Thêm diện tích phân bổ xác định " +  " ID=" + info.ID + " Table = PHANBO_XACDINH", "NHAPLIEU");

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut]
        public HttpResponseMessage PutDienTichPBXD(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            PHANBO_XACDINH info = dbEF.PHANBO_XACDINH.Where(n => n.ID == key).FirstOrDefault();

 
            JsonConvert.PopulateObject(values, info);

           
            dbEF.SaveChanges();
            Log.GhiLogAPI("Cập nhật diện tích phân bổ xác định " + " ID=" + info.ID + " Table = PHANBO_XACDINH", "NHAPLIEU");

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteDienTichPBXD(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            PHANBO_XACDINH info = dbEF.PHANBO_XACDINH.Where(n => n.ID == key).FirstOrDefault();

 
            dbEF.PHANBO_XACDINH.DeleteObject(info);
            dbEF.SaveChanges();
            Log.GhiLogAPI("Xóa diện tích phân bổ xác định " + " ID=" + info.ID + " Table = PHANBO_XACDINH", "NHAPLIEU");

            return Request.CreateResponse(HttpStatusCode.Created);
        }
        #endregion

        #region Nhập liệu CMD Thêm/Xóa/Sửa
        [HttpGet]
        public HttpResponseMessage GetKHCMDByID(DataSourceLoadOptions loadOptions, int ID)
        {
            //List<DM_CHUYENMUCDICH> data = db.EGetAll<DM_CHUYENMUCDICH>().Where(n => n.ENABLED == true).ToList();
            var data = dbEF.KEHOACH_CMD.Where(n => n.ID_KYQH == ID).Select(n => new { n.ID, n.ID_KYQH, n.ID_CMD, n.DIENTICH, n.CAPTINH, n.MAHUYEN,n.MAXA,n.NAM }).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(data, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage PostKHCMD(FormDataCollection form, int IDKYQH)
        {
            var values = form.Get("values");
            KEHOACH_CMD info = new KEHOACH_CMD();
            JsonConvert.PopulateObject(values, info);
            info.ID_KYQH = IDKYQH;

            dbEF.KEHOACH_CMD.AddObject(info);
            dbEF.SaveChanges();
            Log.GhiLogAPI("Thêm kế hoạch chuyển mục đích " + " ID=" + info.ID + " Table = KEHOACH_CMD", "NHAPLIEU");

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut]
        public HttpResponseMessage PutKHCMD(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            KEHOACH_CMD info = dbEF.KEHOACH_CMD.Where(n => n.ID == key).FirstOrDefault();
            JsonConvert.PopulateObject(values, info);


            dbEF.SaveChanges();
            Log.GhiLogAPI("Cập nhật kế hoạch chuyển mục đích " + " ID=" + info.ID + " Table = KEHOACH_CMD", "NHAPLIEU");

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteKHCMD(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");
            KEHOACH_CMD info = dbEF.KEHOACH_CMD.Where(n => n.ID == key).FirstOrDefault();
            dbEF.KEHOACH_CMD.DeleteObject(info);
            dbEF.SaveChanges();
            Log.GhiLogAPI("Xóa kế hoạch chuyển mục đích " + " ID=" + info.ID + " Table = KEHOACH_CMD", "NHAPLIEU");

            return Request.CreateResponse(HttpStatusCode.Created);
        }
        #endregion

        #region Nhập liệu CSD Thêm/Xóa/Sửa
        [HttpGet]
        public HttpResponseMessage GetKHCSDByID(DataSourceLoadOptions loadOptions, int ID)
        {
            //List<DM_CHUYENMUCDICH> data = db.EGetAll<DM_CHUYENMUCDICH>().Where(n => n.ENABLED == true).ToList();
            var data = dbEF.KEHOACH_CSD.Where(n => n.ID_KYQH == ID).Select(n => new { n.ID, n.ID_KYQH, n.ID_MDSD, n.DIENTICH, n.CAPTINH, n.MAHUYEN, n.MAXA, n.NAM }).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(data, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage PostKHCSD(FormDataCollection form, int IDKYQH)
        {
            var values = form.Get("values");
            KEHOACH_CSD info = new KEHOACH_CSD();
            JsonConvert.PopulateObject(values, info);
            info.ID_KYQH = IDKYQH;

            dbEF.KEHOACH_CSD.AddObject(info);
            dbEF.SaveChanges();
            Log.GhiLogAPI("Thêm kế hoạch chưa sử dụng " + " ID=" + info.ID + " Table = KEHOACH_CSD", "NHAPLIEU");

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut]
        public HttpResponseMessage PutKHCSD(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            KEHOACH_CSD info = dbEF.KEHOACH_CSD.Where(n => n.ID == key).FirstOrDefault();
            JsonConvert.PopulateObject(values, info);

            Log.GhiLogAPI("Cập nhật kế hoạch chưa sử dụng " + " ID=" + info.ID + " Table = KEHOACH_CSD", "NHAPLIEU");

            dbEF.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteKHCSD(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");
            KEHOACH_CSD info = dbEF.KEHOACH_CSD.Where(n => n.ID == key).FirstOrDefault();
            dbEF.KEHOACH_CSD.DeleteObject(info);
            dbEF.SaveChanges();
            Log.GhiLogAPI("Xóa kế hoạch chưa sử dụng " + " ID=" + info.ID + " Table = KEHOACH_CSD", "NHAPLIEU");

            return Request.CreateResponse(HttpStatusCode.Created);
        }
        #endregion

        #region Nhập liệu kế hoạch thu hồi Thêm/Xóa/Sửa
        [HttpGet]
        public HttpResponseMessage GetKHTHByID(DataSourceLoadOptions loadOptions, int ID)
        {
            //List<DM_CHUYENMUCDICH> data = db.EGetAll<DM_CHUYENMUCDICH>().Where(n => n.ENABLED == true).ToList();
            var data = dbEF.KEHOACH_THUHOI.Where(n => n.ID_KYQH == ID).Select(n => new { n.ID, n.ID_KYQH,n.ID_MDSD, n.DIENTICH, n.MAHUYEN, n.MAXA, n.NAM }).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(data, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage PostKHTH(FormDataCollection form, int IDKYQH)
        {
            var values = form.Get("values");
            KEHOACH_THUHOI info = new KEHOACH_THUHOI();
            JsonConvert.PopulateObject(values, info);
            info.ID_KYQH = IDKYQH;

            dbEF.KEHOACH_THUHOI.AddObject(info);
            dbEF.SaveChanges();
            Log.GhiLogAPI("Thêm kế hoạch thu hồi "+ " ID=" + info.ID + " Table = KEHOACH_THUHOI", "NHAPLIEU");

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut]
        public HttpResponseMessage PutKHTH(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            KEHOACH_THUHOI info = dbEF.KEHOACH_THUHOI.Where(n => n.ID == key).FirstOrDefault();
            JsonConvert.PopulateObject(values, info);


            dbEF.SaveChanges();
            Log.GhiLogAPI("Cập nhật kế hoạch thu hồi " + " ID=" + info.ID + " Table = KEHOACH_THUHOI", "NHAPLIEU");

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteKHTH(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");
            KEHOACH_THUHOI info = dbEF.KEHOACH_THUHOI.Where(n => n.ID == key).FirstOrDefault();
            dbEF.KEHOACH_THUHOI.DeleteObject(info);
            dbEF.SaveChanges();
            Log.GhiLogAPI("Xóa kế hoạch thu hồi " + " ID=" + info.ID + " Table = KEHOACH_THUHOI", "NHAPLIEU");

            return Request.CreateResponse(HttpStatusCode.Created);
        }
        #endregion

        #region Nhập liệu KCN_MDSD Thêm/Xóa/Sửa
        [HttpGet]
        public HttpResponseMessage GetKCN_MDSDByID(DataSourceLoadOptions loadOptions, int ID)
        {
            //List<DM_CHUYENMUCDICH> data = db.EGetAll<DM_CHUYENMUCDICH>().Where(n => n.ENABLED == true).ToList();
            var data = dbEF.KHUCHUCNANG_MDSD.Where(n => n.ID_KHUCN == ID).Select(n => new { n.ID, n.ID_MDSD, n.DIENTICH, n.ID_KHUCN }).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(data, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage PostKCN_MDSD(FormDataCollection form, int ID_KHUCN)
        {
            var values = form.Get("values");
            KHUCHUCNANG_MDSD info = new KHUCHUCNANG_MDSD();
            JsonConvert.PopulateObject(values, info);
            info.ID_KHUCN = ID_KHUCN;

            dbEF.KHUCHUCNANG_MDSD.AddObject(info);
            dbEF.SaveChanges();
            Log.GhiLogAPI("Thêm khu chức năng mục đích sử dụng " + " ID=" + info.ID + " Table = KHUCHUCNANG_MDSD", "NHAPLIEU");

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut]
        public HttpResponseMessage PutKCN_MDSD(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");
            KHUCHUCNANG_MDSD info = dbEF.KHUCHUCNANG_MDSD.Where(n => n.ID == key).FirstOrDefault();
            JsonConvert.PopulateObject(values, info);


            dbEF.SaveChanges();
            Log.GhiLogAPI("Cập nhật khu chức năng mục đích sử dụng " + " ID=" + info.ID + " Table = KHUCHUCNANG_MDSD", "NHAPLIEU");

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteKHUCHUCNANG_MDSD(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");
            KHUCHUCNANG_MDSD info = dbEF.KHUCHUCNANG_MDSD.Where(n => n.ID == key).FirstOrDefault();
            dbEF.KHUCHUCNANG_MDSD.DeleteObject(info);
            dbEF.SaveChanges();
            Log.GhiLogAPI("Xóa khu chức năng mục đích sử dụng " + " ID=" + info.ID + " Table = KHUCHUCNANG_MDSD", "NHAPLIEU");

            return Request.CreateResponse(HttpStatusCode.Created);
        }
        #endregion

        #region Nhập liệu Công trình Thêm/Xóa/Sửa
        [HttpGet]
        public HttpResponseMessage GetCongTrinhByID(DataSourceLoadOptions loadOptions, int ID)
        {
            //List<DM_CHUYENMUCDICH> data = db.EGetAll<DM_CHUYENMUCDICH>().Where(n => n.ENABLED == true).ToList();
            var data = dbEF.CONGTRINHDUAN.Where(n => n.ID_KYQH == ID).Select(n => new { n.ID, n.ID_KYQH, n.DIENTICH_TANGTHEM, n.MACT, n.MAHUYEN, n.MAXA, n.NAMTH, n.DIENTICH_KH,n.DIENTICH_HT, n.CAPTINH, n.TENCONGTRINH,n.TENDIADIEM, n.ID_LOAICT,n.VITRITRENBD,n.SUDUNGVAOLD }).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(data, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage PostCongTrinh(FormDataCollection form, int IDKYQH)
        {
            var values = form.Get("values");
            CONGTRINHDUAN info = new CONGTRINHDUAN();
            JsonConvert.PopulateObject(values, info);
            info.ID_KYQH = IDKYQH;

            dbEF.CONGTRINHDUAN.AddObject(info);
            dbEF.SaveChanges();
            Log.GhiLogAPI("Thêm công trình " +info.TENCONGTRINH+ " ID=" + info.ID + " Table = CONGTRINHDUAN", "NHAPLIEU");

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut]
        public HttpResponseMessage PutCongTrinh(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            CONGTRINHDUAN info = dbEF.CONGTRINHDUAN.Where(n => n.ID == key).FirstOrDefault();

            string MAHTOld = string.Copy(info.MACT);
            JsonConvert.PopulateObject(values, info);

            //Cập nhật mã vùng
            var clsUpdateShape = new clsUpdateShapeFile(_strConnect, EnvironmentAPI.ServerSDE, EnvironmentAPI.PortSDE, EnvironmentAPI.DbSDE, EnvironmentAPI.UserSDE, EnvironmentAPI.PassSDE);
            int count = clsUpdateShape.QuerySDE("select MAVUNG from CONGTRINH_VUNG where MAVUNG='" + MAHTOld + "'").Rows.Count;
            if (count > 0)
            {
                var xulySDE = clsUpdateShape.QuerySDE("Update CONGTRINH_VUNG set MAVUNG='" + info.MACT + "' where MAVUNG='" + MAHTOld + "'");
                Log.GhiLogAPI("Cập nhật shape " + info.MACT + " Table = CONGTRINH_VUNG", "SHAPE");

            }
            dbEF.SaveChanges();
            Log.GhiLogAPI("Cập nhật công trình " + info.TENCONGTRINH + " ID=" + info.ID + " Table = CONGTRINHDUAN", "NHAPLIEU");

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteCongTrinh(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            CONGTRINHDUAN info = dbEF.CONGTRINHDUAN.Where(n => n.ID == key).FirstOrDefault();

            var clsUpdateShape = new clsUpdateShapeFile(_strConnect, EnvironmentAPI.ServerSDE, EnvironmentAPI.PortSDE, EnvironmentAPI.DbSDE, EnvironmentAPI.UserSDE, EnvironmentAPI.PassSDE);

            var xulySDE = clsUpdateShape.QuerySDE("Delete from CONGTRINH_VUNG where MAVUNG='" + info.MACT + "'");

            dbEF.CONGTRINHDUAN.DeleteObject(info);
            dbEF.SaveChanges();
            Log.GhiLogAPI("Xóa công trình " + info.TENCONGTRINH + " ID=" + info.ID + " Table = CONGTRINHDUAN", "NHAPLIEU");
            return Request.CreateResponse(HttpStatusCode.Created);
        }
        #endregion


        #region Upload Shape

        [HttpGet]
        public bool CheckShape(int TYPE = 1,String MA="")
        {
            
            if(TYPE==2)
            {

            }
            var clsUpdateShape = new clsUpdateShapeFile(_strConnect, EnvironmentAPI.ServerSDE, EnvironmentAPI.PortSDE, EnvironmentAPI.DbSDE, EnvironmentAPI.UserSDE, EnvironmentAPI.PassSDE);
            int count = clsUpdateShape.QuerySDE("select MAVUNG from "+PageDB.data.Where(n=>n.TYPE==TYPE).FirstOrDefault().TableSDE+" where MAVUNG='" + MA + "'").Rows.Count;
            if (count > 0)
            {
                return true;
            }
            return false;

        }


        [HttpPost]
        public object Upload(int TYPE = 1)
        {

            // var clsUpdateShape= new clsUpdateShapeFile(_strConnect, "192.169.3.13","5151", "reis", "sde", "sde");
            // var d = clsUpdateShape.AddShapeFile("H:\\WEBDOTHIBIENHOA\\shap\\1503.shp", "SDE.HienTrang_vung", "","");
            HIENTRANG doc = new HIENTRANG();
            KHUCHUCNANG KCN = new KHUCHUCNANG();
            QUYHOACH QH = new QUYHOACH();
            KEHOACH KH = new KEHOACH();
            CONGTRINHDUAN CT = new CONGTRINHDUAN();

            String MALKSDE = "";
            if (TYPE == 1)
            {
                var CAPTINH = HttpContext.Current.Request.Form["CAPTINH"];
                var MAHUYEN = HttpContext.Current.Request.Form["MAHUYEN"];
                var MAXA = HttpContext.Current.Request.Form["MAXA"];
                var IDKYQH = HttpContext.Current.Request.Form["IDKYQH"];
                var TENVUNG = HttpContext.Current.Request.Form["TENVUNG"];
                var MAHT = HttpContext.Current.Request.Form["MAHT"];


                var NAM = HttpContext.Current.Request.Form["NAM"];
                var DIENTICH = HttpContext.Current.Request.Form["DIENTICH"];
                var ID_MDSD = HttpContext.Current.Request.Form["ID_MDSD"];
                doc.DIENTICH = Convert.ToDecimal(DIENTICH == "" ? "0" : DIENTICH);
                doc.ID_MDSD = Convert.ToInt32(ID_MDSD == "null" ? "0" : ID_MDSD);
                doc.NAM = Convert.ToInt16(NAM == "" ? "0" : NAM);


                doc.CAPTINH = Convert.ToBoolean(CAPTINH ?? "false");
                doc.MAHUYEN = Convert.ToString(MAHUYEN ?? "0");
                doc.MAXA = Convert.ToString(MAXA ?? "0");
                doc.ID_KYQH = Convert.ToInt32(IDKYQH ?? "0");
                doc.TENVUNG = Convert.ToString(TENVUNG ?? "");

                doc.MAHT = Convert.ToString(MAHT ?? "");
                MALKSDE = doc.MAHT;
                using (var dbQH = new KHQHEntities())
                {
                    dbQH.HIENTRANG.AddObject(doc);
                    dbQH.SaveChanges();
                    Log.GhiLogAPI("Thêm shape " + doc.MAHT + " Table = HIENTRANG_VUNG", "SHAPE");

                }

            }
            if (TYPE == 3)
            {
                var CAPTINH = HttpContext.Current.Request.Form["CAPTINH"];
                var MAHUYEN = HttpContext.Current.Request.Form["MAHUYEN"];
                var MAXA = HttpContext.Current.Request.Form["MAXA"];
                var IDKYQH = HttpContext.Current.Request.Form["IDKYQH"];
                var TENVUNG = HttpContext.Current.Request.Form["TENVUNG"];
                var MAHT = HttpContext.Current.Request.Form["MAHT"];
                var MALOAIKHUCN = HttpContext.Current.Request.Form["IDLoaiKCN"];

                KCN.CAPTINH = Convert.ToBoolean(CAPTINH ?? "false");
                KCN.MAHUYEN = Convert.ToString(MAHUYEN ?? "0");
                KCN.MAXA = Convert.ToString(MAXA ?? "0");
                KCN.ID_KYQH = Convert.ToInt32(IDKYQH ?? "0");
                KCN.TENVUNG = Convert.ToString(TENVUNG ?? "");
                KCN.MAKHUCN = Convert.ToString(MAHT ?? "");
                KCN.MALOAIKHUCN = Convert.ToInt32(MALOAIKHUCN ?? "");
                MALKSDE = KCN.MAKHUCN;
                using (var dbQH = new KHQHEntities())
                {
                    dbQH.KHUCHUCNANG.AddObject(KCN);
                    dbQH.SaveChanges();
                    Log.GhiLogAPI("Thêm shape " + KCN.MAKHUCN + " Table = KHUCHUCNANG_VUNG", "SHAPE");

                }

            }


            if (TYPE == 4)
            {
                var CAPTINH = HttpContext.Current.Request.Form["CAPTINH"];
                var MAHUYEN = HttpContext.Current.Request.Form["MAHUYEN"];
                var MAXA = HttpContext.Current.Request.Form["MAXA"];
                var IDKYQH = HttpContext.Current.Request.Form["IDKYQH"];
                var TENVUNG = HttpContext.Current.Request.Form["TENVUNG"];
                var MAHT = HttpContext.Current.Request.Form["MAHT"];
                var NAM = HttpContext.Current.Request.Form["NAM"];
                var DIENTICH = HttpContext.Current.Request.Form["DIENTICH"];
                var ID_MDSD = HttpContext.Current.Request.Form["ID_MDSD"];
                var MALOAIKHUCN = HttpContext.Current.Request.Form["IDLoaiKCN"];


                QH.DIENTICH = Convert.ToDecimal(DIENTICH == "" ? "0" : DIENTICH);
                QH.ID_MDSD = Convert.ToInt32(ID_MDSD == "null" ? "0" : ID_MDSD);
                QH.NAM = Convert.ToInt16(NAM == "" ? "0" : NAM);
                QH.CAPTINH = Convert.ToBoolean(CAPTINH ?? "false");
                QH.MAHUYEN = Convert.ToString(MAHUYEN ?? "0");
                QH.MAXA = Convert.ToString(MAXA ?? "0");
                QH.ID_KYQH = Convert.ToInt32(IDKYQH ?? "0");
                QH.TENVUNG = Convert.ToString(TENVUNG ?? "");
                QH.MAQH = Convert.ToString(MAHT ?? "");
                QH.ID_KHUCN = Convert.ToInt32(MALOAIKHUCN ?? "");
                MALKSDE = QH.MAQH;

                using (var dbQH = new KHQHEntities())
                {
                    dbQH.QUYHOACH.AddObject(QH);
                    dbQH.SaveChanges();
                    Log.GhiLogAPI("Thêm shape " + QH.MAQH + " Table = QUYHOACH_VUNG", "SHAPE");

                }
            }

            if (TYPE == 2)
            {
                var CAPTINH = HttpContext.Current.Request.Form["CAPTINH"];
                var MAHUYEN = HttpContext.Current.Request.Form["MAHUYEN"];
                var MAXA = HttpContext.Current.Request.Form["MAXA"];
                var IDKYQH = HttpContext.Current.Request.Form["IDKYQH"];
                var TENVUNG = HttpContext.Current.Request.Form["TENVUNG"];
                var MAHT = HttpContext.Current.Request.Form["MAHT"];
                var NAM = HttpContext.Current.Request.Form["NAM"];
                var DIENTICH = HttpContext.Current.Request.Form["DIENTICH"];
                var ID_MDSD = HttpContext.Current.Request.Form["ID_MDSD"];
                var MALOAIKHUCN = HttpContext.Current.Request.Form["IDLoaiKCN"];


                KH.DIENTICH = Convert.ToDecimal(DIENTICH == "" ? "0" : DIENTICH);
                KH.ID_MDSD = Convert.ToInt32(ID_MDSD == "null" ? "0" : ID_MDSD);
                KH.NAM = Convert.ToInt16(NAM == "" ? "0" : NAM);
                KH.CAPTINH = Convert.ToBoolean(CAPTINH ?? "false");
                KH.MAHUYEN = Convert.ToString(MAHUYEN ?? "0");
                KH.MAXA = Convert.ToString(MAXA == "null" ? "26509":MAXA);
                KH.ID_KYQH = Convert.ToInt32(IDKYQH ?? "0");
                KH.TENVUNG = Convert.ToString(TENVUNG ?? "");
                KH.MAKH = Convert.ToString(MAHT ?? "");
                KH.ID_KHUCHUCNANG = Convert.ToInt32(MALOAIKHUCN ?? "");
                MALKSDE = KH.MAKH;

                using (var dbQH = new KHQHEntities())
                {
                    dbQH.KEHOACH.AddObject(KH);
                    dbQH.SaveChanges();
                    Log.GhiLogAPI("Thêm shape " + KH.MAKH + " Table = KEHOACH_VUNG", "SHAPE");

                }
            }

            if (TYPE == 5)
            {
                var CAPTINH = HttpContext.Current.Request.Form["CAPTINH"];
                var MAHUYEN = HttpContext.Current.Request.Form["MAHUYEN"];
                var MAXA = HttpContext.Current.Request.Form["MAXA"];
                var IDKYQH = HttpContext.Current.Request.Form["IDKYQH"];
                var TENCONGTRINH = HttpContext.Current.Request.Form["TENVUNG"];
                var MACT = HttpContext.Current.Request.Form["MAHT"];
                var NAMTH = HttpContext.Current.Request.Form["NAM"];
                var DIENTICH_TANGTHEM = HttpContext.Current.Request.Form["DIENTICH"];
                var DIENTICH_KH = HttpContext.Current.Request.Form["DIENTICH_KH"];
                var DIENTICH_HT = HttpContext.Current.Request.Form["DIENTICH_HT"];
                var SUDUNGVAOLD = HttpContext.Current.Request.Form["SUDUNGVAOLD"];
                var TENDIADIEM = HttpContext.Current.Request.Form["TENDIADIEM"];
                var VITRITRENBD = HttpContext.Current.Request.Form["VITRITRENBD"];
                var ID_LOAICT = HttpContext.Current.Request.Form["ID_LOAICT"];





                CT.DIENTICH_TANGTHEM = Convert.ToDecimal(DIENTICH_TANGTHEM == "" ? "0" : DIENTICH_TANGTHEM);
                CT.DIENTICH_HT = Convert.ToDecimal(DIENTICH_HT == "" ? "0" : DIENTICH_HT);
                CT.DIENTICH_KH = Convert.ToDecimal(DIENTICH_KH == "" ? "0" : DIENTICH_KH);
                CT.ID_LOAICT = Convert.ToInt32(ID_LOAICT == "null" ? "0" : ID_LOAICT);
                CT.NAMTH = Convert.ToInt16(NAMTH == "" ? "0" : NAMTH);
                CT.CAPTINH = Convert.ToBoolean(CAPTINH ?? "false");
                CT.MAHUYEN = Convert.ToString(MAHUYEN ?? "0");
                CT.MAXA = Convert.ToString(MAXA ?? "0");
                CT.ID_KYQH = Convert.ToInt32(IDKYQH ?? "0");
                CT.TENCONGTRINH = Convert.ToString(TENCONGTRINH ?? "");
                CT.SUDUNGVAOLD = Convert.ToString(SUDUNGVAOLD ?? "");
                CT.TENDIADIEM = Convert.ToString(TENDIADIEM ?? "");
                CT.VITRITRENBD = Convert.ToString(VITRITRENBD ?? "");
                CT.MACT = Convert.ToString(MACT ?? "");

                MALKSDE = CT.MACT;
                using (var dbQH = new KHQHEntities())
                {
                    dbQH.CONGTRINHDUAN.AddObject(CT);
                    dbQH.SaveChanges();
                    Log.GhiLogAPI("Thêm shape " + CT.MACT + " Table = CONGTRINH_VUNG", "SHAPE");

                }
            }


            var shapfilename = "";

            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                if (TYPE == 1 || TYPE == 4||TYPE==2 || TYPE == 3 || TYPE == 5)
                {


                    string PathFile = uploadRootFolderInput;
                    string PathFileConvert = PathFile + "\\" + Guid.NewGuid();
                    Directory.CreateDirectory(PathFileConvert);
                    for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
                    {
                        var httpPostedFile = HttpContext.Current.Request.Files[i];
                        if (httpPostedFile != null)
                        {
                            // Validate the uploaded image(optional)

                            // Get the complete file path

                            string newFileName = Guid.NewGuid() + Path.GetExtension(httpPostedFile.FileName);
                            string fname = Path.Combine(PathFileConvert, httpPostedFile.FileName);
                            if (Path.GetExtension(httpPostedFile.FileName) == ".shp")
                            {
                                shapfilename = fname;
                            }
                            httpPostedFile.SaveAs(fname);
                            // Save the uploaded file to "UploadedFiles" folder
                        }

                    }
                    var clsUpdateShape = new clsUpdateShapeFile(_strConnect, EnvironmentAPI.ServerSDE, EnvironmentAPI.PortSDE, EnvironmentAPI.DbSDE, EnvironmentAPI.UserSDE, EnvironmentAPI.PassSDE);
                    


                    var d = clsUpdateShape.AddShapeFile(shapfilename, PageDB.data.Where(n => n.TYPE == TYPE).FirstOrDefault().TableSDE, "", MALKSDE);
                    //Get the uploaded image from the Files collection
                }

            }
            return true;



        }

        #endregion
    } 
}
