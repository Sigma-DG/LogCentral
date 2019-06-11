using System;
using System.Collections.Generic;
using System.Text;

namespace LogCentral.Common
{
    public class Device
    {
        public Device()
        {
            Id = Guid.NewGuid();
            RegisterationUtcDate = DateTime.UtcNow;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string OwnerName { get; set; }
        public Nullable<byte> Platform { get; set; }
        public DateTime RegisterationUtcDate { get; set; }
        public string Descriptions { get; set; }
    }
}
