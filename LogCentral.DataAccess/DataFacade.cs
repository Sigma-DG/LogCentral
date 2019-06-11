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

        public async Task<ResultPack<IEnumerable<Common.Log>>> GetLogs(int pageIndex = 0, int pageSize = 50)
        {
            var res = new ResultPack<IEnumerable<Common.Log>>();
            using (var db = new DBEntities())
            {
                try
                {
                    var dbLogs = await(from d in db.Logs orderby d.UtcTime descending select d).Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();

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

        public async Task<ResultPack<IEnumerable<Common.Device>>> GetDevices(int pageIndex = 0, int pageSize = 50)
        {
            var res = new ResultPack<IEnumerable<Common.Device>>();
            using (var db = new DBEntities())
            {
                try
                {
                    res.ReturnParam = await (from d in db.Devices orderby d.RegisterationUtcDate descending select d).Skip(pageIndex * pageSize).Take(pageSize).Select(x => x.ToCommonDevice()).ToListAsync();
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
    }
}
