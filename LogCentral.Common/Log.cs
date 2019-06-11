using System;
using System.Collections.Generic;
using System.Text;

namespace LogCentral.Common
{
    public class Log
    {
        public Log()
        {
            Id = Guid.NewGuid();
            UtcTime = DateTime.UtcNow;
            LocalTime = DateTimeOffset.Now;
        }
        public Guid Id { get; set; }
        public DateTime UtcTime { get; set; }
        public DateTimeOffset LocalTime { get; set; }
        public string Title { get; set; }
        public string Section { get; set; }
        public byte LogType { get; set; }
        public Nullable<double> Latitude { get; set; }
        public Nullable<double> Longitude { get; set; }
        public string Username { get; set; }
        public Nullable<Guid> Device { get; set; }
        public string Descriptions { get; set; }
    }
}
