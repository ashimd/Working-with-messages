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
            ProcessOrderMessages();
            
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

        private static void ProcessOrderMessages()
        {
            var client = QueueClient.CreateFromConnectionString
                (Settings.ConnectionString, Settings.QueueName);

            while (true)
            {
                var orderMessage = client.Receive();

                if (orderMessage != null)
                {
                    Console.WriteLine("Received message");

                    // Verify that the message contains JSON
                    if (orderMessage.ContentType.Equals(("application/json")))
                    {
                        // Deserialize the message body to a string
                        string content = orderMessage.GetBody<string>();
                        Console.WriteLine("Message content: " + content);
                        Console.WriteLine("Message label: " + orderMessage.Label);
                        Console.WriteLine();

                        // Check the message is a Pizza order
                        if (orderMessage.Label.Equals("JsonSerialization.Sender.PizzaOrder"))
                        {
                            Console.WriteLine("Order details:");

                            // Deserialize the JSON string to a dynamic type
                            dynamic order = JsonConvert.DeserializeObject(content);
                            Console.WriteLine("\t" + order.CustomerName);
                            Console.WriteLine("\t" + order.Type);
                            Console.WriteLine("\t" + order.Size);
                        }
                    }
                    orderMessage.Complete();
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
