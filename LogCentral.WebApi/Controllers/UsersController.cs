using LogCentral.Common;
using LogCentral.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace LogCentral.WebApi.Controllers
{
    public class UsersController : ApiController
    {
        // GET api/users
        [Route("api/users")]
        public async Task<IHttpActionResult> Get(int? pageIndex = null, int? pageSize = null)
        {
            ResultPack<IEnumerable<Common.User>> res = null;
            if (pageIndex.HasValue && pageSize.HasValue)
                res = await DataFacade.Current.GetUsers(pageIndex.Value, pageSize.Value);
            else if (pageIndex.HasValue)
                res = await DataFacade.Current.GetUsers(pageIndex.Value);
            else
                res = await DataFacade.Current.GetUsers();

            if (!res.IsSucceeded)
            {
                //TODO: Log in file if is ON
                return StatusCode(HttpStatusCode.InternalServerError);
            }

            return Ok(res.ReturnParam);
        }

        // GET api/activeusers
        [Route("api/activeusers")]
        public async Task<IHttpActionResult> GetActiveUsers(DateTime? lastActivityThreshold, int? pageIndex = null, int? pageSize = null)
        {
            if (!lastActivityThreshold.HasValue)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
            ResultPack<IEnumerable<Common.User>> res = null;
            if (pageIndex.HasValue && pageSize.HasValue)
                res = await DataFacade.Current.GetActiveUsers(lastActivityThreshold.Value, pageIndex.Value, pageSize.Value);
            else if (pageIndex.HasValue)
                res = await DataFacade.Current.GetActiveUsers(lastActivityThreshold.Value, pageIndex.Value);
            else
                res = await DataFacade.Current.GetActiveUsers(lastActivityThreshold.Value);

            if (!res.IsSucceeded)
            {
                //TODO: Log in file if is ON
                return StatusCode(HttpStatusCode.InternalServerError);
            }

            return Ok(res.ReturnParam);
        }

        // GET api/users/user1
        [Route("api/users/{username}")]
        public async Task<IHttpActionResult> Get(string username)
        {
            var res = await DataFacade.Current.GetUser(username);
            if (!res.IsSucceeded)
            {
                //TODO: Log in file if is ON
                return StatusCode(HttpStatusCode.InternalServerError);
            }
            return Ok(res.ReturnParam);
        }

        //// POST api/users
        //public void Post([FromBody]string value)
        //{
        //}

        // PUT api/users
        [Route("api/users")]
        public async Task<IHttpActionResult> Put([FromBody]Common.User user)
        {
            var res = await DataFacade.Current.AddUser(user);
            if (!res.IsSucceeded)
            {
                //TODO: Log in file if is ON
                return StatusCode(HttpStatusCode.InternalServerError);
            }

            return Ok(res);
        }

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
    }
}