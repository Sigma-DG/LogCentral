using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogCentral.Tests.DataAccessTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Common.ActionResult r = null;
            for (int i = 0; i < 10; i++)
            {
                r = DataAccess.DataFacade.Current.AddLog(new Common.Log
                {
                    Title = $"TestLog{i}",
                    Section = "LogCentral.Tests.TestDataConsole.Program.Main()",
                    Descriptions = "Few descriptions"
                }).Result;
                if(!r.IsSucceeded)
                {
                    Console.WriteLine($"Error adding logs.\n{r.Message}\n{r.ErrorMetadata}\nPress any key to exit");
                    Console.ReadKey(true);
                    return;
                }
            }

            var l = DataAccess.DataFacade.Current.GetLogs().Result;

            DataAccess.DataFacade.Current.ClearLogs().Wait();

            Console.WriteLine($"Found {l.ReturnParam.Count()} items and removed them.\nPress any key to exit");
            Console.ReadKey(true);
        }
    }
}
