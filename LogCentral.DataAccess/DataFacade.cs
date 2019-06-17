using LogCentral.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogCentral.DataAccess
{
    public class DataFacade
    {
        public static string ConnectionString { get; set; }

        #region Singleton
        private static object _locker = new object();

        private DataFacade()
        {

        }

        private static DataFacade _current = null;
        public static DataFacade Current
        {
            get
            {
                if (_current == null)
                {
                    lock (_locker)
                    {
                        if (_current == null) _current = new DataFacade();
                        return _current;
                    }
                }
                return _current;
            }
        }
        #endregion

        #region Log
        public async Task<ResultPack<IEnumerable<Common.Log>>> GetLogs(int pageIndex = 0, int pageSize = 50)
        {
            var res = new ResultPack<IEnumerable<Common.Log>>();
            using (var db = new DBEntities())
            {
                try
                {
                    var dbLogs = await (from d in db.Logs orderby d.UtcTime descending select d).Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();

                    res.ReturnParam = dbLogs.Select(x => x.ToCommonLog()).ToList();
                    res.IsSucceeded = true;
                }
                catch (Exception ex)
                {
                    res.IsSucceeded = false;
                    res.Message = ex.Message;
                    res.ErrorMetadata = ex.StackTrace;
                }
            }

            return res;
        }

        public async Task<ResultPack<IEnumerable<Common.Log>>> GetLogsOfDevice(Guid deviceId, int pageIndex = 0, int pageSize = 50)
        {
            var res = new ResultPack<IEnumerable<Common.Log>>();
            using (var db = new DBEntities())
            {
                try
                {
                    var dbLogs = await (from d in db.Logs
                                        where d.Device.HasValue && d.Device.Value.Equals(deviceId)
                                        orderby d.UtcTime descending
                                        select d).Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();

                    res.ReturnParam = dbLogs.Select(x => x.ToCommonLog()).ToList();
                    res.IsSucceeded = true;
                }
                catch (Exception ex)
                {
                    res.IsSucceeded = false;
                    res.Message = ex.Message;
                    res.ErrorMetadata = ex.StackTrace;
                }
            }

            return res;
        }

        public async Task<ResultPack<IEnumerable<Common.Log>>> GetLogsOfUser(string username, int pageIndex = 0, int pageSize = 50)
        {
            if (string.IsNullOrWhiteSpace(username)) return new ResultPack<IEnumerable<Common.Log>> { IsSucceeded = false, Message = "Username is empty or white space" };
            username = username.ToLower();
            var res = new ResultPack<IEnumerable<Common.Log>>();
            using (var db = new DBEntities())
            {
                try
                {
                    var dbLogs = await (from d in db.Logs
                                        where d.Username.ToLower().Contains(username)
                                        orderby d.UtcTime descending
                                        select d).Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();

                    res.ReturnParam = dbLogs.Select(x => x.ToCommonLog()).ToList();
                    res.IsSucceeded = true;
                }
                catch (Exception ex)
                {
                    res.IsSucceeded = false;
                    res.Message = ex.Message;
                    res.ErrorMetadata = ex.StackTrace;
                }
            }

            return res;
        }

        public async Task<ActionResult> AddLog(Common.Log newLog)
        {
            if (newLog == null) return new ActionResult { IsSucceeded = false, Message = "Null object received." };

            var res = new ActionResult();
            using (var db = new DBEntities())
            {
                try
                {
                    #region Check and Add device
                    if(newLog.Device.HasValue)
                    {
                        //if device did not exist
                    }
                    #endregion

                    db.Logs.Add(newLog.ToLog());
                    await db.SaveChangesAsync();
                    res.IsSucceeded = true;
                }
                catch (Exception ex)
                {
                    res.IsSucceeded = false;
                    res.Message = ex.Message;
                    res.ErrorMetadata = ex.StackTrace;
                }
            }

            return res;
        }

        public async Task<ActionResult> AddLogs(IEnumerable<Common.Log> newLogs)
        {
            if (newLogs == null) return new ActionResult { IsSucceeded = false, Message = "Null object received." };

            var res = new ActionResult();
            using (var db = new DBEntities())
            {
                try
                {
                    var logsToInsert = newLogs.Select(x => x.ToLog()).ToList();
                    db.Logs.AddRange(logsToInsert);
                    await db.SaveChangesAsync();
                    res.IsSucceeded = true;
                }
                catch (Exception ex)
                {
                    res.IsSucceeded = false;
                    res.Message = ex.Message;
                    res.ErrorMetadata = ex.StackTrace;
                }
            }

            return res;
        }

        public async Task<ActionResult> ClearLogs()
        {
            var res = new ActionResult();
            using (var db = new DBEntities())
            {
                try
                {
                    await db.Database.ExecuteSqlCommandAsync("TRUNCATE TABLE Log");

                    res.IsSucceeded = true;
                }
                catch (Exception ex)
                {
                    res.IsSucceeded = false;
                    res.Message = ex.Message;
                    res.ErrorMetadata = ex.StackTrace;
                }
            }

            return res;
        }
        #endregion

        #region Device
        public async Task<ResultPack<Common.Device>> GetDevice(Guid deviceId)
        {
            var res = new ResultPack<Common.Device>();
            using (var db = new DBEntities())
            {
                try
                {
                    var dbDevices = await (from d in db.Devices where d.Id.Equals(deviceId) select d).FirstOrDefaultAsync();

                    res.ReturnParam = dbDevices != null ? dbDevices.ToCommonDevice() : null;
                    res.IsSucceeded = true;
                }
                catch (Exception ex)
                {
                    res.IsSucceeded = false;
                    res.Message = ex.Message;
                    res.ErrorMetadata = ex.StackTrace;
                }
            }

            return res;
        }
        public async Task<ResultPack<IEnumerable<Common.Device>>> GetDevices(int pageIndex = 0, int pageSize = 50)
        {
            var res = new ResultPack<IEnumerable<Common.Device>>();
            using (var db = new DBEntities())
            {
                try
                {
                    var dbDevices = await (from d in db.Devices orderby d.RegisterationUtcDate descending select d).Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
                    res.ReturnParam = dbDevices.Select(x => x.ToCommonDevice());
                    res.IsSucceeded = true;
                }
                catch (Exception ex)
                {
                    res.IsSucceeded = false;
                    res.Message = ex.Message;
                    res.ErrorMetadata = ex.StackTrace;
                }
            }

            return res;
        }

        public async Task<ResultPack<IEnumerable<Common.Device>>> GetActiveDevices(DateTime utcLastActivityThreshold, int pageIndex = 0, int pageSize = 50)
        {
            var res = new ResultPack<IEnumerable<Common.Device>>();
            using (var db = new DBEntities())
            {
                try
                {
                    var dbUsers = await (from d in db.Devices
                                         join l in db.Logs on d.Id equals l.Device into joinedLogs
                                         from j in joinedLogs.DefaultIfEmpty()
                                         where j.UtcTime >= utcLastActivityThreshold /*orderby u.RegisterationUtcDate descending*/
                                         select new { device = d, lastActivity = joinedLogs.Max(jt => jt.UtcTime) })
                                         .Distinct().OrderByDescending(x => x.lastActivity).Skip(pageIndex * pageSize)
                                         .Take(pageSize).ToListAsync();

                    res.ReturnParam = dbUsers.Select(x => x.device.ToCommonDevice(x.lastActivity)).ToList();
                    res.IsSucceeded = true;
                }
                catch (Exception ex)
                {
                    res.IsSucceeded = false;
                    res.Message = ex.Message;
                    res.ErrorMetadata = ex.StackTrace;
                }
            }

            return res;
        }

        public async Task<ActionResult> AddDevice(Common.Device newDevice)
        {
            if (newDevice == null) return new ActionResult { IsSucceeded = false, Message = "Null object received." };

            var res = new ActionResult();
            using (var db = new DBEntities())
            {
                try
                {
                    db.Devices.Add(newDevice.ToDevice());
                    await db.SaveChangesAsync();
                    res.IsSucceeded = true;
                }
                catch (Exception ex)
                {
                    res.IsSucceeded = false;
                    res.Message = ex.Message;
                    res.ErrorMetadata = ex.StackTrace;
                }
            }

            return res;
        }
        #endregion

        #region User
        public async Task<ResultPack<Common.User>> GetUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return new ResultPack<Common.User> {
                    IsSucceeded = false,
                    Message = "Username is not provided"
                };

            username = username.ToLower();
            var res = new ResultPack<Common.User>();
            using (var db = new DBEntities())
            {
                try
                {
                    var dbUser = await (from u in db.Users where u.Username.ToLower().Equals(username) select u).FirstOrDefaultAsync();

                    res.ReturnParam = dbUser != null ? dbUser.ToCommonUser() : null;
                    res.IsSucceeded = true;
                }
                catch (Exception ex)
                {
                    res.IsSucceeded = false;
                    res.Message = ex.Message;
                    res.ErrorMetadata = ex.StackTrace;
                }
            }

            return res;
        }

        public async Task<ResultPack<IEnumerable<Common.User>>> GetUsers(int pageIndex = 0, int pageSize = 50)
        {
            var res = new ResultPack<IEnumerable<Common.User>>();
            using (var db = new DBEntities())
            {
                try
                {
                    var dbUsers = await (from d in db.Users orderby d.RegisterationUtcDate descending select d).Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();

                    res.ReturnParam = dbUsers.Select(x => x.ToCommonUser()).ToList();
                    res.IsSucceeded = true;
                }
                catch (Exception ex)
                {
                    res.IsSucceeded = false;
                    res.Message = ex.Message;
                    res.ErrorMetadata = ex.StackTrace;
                }
            }

            return res;
        }

        public async Task<ResultPack<IEnumerable<Common.User>>> GetActiveUsers(DateTime utcLastActivityThreshold, int pageIndex = 0, int pageSize = 50)
        {
            var res = new ResultPack<IEnumerable<Common.User>>();
            using (var db = new DBEntities())
            {
                try
                {
                    var dbUsers = await (from u in db.Users join l in db.Logs on u.Username equals l.Username into joinedLogs
                                         from j in joinedLogs.DefaultIfEmpty()
                                         where j.UtcTime >= utcLastActivityThreshold /*orderby u.RegisterationUtcDate descending*/
                                         select new { user = u, lastActivity = joinedLogs.Max(jt=>jt.UtcTime)})
                                         .Distinct().OrderByDescending(x=>x.lastActivity).Skip(pageIndex * pageSize)
                                         .Take(pageSize).ToListAsync();

                    res.ReturnParam = dbUsers.Select(x => x.user.ToCommonUser(x.lastActivity)).ToList();
                    res.IsSucceeded = true;
                }
                catch (Exception ex)
                {
                    res.IsSucceeded = false;
                    res.Message = ex.Message;
                    res.ErrorMetadata = ex.StackTrace;
                }
            }

            return res;
        }

        public async Task<ActionResult> AddUser(Common.User newUser)
        {
            if (newUser == null) return new ActionResult { IsSucceeded = false, Message = "Null object received." };

            var res = new ActionResult();
            using (var db = new DBEntities())
            {
                try
                {
                    db.Users.Add(newUser.ToUser());
                    await db.SaveChangesAsync();
                    res.IsSucceeded = true;
                }
                catch (Exception ex)
                {
                    res.IsSucceeded = false;
                    res.Message = ex.Message;
                    res.ErrorMetadata = ex.StackTrace;
                }
            }

            return res;
        }
        #endregion

        #region Application
        public async Task<ResultPack<Common.Application>> GetApplication(Guid appId)
        {
            var res = new ResultPack<Common.Application>();
            using (var db = new DBEntities())
            {
                try
                {
                    var dbApp = await (from a in db.Applications where a.Id.Equals(appId) select a).FirstOrDefaultAsync();

                    res.ReturnParam = dbApp != null ? dbApp.ToCommonApplication() : null;
                    res.IsSucceeded = true;
                }
                catch (Exception ex)
                {
                    res.IsSucceeded = false;
                    res.Message = ex.Message;
                    res.ErrorMetadata = ex.StackTrace;
                }
            }

            return res;
        }

        public async Task<ResultPack<IEnumerable<Common.Application>>> GetApplicationList(int pageIndex = 0, int pageSize = 50)
        {
            var res = new ResultPack<IEnumerable<Common.Application>>();
            using (var db = new DBEntities())
            {
                try
                {
                    var dbApps = await (from a in db.Applications orderby a.RegisterationUtcDate descending select a).Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();

                    res.ReturnParam = dbApps.Select(x => x.ToCommonApplication()).ToList();
                    res.IsSucceeded = true;
                }
                catch (Exception ex)
                {
                    res.IsSucceeded = false;
                    res.Message = ex.Message;
                    res.ErrorMetadata = ex.StackTrace;
                }
            }

            return res;
        }

        public async Task<ActionResult> AddApplication(Common.Application newApp)
        {
            if (newApp == null) return new ActionResult { IsSucceeded = false, Message = "Null object received." };

            var res = new ActionResult();
            using (var db = new DBEntities())
            {
                try
                {
                    db.Applications.Add(newApp.ToApplication());
                    await db.SaveChangesAsync();
                    res.IsSucceeded = true;
                }
                catch (Exception ex)
                {
                    res.IsSucceeded = false;
                    res.Message = ex.Message;
                    res.ErrorMetadata = ex.StackTrace;
                }
            }

            return res;
        }
        #endregion
    }
}
