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
    public class ReviewController : ApiController
    {
        [HttpGet]
        [Route("api/Review/GetReviewsForCarrier/{carrierId}")]
        public IEnumerable<ReadReviewDTO> GetReviewsForCarrier(string carrierId)
        {
            return ReviewDataProvider.GetReviewsByCarrier(carrierId);
        }

        [HttpGet]
        [Route("api/Review/GetReviewsByUser/{userId}")]
        public IEnumerable<ReadReviewDTO> GetReviewsByUser(string userId)
        {
            return ReviewDataProvider.GetReviewsByUser(userId);
        }

        // GET: api/Review/5
        public ReadReviewDTO Get(string id)
        {
            return ReviewDataProvider.GetReview(id);
        }

        // POST: api/Review
        public string Post([FromBody]CreateReviewDTO value)
        {
            return ReviewDataProvider.InsertReview(value);
        }

    }
}
