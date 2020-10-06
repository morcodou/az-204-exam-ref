// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace SubscriberToEventGridTopic
{
    public static class Function1
    {
        [FunctionName("EventGridTrigger")]
        public static void Run(
            [EventGridTrigger] EventGridEvent eventGridEvent,
            ILogger logger)
        {
            logger.LogInformation("C# Event Grid trriger handling EventGrid Events.");

            logger.LogInformation($"New event received: {eventGridEvent.Data}");


            if (eventGridEvent.Data is StorageBlobCreatedEventData)
            {
                var eventData = (StorageBlobCreatedEventData)eventGridEvent.Data;
                logger.LogInformation($"Got BlobCreated event data, blob URI {eventData.Url}");
            }
            else if (eventGridEvent.EventType.Equals("MyCompany.Items.NewItemCreated"))
            {
                NewItemCreatedEventData eventData = ((JObject)eventGridEvent.Data).ToObject<NewItemCreatedEventData>();
                logger.LogInformation($"New Item Custom Event, Name {eventData.itemName}");
            }
        }
    }
}