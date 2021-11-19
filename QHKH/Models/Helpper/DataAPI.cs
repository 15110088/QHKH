using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KHQH.Models.Helpper
{
    public class DataAPI<T>
    {
     
            public DataAPI()
            {
                Msg = "";
                Token = "";
                Code = 0;
            }
            public bool KetQua { get; set; }
            public string Msg { get; set; }
            public string Token { get; set; }
            public int Code { get; set; }
            public List<T> ListData { get; set; }
            public T Data { get; set; }


    }

}