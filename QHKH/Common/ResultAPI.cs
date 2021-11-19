using KHQH.Models.Helpper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KHQH.Common
{
   
        public static class ResultAPI<T>
        {
            public static DataAPI<T> DATA(T Data,List<T> LstData,bool kq, string msg, int code, string token = "")
            {

            DataAPI<T> KetQua = new DataAPI<T>();
                if (!kq)
                {
                    KetQua.KetQua = false;
                    KetQua.Msg = msg;
                    KetQua.Code = code;
                }
                else
                {
                    KetQua.KetQua = true;
                    KetQua.Msg = msg;
                    KetQua.Code = code;
                    KetQua.Token = token;
                KetQua.Data = Data;
                KetQua.ListData = LstData;

            }

            return KetQua;
            }


           
        }
    
}