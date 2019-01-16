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
    public class RideController : ApiController
    {

        // GET: api/Ride/5
        public ReadRideDTO Get(string id)
        {
            return RideDataProvider.GetRide(id);
        }

        [HttpGet]
        [Route("api/Ride/GetRidesByCarrier/{carrierId}")]
        public IEnumerable<ReadRideDTO> GetCarrierByUser(string carrierId)
        {
            return RideDataProvider.GetRidesByCarrier(carrierId);
        }

        // POST: api/Ride
        public ReadRideDTO Post([FromBody]CreateRideDTO value)
        {
            return RideDataProvider.InsertRide(value);
        }

        [HttpPost]
        [Route("api/Ride/FindPath")]
        public SearchResultsDTO FindPath([FromBody]SearchDTO value)
        {
            return RideDataProvider.FindPath(value);
        }

        // PUT: api/Ride/5
        public bool Put(string id, [FromBody]UpdateRideDTO value)
        {
            return RideDataProvider.UpdateRide(id, value);
        }

        // DELETE: api/Ride/5
        public bool Delete(string id)
        {
            return RideDataProvider.DeleteRide(id);
        }
    }
}
