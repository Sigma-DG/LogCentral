using LogCentral.Common;
using LogCentral.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace LogCentral.WebApi.Controllers
{
    public class LogsController : ApiController
    {
        // GET: api/Logs
        [Route("api/logs")]
        public async Task<IHttpActionResult> Get(int? pageIndex = null, int? pageSize = null)
        {
            ResultPack<IEnumerable<Common.Log>> res = null;
            if (pageIndex.HasValue && pageSize.HasValue)
                res = await DataFacade.Current.GetLogs(pageIndex.Value, pageSize.Value);
            else if (pageIndex.HasValue)
                res = await DataFacade.Current.GetLogs(pageIndex.Value);
            else
                res = await DataFacade.Current.GetLogs();

            if (!res.IsSucceeded)
            {
                //TODO: Log in file if is ON
                return StatusCode(HttpStatusCode.InternalServerError);
            }

            return Ok(res.ReturnParam);
        }

        // GET: api/Logs
        [Route("api/users/{username}/logs")]
        public async Task<IHttpActionResult> GetByUser(string username, int? pageIndex = null, int? pageSize = null)
        {
            ResultPack<IEnumerable<Common.Log>> res = null;
            if (pageIndex.HasValue && pageSize.HasValue)
                res = await DataFacade.Current.GetLogsOfUser(username, pageIndex.Value, pageSize.Value);
            else if (pageIndex.HasValue)
                res = await DataFacade.Current.GetLogsOfUser(username, pageIndex.Value);
            else
                res = await DataFacade.Current.GetLogsOfUser(username);

            if (!res.IsSucceeded)
            {
                //TODO: Log in file if is ON
                return StatusCode(HttpStatusCode.InternalServerError);
            }

            return Ok(res.ReturnParam);
        }

        // GET: api/Logs
        [Route("api/devices/{deviceid}/logs")]
        public async Task<IHttpActionResult> GetByDevice(Guid deviceid, int? pageIndex = null, int? pageSize = null)
        {
            ResultPack<IEnumerable<Common.Log>> res = null;
            if (pageIndex.HasValue && pageSize.HasValue)
                res = await DataFacade.Current.GetLogsOfDevice(deviceid, pageIndex.Value, pageSize.Value);
            else if (pageIndex.HasValue)
                res = await DataFacade.Current.GetLogsOfDevice(deviceid, pageIndex.Value);
            else
                res = await DataFacade.Current.GetLogsOfDevice(deviceid);

            if (!res.IsSucceeded)
            {
                //TODO: Log in file if is ON
                return StatusCode(HttpStatusCode.InternalServerError);
            }

            return Ok(res.ReturnParam);
        }

        // PUT: api/Logs/5
        [Route("api/logs")]
        public async Task<IHttpActionResult> Put([FromBody]Common.Log log)
        {
            var res = await DataFacade.Current.AddLog(log);
            if (!res.IsSucceeded)
            {
                //TODO: Log in file if is ON
                //return StatusCode(500, new Exception(res.Message, new Exception(res.ErrorMetadata)));
            }

            return Ok(res);
        }

        // DELETE: api/Logs/5
        [Route("api/logs")]
        public async Task<IHttpActionResult> Delete()
        {
            var res = await DataFacade.Current.ClearLogs();
            if (!res.IsSucceeded)
            {
                //TODO: Log in file if is ON
                //return StatusCode(500, new Exception(res.Message, new Exception(res.ErrorMetadata)));
            }

            return Ok(res);
        }
    }
}
