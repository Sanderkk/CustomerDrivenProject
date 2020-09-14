using System;
using System.Configuration;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models; // Namespace for ConfigurationManager
// Namespace for Task
// Namespace for Queue storage types

// Namespace for PeekedMessage

namespace src.QueueStorage
{
    class QueueStorage
    {
        private readonly string connectionString;
        private QueueClient queueClient;

        public QueueStorage()
        {
            // Get the connection string from app settings
            connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];
            Console.WriteLine("Connection: ", connectionString);

            // Instantiate a QueueClient which will be used to create and manipulate the queue
            queueClient = new QueueClient(connectionString, "myqueue");
        }


        public void CreateQueue()
        {
            // Create the queue
            queueClient.CreateIfNotExists();
        }

        public void SendMessage(string message)
        {
            if (queueClient.Exists())
            {
                // Send a message to the queue
                queueClient.SendMessage(message);
            }
        }

        public void PeekMessage()
        {
            if (queueClient.Exists())
            {
                // Peek at the next message
                PeekedMessage[] peekedMessage = queueClient.PeekMessages();

                // Display the message
                Console.WriteLine($"Peeked message: '{peekedMessage[0].MessageText}'");
            }
        }

        public void ChangeContentOfMessage(string content)
        {
            if (queueClient.Exists())
            {
                // Get the message from the queue
                QueueMessage[] message = queueClient.ReceiveMessages();

                // Update the message contents
                queueClient.UpdateMessage(message[0].MessageId,
                    message[0].PopReceipt,
                    content,
                    TimeSpan.FromSeconds(60.0) // Make it invisible for another 60 seconds
                );
            }
        }

        public void DequeueMessage()
        {
            if (queueClient.Exists())
            {
                // Get the next message
                QueueMessage[] retrievedMessage = queueClient.ReceiveMessages();

                // Process (i.e. print) the message in less than 30 seconds
                Console.WriteLine($"De-queued message: '{retrievedMessage[0].MessageText}'");

                // Delete the message
                queueClient.DeleteMessage(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
            }
        }
        
        
        public void test(string[] args)
        {
            Console.WriteLine("Hello World!");
            QueueStorage queue = new QueueStorage();
            queue.CreateQueue();
            queue.SendMessage("Test string");

        }
    }
}
