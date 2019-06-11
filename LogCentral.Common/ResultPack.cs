using System;
using System.Collections.Generic;
using System.Text;

namespace LogCentral.Common
{
    public class ResultPack<T> : ActionResult
    {
        public ResultPack() : base()
        {
            ReturnParam = default(T);
        }

        public T ReturnParam { get; set; }
    }
}
