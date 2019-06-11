using System;
using System.Collections.Generic;
using System.Text;

namespace LogCentral.Common
{
    public class ActionResult
    {
        public ActionResult()
        {
            CreationDateUtc = DateTime.UtcNow;
        }

        public bool IsSucceeded { get; set; }

        public DateTime CreationDateUtc { get; set; }

        public string Message { get; set; }

        public string ErrorMetadata { get; set; }
    }
}
