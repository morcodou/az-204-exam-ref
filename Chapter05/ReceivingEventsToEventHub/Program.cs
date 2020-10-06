using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System;

namespace ReceivingEventsToEventHub
{
    class Program
    {
        private const string EventHubConnectionString =
                             "<your_event_hub_namespace_connection_string>";
        private const string EventHubName = "<your_event_hub_name>";
        private const string StorageContainerName = "<your_container_name>";
        private const string StorageAccountName = "<your_storage_account_name>";
        private const string StorageAccountKey = "<your_storage_account_access_key>";
        private static readonly string StorageConnectionString = string.Format($"DefaultEndpointsProtocol=https;AccountName ={StorageAccountName};AccountKey={StorageAccountKey}");

        static void Main(string[] args)
        {
            Console.WriteLine("Registering EventProcessor...");

            var eventProcessorHost = new EventProcessorHost(
                EventHubName,
                PartitionReceiver.DefaultConsumerGroupName,
                EventHubConnectionString,
                StorageConnectionString,
                StorageContainerName);

            // Registers the Event Processor Host and starts receiving messages
            eventProcessorHost.RegisterEventProcessorAsync<SimpleEventProcessor>();

            Console.WriteLine("Receiving. Press ENTER to stop worker.");
            Console.ReadLine();

            // Disposes of the Event Processor Host
            eventProcessorHost.UnregisterEventProcessorAsync();
        }
    }
}