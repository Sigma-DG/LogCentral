using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogCentral.DataAccess
{
    internal static class Extentions
    {
        public static DataAccess.Log ToLog(this Common.Log log)
        {
            if (log == null) return null;

            return new DataAccess.Log
            {
                Id = log.Id,
                Descriptions = log.Descriptions,
                Device = log.Device,
                Latitude = log.Latitude,
                Longitude = log.Longitude,
                LocalTime = log.LocalTime,
                UtcTime = log.UtcTime,
                LogType = log.LogType,
                Section = log.Section,
                Title = log.Title,
                Username = log.Username,
            };
        }

        public static Common.Log ToCommonLog(this DataAccess.Log log)
        {
            if (log == null) return null;

            return new Common.Log
            {
                Id = log.Id,
                Descriptions = log.Descriptions,
                Device = log.Device,
                Latitude = log.Latitude,
                Longitude = log.Longitude,
                LocalTime = log.LocalTime,
                UtcTime = log.UtcTime,
                LogType = log.LogType,
                Section = log.Section,
                Title = log.Title,
                Username = log.Username
            };
        }

        public static Common.Device ToCommonDevice(this DataAccess.Device device)
        {
            if (device == null) return null;

            return new Common.Device
            {
                Id = device.Id,
                Descriptions = device.Descriptions,
                Name = device.Name,
                OwnerName = device.OwnerName,
                Platform = device.Platform,
                RegisterationUtcDate = device.RegisterationUtcDate,
            };
        }

        public static Common.User ToCommonUser(this DataAccess.User user)
        {
            if (user == null) return null;

            return new Common.User
            {
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Descriptions = user.Descriptions,
                RegisterationUtcDate = user.RegisterationUtcDate,
            };
        }
    }
}
