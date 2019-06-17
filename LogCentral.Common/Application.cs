using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogCentral.Common
{
    public class Application
    {
        public Application()
        {
            Id = Guid.NewGuid();
            RegisterationUtcDate = DateTime.UtcNow;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AppStoreIdentifier { get; set; }
        public DateTime RegisterationUtcDate { get; set; }
        public string Description { get; set; }
    }
}
