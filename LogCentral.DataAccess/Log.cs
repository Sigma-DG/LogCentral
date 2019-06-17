//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LogCentral.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Log
    {
        public System.Guid Id { get; set; }
        public System.DateTime UtcTime { get; set; }
        public System.DateTimeOffset LocalTime { get; set; }
        public string Title { get; set; }
        public string Section { get; set; }
        public byte LogType { get; set; }
        public Nullable<double> Latitude { get; set; }
        public Nullable<double> Longitude { get; set; }
        public string Username { get; set; }
        public Nullable<System.Guid> Device { get; set; }
        public Nullable<System.Guid> Application { get; set; }
        public string Descriptions { get; set; }
    
        public virtual Application Application1 { get; set; }
        public virtual Device Device1 { get; set; }
        public virtual User User { get; set; }
    }
}
