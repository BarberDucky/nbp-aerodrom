using nbp_autobus_data.DataLayers;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data.RedisDataProvider
{
    public class RedisRideDataProvider
    {
        private static string GetRideId(string rideId)
        {
            return "ride:" + rideId + ":id";
        }

        public static bool InsertRide(string rideId, int numSeats)
        {
            try
            {
                using (var client = new RedisClient(RedisDataLayer.SingleHost))
                {
                    if (client.Get<object>(GetRideId(rideId)) == null)
                    {
                        client.Set<int>(GetRideId(rideId), numSeats);
                    }
                    else
                        return false;
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static bool ExistsRide(string rideId)
        {
            try
            {
                using (var client = new RedisClient(RedisDataLayer.SingleHost))
                {
                    if (client.Get<object>(GetRideId(rideId)) != null)
                    {
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static int NumSeats(string rideId)
        {
            try
            {
                using (var client = new RedisClient(RedisDataLayer.SingleHost))
                {
                    return client.Get<int>(GetRideId(rideId));
                }
            }
            catch (Exception e)
            {
                return -1;
            }
        }
    }
}
