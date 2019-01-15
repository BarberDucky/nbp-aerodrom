using nbp_autobus_data.DataLayers;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data.RedisDataProvider
{
    public class RedisReviewDataProvider
    {
     
        private static string GetCarrierId(string carrierId)
        {
            return "carrier:" + carrierId + ":id";
        }

        private static string GetCarrierListId(string carrierId)
        {
            return "carrierList:" + carrierId;
        }

        public static bool InsertReview(string reviewId, string carrierId, int grade)
        {
            try
            {
                using(var client = new RedisClient(RedisDataLayer.SingleHost))
                {
                    client.IncrementValueBy(GetCarrierId(carrierId), grade);
                    client.PushItemToList(GetCarrierListId(carrierId), reviewId);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static float GetAvgGradeCarrier(string carrierId)
        {
            float res = 0;
            try
            {
                
                using (var client = new RedisClient(RedisDataLayer.SingleHost))
                {
                    var totalGrade = (float)client.Get<int>(GetCarrierId(carrierId));
                    var reviewCount = (float)client.GetListCount(GetCarrierListId(carrierId));

                    
                    if (reviewCount != 0)
                    {
                        res = totalGrade / reviewCount;
                    }
                }

                return res;
            }
            catch (Exception e)
            {
                return res;
            }
        }
    }
}
