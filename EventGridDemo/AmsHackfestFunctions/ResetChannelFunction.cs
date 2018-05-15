using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using PusherEventGrid;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace AmsHackfestFunctions
{
    public static class ResetChannelFunction
    {
        [FunctionName("ResetChannelFunction")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req,
            TraceWriter log)
        {
            log.Info("ResetChannel event received.");

            try
            {
                var payloadFromEventGrid = JToken.ReadFrom(new JsonTextReader(new StreamReader(await req.Content.ReadAsStreamAsync())));
                log.Equals($"EventGridMessage paylog : {payloadFromEventGrid.ToString()}");

                dynamic eventGridSoleItem = (payloadFromEventGrid as JArray)?.SingleOrDefault();
                if (eventGridSoleItem == null)
                {
                    return req.CreateErrorResponse(HttpStatusCode.BadRequest, $@"Expecting only one item in the Event Grid message");
                }

                if (eventGridSoleItem.eventType == @"Microsoft.EventGrid.SubscriptionValidationEvent")
                {
                    log.Info("handling EventGrid system SubscriptionValidationEvent.");

                    log.Info(@"Event Grid Validation event received.");
                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new
                        {
                            validationResponse = ((dynamic)payloadFromEventGrid)[0].data.validationCode
                        }))
                    };
                }

                // Get request body
                log.Info("handling ResetChannel event.");
                var resetMsgs = JsonConvert.DeserializeObject<List<CustomEvent<ResetChannelEvent>>>(payloadFromEventGrid.ToString()) as List<CustomEvent<ResetChannelEvent>>;

                if (resetMsgs.Count > 0)
                {
                    log.Info($"RESETTING : pid=[{resetMsgs[0].Data.ChannelPID}]   name=[{resetMsgs[0].Data.ChannelName}]   endpoint=[{resetMsgs[0].Data.AmsEdpoint}]");
                    //TODO : add the code to reset the AMS channel
                }
                else
                {
                    log.Info("Event but NO messages inside.");
                }


            }
            catch(Exception ex)
            {
                log.Error(ex.ToString(), ex, "ResetChannelFunction");
            }
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    status = "good"
                }))
            };
        }
    }
}
