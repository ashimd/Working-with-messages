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
            SimplePizzaReceiveLoop();
            
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

        private static void SimplePizzaReceiveLoop()
        {
            // Create a queue client
            QueueClient client = QueueClient.CreateFromConnectionString
                (Settings.ConnectionString, Settings.QueueName);

            while (true)
            {
                Console.WriteLine("Receiving...");

                // Receive a message
                BrokeredMessage message = client.Receive(TimeSpan.FromSeconds(5));

                if (message != null)
                {
                    try
                    {
                        Console.WriteLine("Received: " + message.Label);

                        // Process the message
                        PizzaOrder order = message.GetBody<PizzaOrder>();

                        // Process the message
                        CookPizza(order);

                        // Mark this message as complete
                        message.Complete();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception: " + ex.Message);

                        // Abandon the message
                        message.Abandon();

                        // Deadletter the message
                        //message.DeadLetter();

                        // Or do nothing
                    }
                }
                else
                {
                    Console.WriteLine("No message present in queue.");
                }
            }
        }

        private static void StopReceiving()
        {
            // Close the client, which will stop the message pump.
            m_QueueClient.Close();            
        }
    }
}
