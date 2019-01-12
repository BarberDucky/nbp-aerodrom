using nbp_autobus_data.BusinessModel;
using nbp_autobus_data.DTOs;
using nbp_autobus_data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data.DataProvider
{
    public class RideDataProvider
    {
        private static bool Validate(CreateRideDTO dto)
        {
            if (dto.ArrivalStationId == dto.TakeOfStationId)
                return false;
            if (StationDataProvider.Get(dto.ArrivalStationId) == null)
                return false;
            if (StationDataProvider.Get(dto.TakeOfStationId) == null)
                return false;
            if (CarrierDataProvider.GetCarrier(dto.CarrierId) == null)
                return false;
            return true;
        }

        public static ReadRideDTO InsertRide(CreateRideDTO dto)
        {

            try
            {
                //if (!Validate(dto))
                //    return null;

                Ride newRide = CreateRideDTO.FromDTO(dto);
                newRide.Id = Guid.NewGuid().ToString();

                var query = DataLayer.Client.Cypher
                    .Create("(ride : Ride {newRide})")
                    .WithParam("newRide", newRide)
                    .With("ride")
                    .Match("(takeOf: Station)", "(arrival: Station)", "(carrier: Carrier)")
                    .Where((Station takeOf) => takeOf.Id == dto.TakeOfStationId)
                    .AndWhere((Station arrival) => arrival.Id == dto.ArrivalStationId)
                    .AndWhere((Carrier carrier) => carrier.Id == dto.CarrierId)
                    .Create("(ride) <- [: TAKES_OF] - (takeOf)")
                    .Create("(ride) - [: ARRIVES] -> (arrival)")
                    .Create("(ride) - [: CARRIER] -> (carrier)")
                    .Create("(takeOf) - [r: RIDE {newRide}] -> (arrival)")
                     .Return((ride, arrival, takeOf, carrier) => new BusinessRide()
                     {
                         Ride = ride.As<Ride>(),
                         Carrier = carrier.As<Carrier>(),
                         ArrivalStation = arrival.As<Station>(),
                         TakeOfStation = takeOf.As<Station>()

                     }).Results;

                if (query != null && query.Count() > 0)
                {
                    var ride = query.ToList();
                    return new ReadRideDTO(ride[0]);
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ReadRideDTO GetRide(string rideId)
        {
            try
            {
                var query = DataLayer.Client.Cypher
                    .Match("(ride: Ride)-[: ARRIVES ]->(arrive : Station)",
                    "(ride: Ride)<-[: TAKES_OF ]- (takeOf : Station)",
                    "(ride:Ride) - [: CARRIER] -> (carrier: Carrier)")
                    .Where((Ride ride) => ride.Id == rideId)
                    .Return((ride, arrive, takeOf, carrier) => new BusinessRide()
                    {
                        Ride = ride.As<Ride>(),
                        Carrier = carrier.As<Carrier>(),
                        ArrivalStation = arrive.As<Station>(),
                        TakeOfStation = takeOf.As<Station>()

                    }).Results;

                if (query != null && query.Count() > 0)
                {
                    var ride = query.ToList();
                    return new ReadRideDTO(ride[0]);
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static IEnumerable<ReadRideDTO> GetRidesByCarrier(string carrierId)
        {
            try
            {
                var query = DataLayer.Client.Cypher
                    .Match("(ride: Ride)-[: ARRIVES ]->(arrive : Station)",
                    "(ride: Ride)<-[: TAKES_OF ]- (takeOf : Station)",
                    "(ride:Ride) - [: CARRIER] -> (carrier: Carrier)")
                    .Where((Carrier carrier) => carrier.Id == carrierId)
                    .Return((ride, arrive, takeOf, carrier) => new BusinessRide()
                    {
                        Ride = ride.As<Ride>(),
                        Carrier = carrier.As<Carrier>(),
                        ArrivalStation = arrive.As<Station>(),
                        TakeOfStation = takeOf.As<Station>()

                    }).Results;

                //if (query != null && query.Count() > 0)
                //{
                //    var ride = query.ToList();
                //    return ReadRideDTO.FromEntityList(ride);
                //}
                //return null;
                return ReadRideDTO.FromEntityList(query);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static bool UpdateRide(string id, UpdateRideDTO dto)
        {
            try
            {
                Ride r = UpdateRideDTO.FromDTO(dto);
                r.Id = id;

                var query = DataLayer.Client.Cypher
                    .Match("(ride:Ride)", "(takeOf:Station) - [rel: RIDE] -> (arrival:Station)")
                    .Where((Ride ride) => ride.Id == id)
                    .AndWhere((Ride rel) => rel.Id == id)
                    .Set("ride = {newRide}")
                    .Set("rel = {newRide}")
                    .WithParam("newRide", r)
                    .Return<Ride>("ride")
                    .Results;

                if (query != null && query.Count() > 0)
                    return true;
                return false;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static bool DeleteRide(string id)
        {
            try
            {
                DataLayer.Client.Cypher
                    .Match("(ride: Ride) <- [t: TAKES_OF] - (takeOf: Station)",
                    "(ride: Ride) - [a: ARRIVES] -> (arrival: Station)",
                    "(ride: Ride) - [c: CARRIER] -> (carrier:Carrier)",
                    "(takeOf:Station) - [r: RIDE] -> (arrival:Station)")
                    .Where((Ride ride) => ride.Id == id)
                    .AndWhere((Ride r) => r.Id == id)
                    .Delete("ride, t, a, c, r").ExecuteWithoutResults();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
