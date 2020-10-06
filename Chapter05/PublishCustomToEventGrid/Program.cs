using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace PublishCustomToEventGrid
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            IConfigurationRoot configuration = builder.Build();

            string topicEndpoint = configuration["EventGridTopicEndpoint"];
            string apiKey = configuration["EventGridAccessKey"];

            string topicHostname = new Uri(topicEndpoint).Host;
            TopicCredentials topicCredentials = new TopicCredentials(apiKey);
            EventGridClient client = new EventGridClient(topicCredentials);

            List<EventGridEvent> events = new List<EventGridEvent>();
            events.Add(new EventGridEvent()
            {
                Id = Guid.NewGuid().ToString(),
                EventType = "MyCompany.Items.NewItemCreated",
                Data = new NewItemCreatedEvent()
                {
                    itemName = "Item 1"
                },
                EventTime = DateTime.Now,
                Subject = "Store A",
                DataVersion = "3.7"
            });

            client.PublishEventsAsync(topicHostname, events).GetAwaiter().GetResult();
            Console.WriteLine("Events published to the Event Grid Topic");
            Console.ReadLine();
        }
    }
}
