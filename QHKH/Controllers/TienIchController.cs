using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Tasks.Query;
using KHQH.Models.DB;
using KHQH.Models.JSONDATA;
using QHKH.Controllers;
using QHKH.DBFactory;
using QHKH.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
            List<DM_KVHC> dataKVHC = dbEF.DM_KVHC.Where(n => (n.DELETED == null || n.DELETED == false)).ToList();

            CombineHienTrang cb = new CombineHienTrang();
            cb.LstPage = PageDB.data.Where(n => n.TYPE > 30&&n.TYPE<=43).ToList();
            cb.LstHuyen = dataKVHC.Where(n => n.ID_CAP_KVHC == 2).ToList();
            cb.LstXa = dataKVHC.Where(n => n.ID_CAP_KVHC == 1).ToList();
            return View(cb);
        }


        [HttpGet]
        public async Task<object> TongHopChuChuyen()
        {
            DBInteractiveSSMS dbssms = new DBInteractiveSSMS();

            try
            {
                dbssms.GetSQL("delete from QUYHOACH2021.TH_CHUCHUYENDATDAI");

           

            var DanhMucMDSD = dbEF.DM_MUCDICHSUDUNG.Select(n => new { n.KIHIEU, n.ID });
            var urlmap = dbEF.MAP_CONFIG.Where(n => n.MAKVHC == MaXa).FirstOrDefault().MAP_SERVICES;
             
            List<TH_CHUCHUYENDATDAI> lstChuChuyen = new List<TH_CHUCHUYENDATDAI>();
            var queryRanhThua = new QueryTask(new Uri(urlmap+"/12"));
            // queryRanhThua.Token = GetToken(td.MaHuyen);
            var whereRanhThua = new Query(String.Format("1=1"));
            whereRanhThua.OutFields = OutFields.All;


            int Index = 1;
            var resultQH = await queryRanhThua.ExecuteAsync(whereRanhThua);
            var result = resultQH.FeatureSet.Features;
            var queryQuyHoach = new QueryTask(new Uri(urlmap+"/17"));

            foreach (var item in result)
            {
                var queryParamsQH = new Query(item.Geometry.Extent);
                queryParamsQH.Geometry = item.Geometry;
                queryParamsQH.SpatialRelationship = SpatialRelationship.Intersects;
                queryParamsQH.OutSpatialReference = resultQH.FeatureSet.SpatialReference;
                queryParamsQH.ReturnGeometry = true;
                queryParamsQH.OutFields = OutFields.All;
                QueryResult queryResultQH = null;
                queryResultQH = await queryQuyHoach.ExecuteAsync(queryParamsQH);
                if (queryResultQH != null)
                {

                    if (queryResultQH != null)
                    {
                        var resultFeaturesQH = queryResultQH.FeatureSet.Features;
                        foreach (var feature in resultFeaturesQH)
                        {
                            var geo2 = feature.Geometry;
                            TH_CHUCHUYENDATDAI th = new TH_CHUCHUYENDATDAI();
                            try
                            {
                                var dtGiao = GeometryEngine.Area(GeometryEngine.Intersection(item.Geometry, geo2));
                                th.ID = Index++;
                                th.DIENTICH = Convert.ToDecimal(dtGiao);
                                th.ID_KYQH = Convert.ToInt32(feature.Attributes["MAKY"]);
                                //hiện trạng
                                // th.ID_MDSD= Convert.ToInt32(feature.Attributes["LOAIDAT"]);
                                String LoaiDatHT = Convert.ToString(feature.Attributes["LOAIDAT"]);

                                if (LoaiDatHT != null)
                                {
                                    if (DanhMucMDSD.Where(n => n.KIHIEU == LoaiDatHT).FirstOrDefault() != null)
                                    {
                                        th.ID_MDSD = DanhMucMDSD.Where(n => n.KIHIEU == LoaiDatHT).FirstOrDefault().ID;

                                    }
                                    else
                                    {
                                        th.ID_MDSD = 0;
                                    }


                                }

                                //Công trình
                                String LoaiDatChuyen = Convert.ToString(item.Attributes["LOAIDAT"]);
                                if (LoaiDatChuyen != null)
                                {
                                    if (DanhMucMDSD.Where(n => n.KIHIEU == LoaiDatChuyen).FirstOrDefault() != null)
                                    {
                                        th.ID_MDSD_CHUYEN = DanhMucMDSD.Where(n => n.KIHIEU == LoaiDatChuyen).FirstOrDefault().ID;

                                    }
                                    else
                                    {
                                        th.ID_MDSD_CHUYEN = 0;
                                    }


                                }
                                th.MAXA = Convert.ToString(feature.Attributes["MAKVHC"]);
                                    th.ROWID = Guid.NewGuid();
                                dbEF.TH_CHUCHUYENDATDAI.Add(th);

                                lstChuChuyen.Add(th);
                            }
                            catch (Exception e)
                            {
                                return false;
                            }
                        }
                    }



                }
            }

            dbEF.SaveChanges();
            if (dbEF.Database.Connection.State == ConnectionState.Open)
            {
                dbEF.Database.Connection.Close();
            }
            return true;
            }
            catch (Exception e)
            {
                int a = 0;
                return false;

            }
        }

    }
}