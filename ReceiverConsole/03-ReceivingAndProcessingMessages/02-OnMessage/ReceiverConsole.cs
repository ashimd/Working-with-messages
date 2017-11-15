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
            ReceiveAndProcessPizzaOrdersUsingOnMessage(1);
	    ReceiveAndProcessPizzaOrdersUsingOnMessage(5);
	    ReceiveAndProcessPizzaOrdersUsingOnMessage(100);
            
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

        private static void ReceiveAndProcessPizzaOrdersUsingOnMessage(int threads)
        {
            // Create a new client
            m_QueueClient = QueueClient.CreateFromConnectionString
                (Settings.ConnectionString, Settings.QueueName);

            // Set the options for using OnMessage
            var options = new OnMessageOptions()
            {
                AutoComplete = false,
                MaxConcurrentCalls = threads,
                AutoRenewTimeout = TimeSpan.FromSeconds(30)
            };

            // Create a message pump using OnMessage
            m_QueueClient.OnMessage(message =>
            {
                // Deserialize the message body
                var order = message.GetBody<PizzaOrder>();

                // Process the message
                CookPizza(order);

                // Complete the message
                message.Complete();
            }, options);

            Console.WriteLine("Receiving, hit enter to exit");
            Console.ReadLine();            
        }
	    
	private static void CookPizza(PizzaOrder order)
        {
            Console.WriteLine("Cooking {0} for {1}.", order.Type, order.CustomerName);
            Thread.Sleep(5000);
            Console.WriteLine("     {0} pizza for {1}.", order.Type, order.CustomerName);
        }

        private static void StopReceiving()
        {
            // Close the client, which will stop the message pump.
            m_QueueClient.Close();            
        }
    }
}
