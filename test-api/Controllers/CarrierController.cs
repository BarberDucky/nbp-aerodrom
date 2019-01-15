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
    public class CarrierController : ApiController
    {
        // GET: api/Carrier/id
        [HttpGet]
        [Route("api/Carrier/GetCarriersByUser/{userId}")]
        public IEnumerable<CarrierDTO> GetCarrierByUser(string userId)
        {
            return CarrierDataProvider.GetCarrierByUser(userId);
        }

        [HttpGet]
        [Route("api/Carrier/GetCarrierByName/{carrierName}")]
        public IEnumerable<CarrierDTO> GetCarriersByName(string carrierName)
        {
            return CarrierDataProvider.GetCarriersByName(carrierName);
        }

        // GET: api/Carrier/5
        public CarrierDTO Get(string id)
        {
            return CarrierDataProvider.GetCarrier(id);
        }

        // GET: api/Carrier/5
        public IEnumerable<CarrierDTO> Get()
        {
            return CarrierDataProvider.GetAllCarriers();
        }

        // POST: api/Carrier
        public string Post([FromBody]CarrierDTO dto)
        {
            return CarrierDataProvider.InsertCarrier(dto);
        }

        // PUT: api/Carrier/5
        public bool Put(string id, [FromBody]CarrierDTO value)
        {
            return CarrierDataProvider.UpdateCarrier(id, value);
        }

        // DELETE: api/Carrier/5
        public void Delete(int id)
        {
        }
    }
}
