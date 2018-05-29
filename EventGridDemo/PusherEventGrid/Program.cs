using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PusherEventGrid
{
    class Program
    {
        static void Main(string[] args)
        {
            const int defaultMsgCount=5; 
            var nbMsg = defaultMsgCount;
            if (args.Length > 0)
            {
                if (!int.TryParse(args[0], out nbMsg))
                {
                    nbMsg = defaultMsgCount;
                    Console.WriteLine($"Warning : invalid integer value '{args[0]}'. Using default value '{nbMsg}'");
                }
            }
            for (int n=1;n<50;n++)
            {
                var eventNew = MakeRequestEvent(n.ToString());
                eventNew.Wait();
                Console.WriteLine(eventNew.Result.Content.ReadAsStringAsync().Result);
                Task.Delay(1000).Wait();
            }
            Console.ReadKey();
        }

        private static async Task<HttpResponseMessage> MakeRequestEvent(string channelPid)
        {
            // TODO : replace using the .NEt SDK
            //string endpoint = "REPLACE_BY_URL_OF_YOUR_EVENTGRIDTOPIC_ENDPOINT";
            string endpoint = endpoint = "https://amsresetchanneltopic.westeurope-1.eventgrid.azure.net/api/events";

            var httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Add("aeg-sas-key", "REPLACE_BY_YOUR_EVENTGRIDTOPIC_KEY");
            httpClient.DefaultRequestHeaders.Add("aeg-sas-key", "RpfDX+hA3nEOGzDfsswRAXnojLpEWJ0KyvCp0LTGO3Q=");


            List<CustomEvent<ResetChannelEvent>> events = new List<CustomEvent<ResetChannelEvent>>();


            var customEvent = new CustomEvent<ResetChannelEvent>();
            customEvent.EventType = "ResetChannelType";

            customEvent.Subject = "Test";
            customEvent.Data = new ResetChannelEvent() { ChannelPID= channelPid, ChannelName=$"TF{channelPid}",AmsEdpoint=$"ams://toto.com/pid/{channelPid}" };

            events.Add(customEvent);
            string jsonContent = JsonConvert.SerializeObject(events);

            Console.WriteLine(jsonContent);

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            return await httpClient.PostAsync(endpoint, content);
        }
    }
}
