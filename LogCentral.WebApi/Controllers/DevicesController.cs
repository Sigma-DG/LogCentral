﻿using LogCentral.Common;
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
    public class DevicesController : ApiController
    {
        // GET: api/Devices
        [Route("api/devices")]
        public async Task<IHttpActionResult> Get(int? pageIndex = null, int? pageSize = null)
        {
            ResultPack<IEnumerable<Common.Device>> res = null;
            if (pageIndex.HasValue && pageSize.HasValue)
                res = await DataFacade.Current.GetDevices(pageIndex.Value, pageSize.Value);
            else if (pageIndex.HasValue)
                res = await DataFacade.Current.GetDevices(pageIndex.Value);
            else
                res = await DataFacade.Current.GetDevices();

            if (!res.IsSucceeded)
            {
                //TODO: Log in file if is ON
                return StatusCode(HttpStatusCode.InternalServerError);
            }

            return Ok(res.ReturnParam);
        }

        // GET api/activedevices
        [Route("api/activedevices")]
        public async Task<IHttpActionResult> GetActiveDevices(DateTime? lastActivityThreshold, int? pageIndex = null, int? pageSize = null)
        {
            if (!lastActivityThreshold.HasValue)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
            ResultPack<IEnumerable<Common.Device>> res = null;
            if (pageIndex.HasValue && pageSize.HasValue)
                res = await DataFacade.Current.GetActiveDevices(lastActivityThreshold.Value, pageIndex.Value, pageSize.Value);
            else if (pageIndex.HasValue)
                res = await DataFacade.Current.GetActiveDevices(lastActivityThreshold.Value, pageIndex.Value);
            else
                res = await DataFacade.Current.GetActiveDevices(lastActivityThreshold.Value);

            if (!res.IsSucceeded)
            {
                //TODO: Log in file if is ON
                return StatusCode(HttpStatusCode.InternalServerError);
            }

            return Ok(res.ReturnParam);
        }

        // GET: api/Devices/5
        [Route("api/devices/{id}")]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var res = await DataFacade.Current.GetDevice(id);
            if (!res.IsSucceeded)
            {
                //TODO: Log in file if is ON
                return StatusCode(HttpStatusCode.InternalServerError);
            }
            return Ok(res.ReturnParam);
        }

        //// POST: api/Devices
        //public void Post([FromBody]string value)
        //{
        //}

        // PUT: api/Devices
        [Route("api/devices")]
        public async Task<IHttpActionResult> Put([FromBody]Common.Device device)
        {
            var res = await DataFacade.Current.AddDevice(device);
            if (!res.IsSucceeded)
            {
                //TODO: Log in file if is ON
                return StatusCode(HttpStatusCode.InternalServerError);
            }

            return Ok(res);
        }

        //// DELETE: api/Devices/5
        //public void Delete(int id)
        //{
        //}
    }
}
