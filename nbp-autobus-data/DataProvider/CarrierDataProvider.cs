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
    public class CarrierDataProvider
    {
        #region Private

        private static bool ExistsCarrier(string carrierName)
        {
            try
            {
                var query = DataLayer.Client.Cypher
                    .Match("(carrier: Carrier)")
                    .Where((Carrier carrier) => carrier.Name == carrierName)
                    .Return<Carrier>("carrier")
                    .Results;

                if (query != null && query.Count<Carrier>() > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private static User CreatedByUser(string carrierId)
        {
            try
            {
                var query = DataLayer.Client.Cypher
                    .Match("(carrier: Carrier)-[CREATED_BY]->(user: User)")
                    .Where((Carrier carrier) => carrier.Id == carrierId)
                    .Return<User>("user")
                    .Results;
                if (query != null && query.Count<User>() > 0)
                {
                    return query.ToList()[0];
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        #endregion

        public static string InsertCarrier(CarrierDTO dto)
        {
            try
            {
                if (ExistsCarrier(dto.Name))
                    return null;

                if (UserDataProvider.GetUser(dto.UserId) == null)
                    return null;

                Carrier newCarrier = CarrierDTO.FromDTO(dto);
                newCarrier.Id = Guid.NewGuid().ToString();

                var query = DataLayer.Client.Cypher
                    .Create("(carrier : Carrier {newCarrier})")
                    .WithParam("newCarrier", newCarrier)
                    .With("carrier")
                    .Match("(user: User)")
                    .Where((User user) => user.Id == dto.UserId)
                    .Create("(carrier) - [: CREATED_BY] -> (user)")
                    .Return<Carrier>("carrier")
                    .Results;

                if (query != null && query.Count() > 0)
                {
                    return query.ToList()[0].Id;
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static CarrierDTO GetCarrier(string carrierId)
        {
            try
            {
                var query = DataLayer.Client.Cypher
                    .Match("(carrier: Carrier)-[CREATED_BY]->(user: User)")
                    .Where((Carrier carrier) => carrier.Id == carrierId)
                    .OptionalMatch("(ride:Ride) - [: CARRIER] -> (c:Carrier)")
                    .Where((Carrier carrier, Carrier c) => c.Id == carrierId)
                    .OptionalMatch("(arrival:Station) <-[: ARRIVES] -(r: Ride) < - [: TAKES_OF] -(takeOf: Station)")
                    .Where((Ride ride, Ride r) => r.Id == ride.Id)
                    .Return((carrier, user, ride, takeOf, arrival) => new BusinessCarrier
                    {
                        Carrier = carrier.As<Carrier>(),
                        UserId = user.As<User>().Id,
                        Rides = ride.CollectAs<Ride>(),
                        TakeOfStations = takeOf.CollectAs<Station>(),
                        ArrivalStations = arrival.CollectAs<Station>()
                    })
                    .Results;

                if (query != null && query.Count() > 0)
                {
                    var c = query.ToList()[0];
                    return new CarrierDTO(c);
                }

                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static bool UpdateCarrier(string carrierdId, CarrierDTO dto)
        {
            try
            {
                
                Dictionary<string, object> queryDict = new Dictionary<string, object>
                {
                   
                    { "PhoneNumber", dto.PhoneNumber },
                    { "Website", dto.Website }
                };

                var query = DataLayer.Client.Cypher
                    .Match("(carrier: Carrier)")
                    .Where((Carrier carrier) => carrier.Id == carrierdId)
                    .Set("carrier.PhoneNumber = {PhoneNumber}")
                    .Set("carrier.Website = {Website}")
                    .WithParams(queryDict)
                    .Return<Carrier>("carrier")
                    .Results;
                if (query != null && query.Count<Carrier>() > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static IEnumerable<CarrierDTO> GetCarrierByUser(string userId)
        {
            try
            {
                var query = DataLayer.Client.Cypher
                    .Match("(carrier: Carrier)-[CREATED_BY]->(user: User)")
                    .Where((User user) => user.Id == userId)
                    .OptionalMatch("(ride:Ride) - [: CARRIER] -> (c:Carrier)")
                    .Where((Carrier carrier, Carrier c) => c.Id == carrier.Id)
                    .OptionalMatch("(arrival:Station) <-[: ARRIVES] -(r: Ride) < - [: TAKES_OF] -(takeOf: Station)")
                    .Where((Ride ride, Ride r) => r.Id == ride.Id)
                    .Return((carrier, ride, takeOf, arrival, user) => new BusinessCarrier
                    {
                        Carrier = carrier.As<Carrier>(),
                        UserId = user.As<User>().Id,
                        Rides = ride.CollectAs<Ride>(),
                        TakeOfStations = takeOf.CollectAs<Station>(),
                        ArrivalStations = arrival.CollectAs<Station>()
                    })
                    .Results;

                return CarrierDTO.FromEntityList(query);
            }
            catch (Exception e)
            {
                return new List<CarrierDTO>();
            }
        }

        public static IEnumerable<CarrierDTO> GetAllCarriers()
        {
            try
            {
                var query = DataLayer.Client.Cypher
                    .Match("(carrier: Carrier)-[CREATED_BY]->(user: User)")
                    .Return((carrier, user) => new BusinessCarrier()
                    {
                        Carrier = carrier.As<Carrier>(),
                        UserId = user.As<User>().Id
                    })
                    .Results;

                
                return CarrierDTO.FromEntityList(query);
            }
            catch (Exception e)
            {
                return new List<CarrierDTO>();
            }
        }

        public static IEnumerable<CarrierDTO> GetCarriersByName(string carrierName)
        {
            try
            {
                var query = DataLayer.Client.Cypher
                    .Match("(carrier: Carrier)-[CREATED_BY]->(user: User)")
                    .Where((Carrier carrier) => carrier.Name.Contains(carrierName))
                    .OptionalMatch("(ride:Ride) - [: CARRIER] -> (c:Carrier)")
                    .Where((Carrier carrier, Carrier c) => c.Id == carrier.Id)
                    .OptionalMatch("(arrival:Station) <-[: ARRIVES] -(r: Ride) < - [: TAKES_OF] -(takeOf: Station)")
                    .Where((Ride ride, Ride r) => r.Id == ride.Id)
                    .Return((carrier, ride, takeOf, arrival, user) => new BusinessCarrier
                    {
                        Carrier = carrier.As<Carrier>(),
                        UserId = user.As<User>().Id,
                        Rides = ride.CollectAs<Ride>(),
                        TakeOfStations = takeOf.CollectAs<Station>(),
                        ArrivalStations = arrival.CollectAs<Station>()
                    })
                    .Results;

                return CarrierDTO.FromEntityList(query);
            }
            catch (Exception e)
            {
                return new List<CarrierDTO>();
            }
        }

    }
}
