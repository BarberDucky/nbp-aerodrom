using nbp_autobus_data.BusinessModel;
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
    public class RedisCardDataProvider
    {
        private static string GetRideDateId(RedisReservation reservation)
        {
            return $"ride:{reservation.RideId}date:{reservation.Date.Date}:id";
        }

        private static string GetRideDateListId(RedisReservation reservation)
        {
            return $"list:ride:{reservation.RideId}date:{reservation.Date.Date}:id";
        }

       
        private static string GetRideDateId(string RideId, DateTime date)
        {
            return $"ride:{RideId}date:{date.Date}:id";
        }

        private static string GetRideDateListId(string RideId, DateTime date)
        {
            return $"list:ride:{RideId}date:{date.Date}:id";
        }

        public static bool CheckExistsNumSeats(string RideId, DateTime date, int numSeats)
        {
            try
            {
                if (!RedisRideDataProvider.ExistsRide(RideId))
                    return false;

                using (var client = new RedisClient(RedisDataLayer.SingleHost))
                {
                    var numS = client.Get<object>(GetRideDateId(RideId, date));
                    var numSInt = client.Get<int>(GetRideDateId(RideId, date));
                    var rideSeats = RedisRideDataProvider.NumSeats(RideId);
                    if (numS != null && numSInt >= numSeats)
                    {
                        return true;
                    }

                    if (numS == null && numSeats <= rideSeats)
                        return true;

                    return false;
                }

            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static bool CheckExistsNumSeats(BusinessTrip trip, int numCards)
        {
            foreach (var card in trip.CardsInTrip)
            {
                foreach (var ride in card.Rides)
                {
                    var rideDate = ride.TakeOfDate.Date;
                    var numSeats = CheckExistsNumSeats(ride.Ride.Id, rideDate, numCards);
                    if (!numSeats)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool AddCards(BusinessTrip trip, int numCards)
        {
            try
            {
                if (!CheckExistsNumSeats(trip, numCards))
                    return false;
                foreach (var card in trip.CardsInTrip)
                {
                    foreach (var ride in card.Rides)
                    {
                        var rideDate = ride.TakeOfDate.Date;
                        var key = GetRideDateId(ride.Ride.Id, rideDate);
                        using(var client = new RedisClient(RedisDataLayer.SingleHost))
                        {
                            if (client.Get<object>(key) != null)
                            {
                                client.DecrBy(key, numCards);
                            }
                            else
                            {
                                var numSeats = RedisRideDataProvider.NumSeats(ride.Ride.Id);
                                client.Set<int>(key, numSeats - numCards);
                            }
                           
                        }
                    }
                }

                return true;

            }
            catch(Exception e)
            {
                return false;
            }
        }

        public static IEnumerable<BusinessRideRelationship> CheckNumberOfSeats(IEnumerable<BusinessRideRelationship> paths, RedisSearch search)
        {
            var takeOfDate = search.TakeOfDate.Date;
            List<BusinessRideRelationship> list = new List<BusinessRideRelationship>();

            foreach (var path in paths)
            {
                bool add = true;
                foreach (var ride in path.Rides)
                {
                    var rideDate = takeOfDate.AddDays((ride.DayOfWeek - takeOfDate.DayOfWeek + 7) % 7);
                    var numSeats = CheckExistsNumSeats(ride.Id, rideDate, search.NumberOfCards);
                    if (!numSeats)
                    {
                        add = false;
                        break;
                    }
                }

                if (add)
                    list.Add(path);
            }

            return list;
        }

       

        
    }
}
