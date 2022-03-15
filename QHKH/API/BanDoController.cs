using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using KHQH.Common;
using KHQH.Models.DB;
using KHQH.Models.Helpper;
using QHKH.Controllers;
using QHKH.Interface;
using QHKH.Models;
using QHKH.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QHKH.API
{
    [Route("api/BanDo/{action}", Name = "BanDo")]
    public class BanDoController : ApiController
    {

        IBase Log = new BaseController();

        private KHQHEntities dbEF = new KHQHEntities();

        [HttpGet]
        public DataAPI<DuLieuBanDo> GetData(int TYPE,string MAVUNG)
        {
            var DanhMucMDSD = dbEF.DM_MUCDICHSUDUNG.Select(n => new { n.KIHIEU, n.ID });

            DuLieuBanDo data = new DuLieuBanDo();
            if (TYPE==1)
            {
                var loaddb = dbEF.HIENTRANGs.FirstOrDefault(n => n.MAHT == MAVUNG);
                data.DIENTICH = Convert.ToDecimal(loaddb.DIENTICH);
                data.TEN = loaddb.TENVUNG;
                data.LOAIDAT = DanhMucMDSD.FirstOrDefault(n=>n.ID==loaddb.ID_MDSD).KIHIEU;

            }
            if (TYPE == 2)
            {
                var loaddb = dbEF.KEHOACHes.FirstOrDefault(n => n.MAKH == MAVUNG);
                data.DIENTICH = Convert.ToDecimal(loaddb.DIENTICH);
                data.TEN = loaddb.TENVUNG;
                data.LOAIDAT = DanhMucMDSD.FirstOrDefault(n => n.ID == loaddb.ID_MDSD).KIHIEU;

            }

            if (TYPE == 3)
            {
                var loaddb = dbEF.QUYHOACHes.FirstOrDefault(n => n.MAQH == MAVUNG);
                data.DIENTICH = Convert.ToDecimal(loaddb.DIENTICH);
                data.TEN = loaddb.TENVUNG;
                data.LOAIDAT = DanhMucMDSD.FirstOrDefault(n => n.ID == loaddb.ID_MDSD).KIHIEU;

            }
            if (TYPE == 4)
            {
                var loaddb = dbEF.KHUCHUCNANGs.FirstOrDefault(n => n.MAKHUCN == MAVUNG);
              //  data.DIENTICH = Convert.ToDecimal(loaddb.);
                data.TEN = loaddb.TENVUNG;
                //data.LOAIDAT = DanhMucMDSD.FirstOrDefault(n => n.ID == loaddb.ID_MDSD).KIHIEU;

            }

            if (TYPE == 5)
            {
                var loaddb = dbEF.CONGTRINHDUANs.FirstOrDefault(n => n.MACT == MAVUNG);
                //data.DIENTICH = Convert.ToDecimal(loaddb.);
                data.TEN = loaddb.TENCONGTRINH;
               // data.LOAIDAT = DanhMucMDSD.FirstOrDefault(n => n.ID == loaddb.).KIHIEU;

            }
            return ResultAPI<DuLieuBanDo>.DATA(data, null,true,"thành công",200);

        }
    }
}
