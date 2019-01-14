using nbp_autobus_data.BusinessModel;
using nbp_autobus_data.DataLayers;
using nbp_autobus_data.DTOs;
using Newtonsoft.Json;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data.RedisDataProvider
{
    public class RedisSearchDataProvider
    {

        public static void CacheSearch(SearchDTO search, List<BusinessTrip> searchResult)
        {
            using (var client = new RedisClient(RedisDataLayer.SingleHost))
            {
                TimeSpan expire = new TimeSpan(0, 1, 0);
                var searchKey = JsonConvert.SerializeObject(search);
                client.Set<List<BusinessTrip>>(searchKey, searchResult, expire);

            }

        }

        public static List<BusinessTrip> GetCachedSearch(SearchDTO search)
        {
            using (var client = new RedisClient(RedisDataLayer.SingleHost))
            {
                var searchKey = JsonConvert.SerializeObject(search);
                var result = client.Get<List<BusinessTrip>>(searchKey);
                return result;
            }
        }
    }
}
