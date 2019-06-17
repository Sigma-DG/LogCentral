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
    public class ApplicationsController : ApiController
    {
        // GET api/Applications
        [Route("api/Applications")]
        public async Task<IHttpActionResult> Get(int? pageIndex = null, int? pageSize = null)
        {
            ResultPack<IEnumerable<Common.Application>> res = null;
            if (pageIndex.HasValue && pageSize.HasValue)
                res = await DataFacade.Current.GetApplicationList(pageIndex.Value, pageSize.Value);
            else if (pageIndex.HasValue)
                res = await DataFacade.Current.GetApplicationList(pageIndex.Value);
            else
                res = await DataFacade.Current.GetApplicationList();

            if (!res.IsSucceeded)
            {
                //TODO: Log in file if is ON
                return StatusCode(HttpStatusCode.InternalServerError);
            }

            return Ok(res.ReturnParam);
        }

        // GET api/Applications/5
        [Route("api/Applications/{id}")]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var res = await DataFacade.Current.GetApplication(id);
            if (!res.IsSucceeded)
            {
                //TODO: Log in file if is ON
                return StatusCode(HttpStatusCode.InternalServerError);
            }
            return Ok(res.ReturnParam);
        }

        //// POST api/Applications
        //public void Post([FromBody]string value)
        //{
        //}

        // PUT api/Applications
        [Route("api/Applications")]
        public async Task<IHttpActionResult> Put([FromBody]Common.Application app)
        {
            var res = await DataFacade.Current.AddApplication(app);
            if (!res.IsSucceeded)
            {
                //TODO: Log in file if is ON
                return StatusCode(HttpStatusCode.InternalServerError);
            }

            return Ok(res);
        }

        //// DELETE api/Applications/5
        //public void Delete(int id)
        //{
        //}
    }
}