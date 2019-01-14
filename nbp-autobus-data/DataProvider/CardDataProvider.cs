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
    public class CardDataProvider
    {
        private static string InsertCard(BusinessCard card, string userId)
        {
            try
            {
                Card newCard = card.Card;
                newCard.Id = Guid.NewGuid().ToString();
                //TODO Get seat number
                newCard.SeatNumber = 0;

                var query = DataLayer.Client.Cypher
                    .Create("(card : Card {newCard})")
                    .WithParam("newCard", newCard)
                    .With("card")
                    .Match("(user: User),(carrier: Carrier)")
                    .Where((User user) => user.Id == userId)
                    .Create("(card) - [: CARD_BOUGHT] -> (user)")
                    .Return<Card>("card")
                    .Results;
                if (query != null)
                {
                    foreach (var ride in card.Rides)
                    {
                        var q = DataLayer.Client.Cypher
                            .Match("(c: Card),(r: Ride)")
                            .Where((Card c) => c.Id == newCard.Id)
                            .AndWhere((Ride r) => r.Id == ride.Ride.Id)
                            .Create("(c) - [: OF_RIDE] -> (r)")
                            .Return<Card>("c")
                            .Results;
                        if (q == null)
                            return null;
                    }
                }

                return newCard.Id;

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static bool InsertTrip(BusinessTrip trip, string userId, int numSeats)
        {
            try
            {
                if (!RedisDataProvider.RedisReservationDataProvider.AddCards(trip, numSeats))
                    return false;
                for(int i = 0; i < numSeats; i++)
                {
                    foreach (var card in trip.CardsInTrip)
                    {
                        if (InsertCard(card, userId) == null)
                            return false;
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            
        }

        public static IEnumerable<CardDTO> GetAllCardsOfUser(string userId)
        {
            try
            {
                var query = DataLayer.Client.Cypher
                    .Match("(card: Card) - [: CARD_BOUGHT] -> (user: User)")
                    .Where((User user) => user.Id == userId)
                    .Return<Card>("card")
                    .Results;

                return CardDTO.FromEntityList(query);
            }
            catch (Exception e)
            {
                return new List<CardDTO>();
            }
        }

        public static IEnumerable<ReadRideDTO> GetRidesOfCard(string cardId)
        {
            try
            {
                var query = DataLayer.Client.Cypher
                    .Match("(c:Card) - [: OF_RIDE] -> (ride:Ride)", 
                    "(ride: Ride)-[: ARRIVES ]->(arrive : Station)",
                    "(ride: Ride)<-[: TAKES_OF ]- (takeOf : Station)",
                    "(ride:Ride) - [: CARRIER] -> (carrier: Carrier)")
                    .Where((Card c) => c.Id == cardId)
                    .Return((ride, arrive, takeOf, carrier) => new BusinessRide()
                    {
                        Ride = ride.As<Ride>(),
                        Carrier = carrier.As<Carrier>(),
                        ArrivalStation = arrive.As<Station>(),
                        TakeOfStation = takeOf.As<Station>()

                    }).Results;

                return ReadRideDTO.FromEntityList(query);
            }
            catch (Exception e)
            {
                return new List<ReadRideDTO>();
            }
        }
    }
}
