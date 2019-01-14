using nbp_autobus_data.BusinessModel;
using nbp_autobus_data.DTOs;
using nbp_autobus_data.Model;
using nbp_autobus_data.RedisDataProvider;
using Neo4jClient.Cypher;
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

                RideRelationship rel = new RideRelationship(newRide, dto.CarrierId);

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
                    .Create("(takeOf) - [r: RIDE {rideRel}] -> (arrival)")
                    .WithParam("rideRel", rel)
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
                    RedisRideDataProvider.InsertRide(newRide.Id, newRide.NumberOfSeats);
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
                return new List<ReadRideDTO>();
            }
        }

        public static bool UpdateRide(string id, UpdateRideDTO dto)
        {
            try
            {
                Ride r = UpdateRideDTO.FromDTO(dto);
                r.Id = id;

                RideRelationship rideRel = new RideRelationship(r, dto.CarrierId);

                var query = DataLayer.Client.Cypher
                    .Match("(ride:Ride)", "(takeOf:Station) - [rel: RIDE] -> (arrival:Station)")
                    .Where((Ride ride) => ride.Id == id)
                    .AndWhere((Ride rel) => rel.Id == id)
                    .Set("ride = {newRide}")
                    .Set("rel = {rideRel}")
                    .WithParam("newRide", r)
                    .WithParam("rideRel", rideRel)
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

                RedisDataProvider.RedisRideDataProvider.DeleteRide(id);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static IEnumerable<BusinessTrip> FindPath(SearchDTO search)
        {
            try
            {
                var cachedResult = RedisSearchDataProvider.GetCachedSearch(search);
                if (cachedResult != null)
                    return cachedResult;

                var takeOfDay = search.TakeOfDate.DayOfWeek;

                var query = DataLayer.Client.Cypher
                    .Match("p = (takeOf: Station) - [ride: RIDE *..5]->(arrive: Station)")
                    .Where((Station takeOf) => takeOf.Id == search.TakeOfStationId)
                    .AndWhere((Station arrive) => arrive.Id == search.ArrivalStationId)
                    .AndWhere("(ride[0]).DayOfWeek = {takeOfDay} ")
                    .WithParam("takeOfDay", takeOfDay)
                    .AndWhere("all (index in range(0, size(ride) -2)" +
                    " where ( (ride[index]).ArrivalTime <= (ride[index+1]).TakeOfTime and (ride[index]).DayOfWeek = (ride[index]).DayOfWeek ) " +
                    "or (ride[index]).DayOfWeek <> (ride[index]).DayOfWeek )")
                    //.AndWhere("all (index in range(0, size(ride) -2)" +
                    //" where ( (ride[index]).ArrivalTime < (ride[index+1]).TakeOfTime and (ride[index]).DayOfWeek = (ride[index]).DayOfWeek ) " +
                    //"or (ride[index]).DayOfWeek <> (ride[index]).DayOfWeek )")
                    //  .Return<IEnumerable<RideRelationship>>("relationships (p)")
                    .Return(() => new BusinessRideRelationship
                    {
                        Rides = Return.As<IEnumerable<RideRelationship>>("relationships (p)"),
                        Stations = Return.As<IEnumerable<Station>>("nodes (p)"),
                    })
                    .Results;

                var valid = CheckIfPathInRange(query, search);

                //check da li ima mesta

                var checkNumSeats = RedisReservationDataProvider.CheckNumberOfSeats(valid, search);

                var result = GetSearchResults(checkNumSeats, search.TakeOfDate);

                RedisSearchDataProvider.CacheSearch(search, result.ToList());

                return result;
            }
            catch (Exception e)
            {
                return new List<BusinessTrip>();
            }
        }

        private static IEnumerable<BusinessRideRelationship> CheckIfPathInRange(IEnumerable<BusinessRideRelationship> paths, SearchDTO search)
        {
            List<BusinessRideRelationship> list = new List<BusinessRideRelationship>();
            DateTime arrivalDate = search.ArrivalDate.Date;
            DateTime takeOfDate = search.TakeOfDate.Date;

            foreach (var path in paths)
            {
                var totalDays = 0;
                bool add = true;
                for (int i = 1; i < path.Rides.Count(); i++)
                {
                    totalDays += (path.Rides.ToList()[i].DayOfWeek - path.Rides.ToList()[i - 1].DayOfWeek + 7) % 7;
                    if (takeOfDate.AddDays(totalDays) > arrivalDate)
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

        private static IEnumerable<BusinessTrip> GetSearchResults(IEnumerable<BusinessRideRelationship> rides, DateTime TakeOfDate)
        {
            List<BusinessTrip> results = new List<BusinessTrip>();

            foreach (var path in rides)
            {
                results.Add(GroupByCarrier(path, TakeOfDate));
            }
            return results;
        }

        private static BusinessTrip GroupByCarrier(BusinessRideRelationship rides, DateTime TakeOfDate)
        {
            int start = 0;
            BusinessTrip trip = new BusinessTrip();

            while (start < rides.Rides.Count())
            {
                var currentCarrier = rides.Rides.ToList()[start].CarrierId;
                var rideDay = rides.Rides.ToList()[start].DayOfWeek;
                BusinessCard card = new BusinessCard()
                {
                    CarrierId = currentCarrier
                };
                card.Card.TakeOfDate = TakeOfDate.AddDays((rideDay - TakeOfDate.DayOfWeek + 7) % 7);

                while (start < rides.Rides.Count() && rides.Rides.ToList()[start].CarrierId == currentCarrier)
                {
                    card.Card.Price += rides.Rides.ToList()[start].RidePrice;
                    trip.TotalCost += rides.Rides.ToList()[start].RidePrice;
                    Ride ride = new Ride(rides.Rides.ToList()[start]);
                    var rDay = ride.DayOfWeek;
                    card.Rides.Add(new BusinessRide()
                    {
                        Ride = ride,
                        TakeOfDate = TakeOfDate.AddDays((rDay - TakeOfDate.DayOfWeek + 7) % 7),
                        TakeOfStation = rides.Stations.ToList()[start],
                        ArrivalStation = rides.Stations.ToList()[start + 1]
                    });

                    start++;
                }

                trip.CardsInTrip.Add(card);
                trip.OverlayNumber++;

            }
            return trip;
        }

    }
}
