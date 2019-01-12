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
    public class UserController : ApiController
    {
        // GET: api/User
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/User/5
        public UserDTO Get(string id)
        {
            return UserDataProvider.GetUser(id);
        }

        
        [HttpPost]
        [Route("api/User/Login")]
        public UserDTO LogIn([FromBody]UserDTO user)
        {
            return UserDataProvider.LogInUser(user.Email, user.Password);
        }

        // POST: api/User
        //Register
        public string Post([FromBody]UserDTO user)
        {
            return UserDataProvider.RegisterUser(user);
        }

        // PUT: api/User/5
        public bool Put(string id, [FromBody]UserDTO value)
        {
            return UserDataProvider.UpdateUser(id, value);
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
        }
    }
}
