using nbp_autobus_data.BusinessModel;
using nbp_autobus_data.DataProvider;
using nbp_autobus_data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace test_api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CardController : ApiController
    {
        [HttpGet]
        [Route("api/Card/GetCardsByUser/{userId}")]
        public IEnumerable<CardDTO> GetCarrierByUser(string userId)
        {
            return CardDataProvider.GetAllCardsOfUser(userId);
        }

        [HttpGet]
        [Route("api/Card/GetRidesOfCard/{cardId}")]
        public IEnumerable<ReadRideDTO> GetRidesOfCard(string cardId)
        {
            return CardDataProvider.GetRidesOfCard(cardId);
        }

        // POST: api/Card
        [HttpPost]
        [Route("api/Card/User/{userId}/NumberOfSeats/{numSeats}")]
        public bool Post(string userId, int numSeats, [FromBody]BusinessTrip value)
        {
            return CardDataProvider.InsertTrip(value, userId, numSeats);
        }

    }
}
