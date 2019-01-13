using nbp_autobus_data.DTOs;
using nbp_autobus_data.RedisDataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace test_api.Controllers
{
    public class ReservationController : ApiController
    {
        // GET: api/Reservation
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Reservation/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Reservation
        public void Post([FromBody]CreateReservationDTO value)
        {
            RedisReservationDataProvider p = new RedisReservationDataProvider();
            p.AddReservation(value, Guid.NewGuid().ToString());
        }

        // PUT: api/Reservation/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Reservation/5
        public void Delete(int id)
        {
        }
    }
}
