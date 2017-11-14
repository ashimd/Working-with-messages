using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Threading;
using WorkingWithMessages.Config;
using WorkingWithMessages.DataContracts;

namespace WorkingWithMessages.Receiver
{
    class ReceiverConsole
    {
        static QueueClient m_QueueClient;

        static void Main(string[] args)
        {
            Console.WriteLine("Press enter to receive.");
            Console.ReadLine();

            CreateQueue();
            ReceiveAndProcess();
            
            Console.WriteLine("Receiving, hit enter to exit.");
            Console.ReadLine();
            StopReceiving();
        }
		
		private static void CreateQueue()
        {
            var manager = NamespaceManager.CreateFromConnectionString
			(Settings.ConnectionString);
			
            if (!manager.QueueExists(Settings.QueueName))
            {
                Console.WriteLine("Creating queue: " + Settings.QueueName + ".....");
                manager.CreateQueue(Settings.QueueName);
                Console.WriteLine("Done!");
            }
        }

        private static void ReceiveAndProcess()
        {
            // Create a queue client
            m_QueueClient = QueueClient.CreateFromConnectionString
                (Settings.ConnectionString, Settings.QueueName);

            // Create a message pump
            m_QueueClient.OnMessage(message =>
            {
                Console.Write("Received: " + message.Label);
                message.Complete();
            });
        }

        private static void StopReceiving()
        {
            // Close the client, which will stop the message pump.
            m_QueueClient.Close();            
        }
    }
}
