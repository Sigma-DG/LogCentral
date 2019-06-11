using LogCentral.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace LogCentral.Tests.WebApiTest
{
    class Program
    {

        static async Task DeleteAllLogs()
        {
            var delClient = new HttpClient();
            delClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var delRes = await delClient.DeleteAsync("http://localhost/LogCentral.WebApi/api/logs");
            if (!delRes.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error calling WebApi method to delete logs:\n{delRes.ReasonPhrase}\n{delRes.RequestMessage}");
                Console.ReadKey(true);
                return;
            }
            var strDelData = await delRes.Content.ReadAsStringAsync();
            var delResData = JsonConvert.DeserializeObject<ResultPack<IEnumerable<Log>>>(strDelData);
            if (!delResData.IsSucceeded)
            {
                Console.WriteLine($"Error deleting logs.\n{delResData.Message}\n{delResData.ErrorMetadata}\nPress any key to exit");
                Console.ReadKey(true);
                return;
            }
        }

        static async Task AddTenItemsInTenThreads()
        {
            var tasks = new List<Task>();
            for (int ti = 0; ti < 100; ti++)
            {
                tasks.Add(new Task(()=> {
                    for (int i = 0; i < 5; i++)
                    {
                        var addClient = new HttpClient();
                        addClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var logToAdd = new Log
                        {
                            Title = $"TestLog{i}",
                            Section = "LogCentral.Tests.TestDataConsole.Program.Main()",
                            Descriptions = "Few descriptions"
                        };
                        var content = new StringContent(JsonConvert.SerializeObject(logToAdd), System.Text.Encoding.UTF8, "application/json");
                        var addRes = addClient.PutAsync("http://localhost/LogCentral.WebApi/api/logs", content).Result;
                        if (!addRes.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"Error calling WebApi method to add logs:\n{addRes.ReasonPhrase}\n{addRes.RequestMessage}");
                            Console.ReadKey(true);
                            return;
                        }
                        var strAddData = addRes.Content.ReadAsStringAsync().Result;
                        var addResData = JsonConvert.DeserializeObject<ResultPack<IEnumerable<Log>>>(strAddData);
                        if (!addResData.IsSucceeded)
                        {
                            Console.WriteLine($"Error adding logs.\n{addResData.Message}\n{addResData.ErrorMetadata}\nPress any key to exit");
                            Console.ReadKey(true);
                            return;
                        }
                        Console.WriteLine($"- Log added at :{DateTime.Now.ToString("HH:mm:ss ffff")}");
                    }
                }));
            }
            tasks.ForEach(t => t.Start());
            await Task.WhenAll(tasks);
        }

        static void Main(string[] args)
        {
            Console.Write("Press any key to start..");
            //Console.ReadKey(true);
            Console.Clear();

            Task.Run(async () => {
                await AddTenItemsInTenThreads();

                var c = new HttpClient();
                var res = await c.GetAsync("http://localhost/LogCentral.WebApi/api/logs?pageIndex=0&pageSize=500");
                if (!res.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error calling WebApi method:\n{res.ReasonPhrase}\n{res.RequestMessage}");
                    Console.ReadKey(true);
                    return;
                }
                var strData = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<IEnumerable<Log>>(strData);

                await DeleteAllLogs();
                //if (!data.IsSucceeded)
                //{
                //    Console.WriteLine($"Error getting logs.\n{data.Message}\n{data.ErrorMetadata}\nPress any key to exit");
                //    Console.ReadKey(true);
                //    return;
                //}
                Console.WriteLine($"Found {data.Count()} items and removed them.\nPress any key to exit");
            });

            
            Console.ReadKey(true);
        }
    }
}
