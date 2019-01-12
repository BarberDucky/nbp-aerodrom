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
    public class StationController : ApiController
    {
        // GET: api/Station
        public IEnumerable<StationDTO> Get()
        {
            return StationDataProvider.GetAllStations();
        }

        // GET: api/Station/5
        public StationDTO Get(string id)
        {
            return StationDataProvider.Get(id);
        }

        // POST: api/Station
        public StationDTO Post([FromBody]StationDTO dto)
        {
            return StationDataProvider.InsertStation(dto);
        }

        // PUT: api/Station/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Station/5
        public void Delete(int id)
        {
        }
    }
}
