using nbp_autobus_data.DataLayers;
using nbp_autobus_data.DTOs;
using nbp_autobus_data.RedisModel;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data.RedisDataProvider
{
    public class RedisReservationDataProvider
    {
        private string globalRideDateCounter = "next.ridedate.id";

        private bool CheckNextUrlGlobalCounterExists()
        {
            using (var client = new RedisClient(RedisDataLayer.SingleHost))
            {
                var test = client.Get<object>(globalRideDateCounter);
                return (test != null) ? true : false;
            }

        }
        public string GetNextUrlId()
        {
            using (var client = new RedisClient(RedisDataLayer.SingleHost))
            {
                long nextCounterKey = client.Incr(globalRideDateCounter);
                return nextCounterKey.ToString("x");
            }
        }

        private string GetRideDateId(RedisReservation reservation)
        {
            return $"ride:{reservation.RideId}date:{reservation.Date.Date}:id";
        }

        private string GetRideDateListId(RedisReservation reservation)
        {
            return $"list:ride:{reservation.RideId}date:{reservation.Date.Date}:id";
        }

        private bool AddRideDate(RedisReservation reservation)
        {
            try
            {
                if (!RedisRideDataProvider.ExistsRide(reservation.RideId))
                    return false;

                using (var client = new RedisClient(RedisDataLayer.SingleHost))
                {
                    var existsRes = client.Get<RedisReservation>(GetRideDateId(reservation));

                    if (existsRes == null)
                    {
                        client.Set<RedisReservation>(GetRideDateId(reservation), reservation);
                    }
                    return true;

                }

            }
            catch (Exception e)
            {
                return false;
            }
        }

        private long GetListCount(RedisReservation reservation)
        {
            try
            {
                using(var client = new RedisClient(RedisDataLayer.SingleHost))
                {
                    return client.GetListCount(GetRideDateListId(reservation));
                }

            }catch(Exception e)
            {
                return -1;
            }
        }

        private bool AddToList(RedisReservation res, string resId)
        {
            try
            {
                
                using (var client = new RedisClient(RedisDataLayer.SingleHost))
                {
                    client.PushItemToList(GetRideDateListId(res), resId);
                    return true;
                }

            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool AddReservation(CreateReservationDTO dto, string reservationId)
        {
            try
            {
                foreach (var rideId in dto.RidesId)
                {
                    RedisReservation redisR = new RedisReservation
                    {
                        Date = dto.Date,
                        RideId = rideId
                    };

                    if (!AddRideDate(redisR))
                        return false;

                    int numSeats = RedisRideDataProvider.NumSeats(rideId);
                    long seatsCount = GetListCount(redisR);
                    if (numSeats <= seatsCount)
                        return false;
                }

                foreach (var rideId in dto.RidesId)
                {
                    RedisReservation redisR = new RedisReservation
                    {
                        Date = dto.Date,
                        RideId = rideId
                    };

                    AddToList(redisR, reservationId);
                }

                return true;
            }

            catch (Exception e)
            {
                return false;
            }
        }
    }
}
