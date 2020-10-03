using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ImplementAzureFunctions
{
    public static class AFBlobTrigger
    {
        [FunctionName("AFBlobTrigger")]
        public static Task Run(
                [EventGridTrigger] EventGridEvent eventGridEvent,
                [Blob("{data.url}", FileAccess.Read, Connection = "ImagesBlobStorage")] Stream blobStream,
                [CosmosDB(
                databaseName: "GIS",
                collectionName: "Processed_images",
                ConnectionStringSetting = "CosmosDBConnection")] out dynamic document,
                [SignalR(HubName = "notifications")] IAsyncCollector<SignalRMessage> message,
                ILogger logger)
        {
            document = new { Description = eventGridEvent.Topic, id = Guid.NewGuid() };

            logger.LogInformation($"C# Blob trigger function Processed event\n Topic: {eventGridEvent.Topic} \n Subject: {eventGridEvent.Subject} ");
            return message.AddAsync(
            new SignalRMessage
            {
                Target = "newMessage",
                Arguments = new[] { eventGridEvent.Subject }
            });
        }
    }
}
