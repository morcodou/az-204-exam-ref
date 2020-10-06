using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System;

namespace AddMessagesToQueueStorage
{
    class Program
    {
        private const string connectionString = "<your_storage_account_connection_string>";
        private const string queueName = "az204queue";
        private const int maxNumOfMessages = 10;
        static void Main(string[] args)
        {
            QueueClient queueClient = new QueueClient(connectionString, queueName);

            //Create the queue
            queueClient.CreateIfNotExists();

            //Sending messages to the queue.
            for (int i = 0; i < maxNumOfMessages; i++)
            {
                queueClient.SendMessageAsync($"Message {i} {DateTime.Now}");
            }

            //Getting the length of the queue
            QueueProperties queueProperties = queueClient.GetProperties();
            int? cachedMessageCount = queueProperties.ApproximateMessagesCount;

            //Reading messages from the queue without removing the message
            Console.WriteLine("Reading message from the queue without removing them from the queue");
            PeekedMessage[] peekedMessages = queueClient.PeekMessages((int)cachedMessageCount);

            foreach (PeekedMessage peekedMessage in peekedMessages)
            {
                Console.WriteLine($"Message read from the queue: {peekedMessage.MessageText}");

                //Getting the length of the queue
                queueProperties = queueClient.GetProperties();
                int? queueLenght = queueProperties.ApproximateMessagesCount;
                Console.WriteLine($"Current lenght of the queue {queueLenght}");
            }

            //Reading messages removing it from the queue
            Console.WriteLine("Reading message from the queue removing");
            QueueMessage[] messages = queueClient.ReceiveMessages((int)cachedMessageCount);
            foreach (QueueMessage message in messages)
            {
                Console.WriteLine($"Message read from the queue: {message.MessageText}");
                //You need to process the message in less than 30 seconds.
                queueClient.DeleteMessage(message.MessageId, message.PopReceipt);

                //Getting the length of the queue
                queueProperties = queueClient.GetProperties();
                int? queueLenght = queueProperties.ApproximateMessagesCount;
                Console.WriteLine($"Current lenght of the queue {queueLenght}");
            }
        }
    }
}