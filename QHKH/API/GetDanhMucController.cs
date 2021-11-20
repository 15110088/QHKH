using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using KEHOACHQH.DAL;
using KHQH.Models.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace KHQH.API
{
    [Route("api/GetDanhMuc/{action}", Name = "GetDanhMuc")]

    public class GetDanhMucController : ApiController
    {

        private KHQH.DBFactory.DBInteractive db = new KHQH.DBFactory.DBInteractive();

        private KHQHEntities dbEF = new KHQHEntities();



        #region Mục Đích Sử dụng Thêm/Xóa/Sửa
        [HttpGet]
        public HttpResponseMessage MucDichSuDung(DataSourceLoadOptions loadOptions)
        {
            var data = dbEF.DM_MUCDICHSUDUNG.Where(n => n.ENABLED == true)
                .Select(n=>new {n.ID,n.ID_CAPTREN,n.KIHIEU,n.NGAYTAO,n.CHIMUC,n.TEN,n.TT01,n.TT23,n.TT24,n.TT25,n.STT} ).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(data, loadOptions));


        }

        [HttpPost]
        public HttpResponseMessage PostMDSD(FormDataCollection form)
        {
            var values = form.Get("values");
            DM_MUCDICHSUDUNG info = new DM_MUCDICHSUDUNG();
            JsonConvert.PopulateObject(values, info);
            info.NGAYTAO = DateTime.Now;
            info.ENABLED = true;

            dbEF.DM_MUCDICHSUDUNG.AddObject(info);
            dbEF.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut]
        public HttpResponseMessage PutMDSD(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            DM_MUCDICHSUDUNG info = dbEF.DM_MUCDICHSUDUNG.Where(n => n.ID == key).FirstOrDefault();
            JsonConvert.PopulateObject(values, info);
            dbEF.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteMDSD(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            DM_MUCDICHSUDUNG info = dbEF.DM_MUCDICHSUDUNG.Where(n => n.ID == key).FirstOrDefault();
            info.ENABLED = false;

            dbEF.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        #endregion


        #region Chuyển Mục Đích Thêm/Xóa/Sửa
        [HttpGet]
        public HttpResponseMessage ChuyenMucDich(DataSourceLoadOptions loadOptions)
        {
            //List<DM_CHUYENMUCDICH> data = db.EGetAll<DM_CHUYENMUCDICH>().Where(n => n.ENABLED == true).ToList();
            var data = dbEF.DM_CHUYENMUCDICH.Where(n => n.ENABLED == true).Select(n=>new {n.ID,n.ID_CAPTREN,n.KYHIEU,n.TEN,n.CHIMUC,n.CAPTINH,n.CHIMUCTINH,n.ENABLED }).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(data, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage PostCMD(FormDataCollection form)
        {
            var values = form.Get("values");
            DM_CHUYENMUCDICH info = new DM_CHUYENMUCDICH();
            JsonConvert.PopulateObject(values, info);
            info.ENABLED = true;

            dbEF.DM_CHUYENMUCDICH.AddObject(info);
            dbEF.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut]
        public HttpResponseMessage PutCMD(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            DM_CHUYENMUCDICH info = dbEF.DM_CHUYENMUCDICH.Where(n => n.ID == key).FirstOrDefault();
            JsonConvert.PopulateObject(values, info);
            dbEF.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteCMD(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            DM_CHUYENMUCDICH info = dbEF.DM_CHUYENMUCDICH.Where(n => n.ID == key).FirstOrDefault();
            info.ENABLED = false;
            dbEF.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        #endregion


        #region Khu Chức Năng Thêm/Xóa/Sửa
        [HttpGet]
        public HttpResponseMessage KhuChucNang(DataSourceLoadOptions loadOptions)
        {
            //List<DM_CHUYENMUCDICH> data = db.EGetAll<DM_CHUYENMUCDICH>().Where(n => n.ENABLED == true).ToList();
            var data = dbEF.DM_LOAIKHUCHUCNANG.Where(n => n.ENABLED == true).Select(n=>new { n.CAPTINH,n.ID,n.TEN,n.KYHIEU}).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(data, loadOptions));
        }

        public HttpResponseMessage KhuChucNangMDSD(DataSourceLoadOptions loadOptions)
        {
            //List<DM_CHUYENMUCDICH> data = db.EGetAll<DM_CHUYENMUCDICH>().Where(n => n.ENABLED == true).ToList();
            var data = dbEF.KHUCHUCNANGs.Select(n => new { n.CAPTINH, n.ID, n.MAHUYEN, n.MAXA,n.MAKHUCN,n.MALOAIKHUCN,n.ID_KYQH,n.TENVUNG }).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(data, loadOptions));
        }

        public HttpResponseMessage KhuChucNangCH(DataSourceLoadOptions loadOptions)
        {
            //List<DM_CHUYENMUCDICH> data = db.EGetAll<DM_CHUYENMUCDICH>().Where(n => n.ENABLED == true).ToList();
            var data = dbEF.KHUCHUCNANGs.Where(n => n.CAPTINH == false ).Select(n => new { n.CAPTINH, n.ID, n.MAHUYEN, n.MAXA, n.MAKHUCN, n.MALOAIKHUCN, n.ID_KYQH, n.TENVUNG }).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(data, loadOptions));
        }


        [HttpPost]
        public HttpResponseMessage PostKCN(FormDataCollection form)
        {
            var values = form.Get("values");
            DM_LOAIKHUCHUCNANG info = new DM_LOAIKHUCHUCNANG();
            JsonConvert.PopulateObject(values, info);
            info.ENABLED = true;

            dbEF.DM_LOAIKHUCHUCNANG.AddObject(info);
            dbEF.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut]
        public HttpResponseMessage PutKCN(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            DM_LOAIKHUCHUCNANG info = dbEF.DM_LOAIKHUCHUCNANG.Where(n => n.ID == key).FirstOrDefault();
            JsonConvert.PopulateObject(values, info);
            dbEF.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteKCN(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            DM_LOAIKHUCHUCNANG info = dbEF.DM_LOAIKHUCHUCNANG.Where(n => n.ID == key).FirstOrDefault();
            info.ENABLED = false;
            dbEF.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        #endregion


        #region Công Trình Thêm/Xóa/Sửa
        [HttpGet]
        public HttpResponseMessage CongTrinh(DataSourceLoadOptions loadOptions)
        {
            //List<DM_CHUYENMUCDICH> data = db.EGetAll<DM_CHUYENMUCDICH>().Where(n => n.ENABLED == true).ToList();
            var data = dbEF.DM_LOAICONGTRINH.Select(n=>new { n.CAPTINH,n.ID,n.ID_CAPTREN,n.CHIMUC,n.LOAIHANGMUC}).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(data, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage PostCT(FormDataCollection form)
        {
            var values = form.Get("values");
            DM_LOAICONGTRINH info = new DM_LOAICONGTRINH();
            JsonConvert.PopulateObject(values, info);

            dbEF.DM_LOAICONGTRINH.AddObject(info);
            dbEF.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut]
        public HttpResponseMessage PutCT(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            DM_LOAICONGTRINH info = dbEF.DM_LOAICONGTRINH.Where(n => n.ID == key).FirstOrDefault();
            JsonConvert.PopulateObject(values, info);
            dbEF.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteCT(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            DM_LOAICONGTRINH info = dbEF.DM_LOAICONGTRINH.Where(n => n.ID == key).FirstOrDefault();
           // info.ENABLED = false;
            dbEF.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        #endregion

        #region KYQH Thêm/Xóa/Sửa
        [HttpGet]
        public HttpResponseMessage KYQH(DataSourceLoadOptions loadOptions)
        {
            //List<DM_CHUYENMUCDICH> data = db.EGetAll<DM_CHUYENMUCDICH>().Where(n => n.ENABLED == true).ToList();
            var data = dbEF.KYQUYHOACHKEHOACHes.Where(n => n.BIKHOA == false).Select(n=>new { n.ID,n.ID_CHA,n.IS_KEHOACH_DIEUCHINH,n.NAM,n.TUNAM,n.TOINAM,n.TEN}).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(data, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage KeHoachDieuChinh(DataSourceLoadOptions loadOptions)
        {


            List<DM_KeHoachDieuChinh> data = new List<DM_KeHoachDieuChinh>();
            data.Add(new DM_KeHoachDieuChinh(1, "Quy Hoạch"));
            data.Add(new DM_KeHoachDieuChinh(2, "Kế Hoạch"));
            data.Add(new DM_KeHoachDieuChinh(3, "QH Điều Chỉnh"));

            return Request.CreateResponse(DataSourceLoader.Load(data, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage PostKYQH(FormDataCollection form)
        {
            var values = form.Get("values");
            
            KYQUYHOACHKEHOACH info = new KYQUYHOACHKEHOACH();
            JsonConvert.PopulateObject(values, info);
            info.BIKHOA = false;

            if(info.IS_KEHOACH_DIEUCHINH!=null)
            {
                var checkNam = dbEF.KYQUYHOACHKEHOACHes.Where(n => n.TUNAM == info.TUNAM && n.TOINAM == info.TOINAM && n.IS_KEHOACH_DIEUCHINH==info.IS_KEHOACH_DIEUCHINH).Count();
                if(checkNam>0)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Kỳ quy hoạch đã tồn tại");

                }

            }
            dbEF.KYQUYHOACHKEHOACHes.AddObject(info);
            dbEF.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut]
        public HttpResponseMessage PutKYQH(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            KYQUYHOACHKEHOACH info = dbEF.KYQUYHOACHKEHOACHes.Where(n => n.ID == key).FirstOrDefault();
            JsonConvert.PopulateObject(values, info);
            info.BIKHOA = false;

            dbEF.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteKYQH(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            KYQUYHOACHKEHOACH info = dbEF.KYQUYHOACHKEHOACHes.Where(n => n.ID == key).FirstOrDefault();
           info.BIKHOA = true;
            dbEF.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        #endregion


        #region Khu Vuc Hanh Chinh Thêm/Xóa/Sửa
        [HttpGet]
        public HttpResponseMessage KVHC(DataSourceLoadOptions loadOptions)
        {
            var data = dbEF.DM_KVHC.Where(n => n.DELETED == null || n.DELETED == false).Select(n => new {n.MA_KVHC_CHA,n.ID,n.ID_CAP_KVHC,n.TEN_KVHC,n.MA_KVHC}).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(data, loadOptions));


        }
        [HttpGet]
        public HttpResponseMessage KVHCByIDHUYEN(DataSourceLoadOptions loadOptions,int ID=0)
        {
            string IDCha = Convert.ToString(ID);
            var data = dbEF.DM_KVHC.Where(n => n.MA_KVHC_CHA== IDCha && (n.DELETED == null || n.DELETED == false)).Select(n=>new { n.ID,n.ID_CAP_KVHC,n.TEN_KVHC,n.MA_KVHC_CHA,n.MA_KVHC,n.DIACHI}).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(data, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage PostKVHC(FormDataCollection form)
        {
            var values = form.Get("values");
            DM_KVHC info = new DM_KVHC();
            JsonConvert.PopulateObject(values, info);
            info.DELETED = false;
            dbEF.DM_KVHC.AddObject(info);
            dbEF.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut]
        public HttpResponseMessage PutKVHC(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            DM_KVHC info = dbEF.DM_KVHC.Where(n => n.ID == key).FirstOrDefault();
            JsonConvert.PopulateObject(values, info);
            dbEF.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteKVHC(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var values = form.Get("values");

            DM_KVHC info = dbEF.DM_KVHC.Where(n => n.ID == key).FirstOrDefault();
            info.DELETED = true;

            dbEF.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        #endregion

    }
}
