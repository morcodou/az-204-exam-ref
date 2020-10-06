using Microsoft.Azure.ServiceBus;
using System;
using System.Text;

namespace PublishMessagesToServiceBusTopic
{
    class Program
    {
        const string ServiceBusConnectionString =
                      "<your_service_bus_connection_string>";
        const string TopicName = "<your_topic_name>";
        const int numberOfMessagesToSend = 100;

        static ITopicClient topicClient;

        static void Main(string[] args)
        {
            topicClient = new TopicClient(ServiceBusConnectionString, TopicName);

            Console.WriteLine("Press ENTER key to exit after sending all the messages.");
            Console.WriteLine();

            // Send messages.
            try
            {
                for (var i = 0; i < numberOfMessagesToSend; i++)
                {
                    // Create a new message to send to the topic.
                    string messageBody = $"Message {i} {DateTime.Now}";
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                    // Write the body of the message to the console.
                    Console.WriteLine($"Sending message: {messageBody}");

                    // Send the message to the topic.
                    topicClient.SendAsync(message);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
            Console.ReadKey();

            topicClient.CloseAsync();
        }
    }
}