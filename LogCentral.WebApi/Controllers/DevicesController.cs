using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LogCentral.WebApi.Controllers
{
    public class DevicesController : ApiController
    {
        // GET: api/Devices
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Devices/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Devices
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Devices/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Devices/5
        public void Delete(int id)
        {
        }
    }
}
