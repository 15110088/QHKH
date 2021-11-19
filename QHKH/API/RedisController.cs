using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace KeHoachQH.API
{
    [Route("api/Redis/{action}", Name = "Redis")]
    public class RedisController : ApiController
    {
       // private readonly IDistributedCache _distributedCache;

        private string HOST_REDIS = "192.168.1.6:6379";
        [HttpGet]
        public string Get()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(HOST_REDIS);


            // Lấy DB
            IDatabase db = redis.GetDatabase(1);
            db.StringIncrement("Website_Counter", 1);
            var s= db.KeyTimeToLive("Website_Counter");
            if (s == null)
            {
                db.KeyExpire("Website_Counter", DateTime.Now.AddSeconds(20));

            }
            else 
            {
                if(s.Value.Seconds<1)
                {
                    db.StringIncrement("Website_Counter", 1);
                    db.KeyExpire("Website_Counter", DateTime.Now.AddSeconds(20));
                }    
              


            }

            db.KeyTimeToLive("Website_Counter");
            // Ping thử
            if (db.Ping().TotalSeconds > 5)
            {
                throw new TimeoutException("Server Redis không hoạt động");
            }

            // Lưu dữ liệu
            db.StringSet("hello", "Xin chào Redis C#");

            // Đọc lại dữ liệu
            var vl = db.StringGet("hello");
            Console.WriteLine(vl);

            // Xóa một key
            db.KeyDelete("mykey");

            // Lấy tất cả các key
            IServer server = redis.GetServer(HOST_REDIS);
            var keys = server.Keys(pattern: "*");
            string all = "";
            foreach (var k in keys)
            {
                all += k.ToString();
            }

            // Xóa tất cả các key
            // server.FlushAllDatabases();
            return db.StringGet("Website_Counter") +" "+ db.KeyTimeToLive("Website_Counter").Value.Seconds;
        }
    }
}
