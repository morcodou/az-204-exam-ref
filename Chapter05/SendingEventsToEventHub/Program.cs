using Microsoft.Azure.EventHubs;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SendingEventsToEventHub
{
    class Program
    {
        private static EventHubClient eventHubClient;
        private const string EventHubConnectionString = "<Your_event_hub_namespace_connection_string>";
        private const string EventHubName = "<your_event_hub_name>";
        private const int numMessagesToSend = 100;

        static void Main(string[] args)
        {
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(
            EventHubConnectionString)
            {
                EntityPath = EventHubName
            };

            eventHubClient = EventHubClient.CreateFromConnectionString(
            connectionStringBuilder.ToString());

            for (var i = 0; i < numMessagesToSend; i++)
            {
                try
                {
                    var message = $"Message {i}";
                    Console.WriteLine($"Sending message: {message}");
                    eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
                }

                Task.Delay(10);
            }

            Console.WriteLine($"{numMessagesToSend} messages sent.");

            eventHubClient.CloseAsync();

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }
    }
}
