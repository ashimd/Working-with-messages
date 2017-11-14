using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkingWithMessages.Config;
using WorkingWithMessages.DataContracts;

namespace WorkingWithMessages.Sender
{
    class SenderConsole
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sender Console - Hit enter");
            Console.ReadLine();
			
			      SendPizzaOrderBatch();       

            Console.WriteLine("Sender Console - Complete");
            Console.ReadLine();
        }        

        private static void SendPizzaOrderBatch()
        {
            // Create Some Data
            string[] names = { "Alan", "Jennifer", "James" };
            string[] pizzas = { "Hawaiian", "Vegitarian", "Capricciosa" };

            // Create a queue client
            var client = QueueClient.CreateFromConnectionString
                (Settings.ConnectionString, Settings.QueueName);

            // Send a batch of pizza orders
            var taskList = new List<Task>();
            for (int pizza = 0; pizza < pizzas.Length; pizza++)
            {
                for (int name = 0; name < names.Length; name++)
                {
                    PizzaOrder order = new PizzaOrder()
                    {
                        CustomerName = names[name],
                        Type = pizzas[pizza],
                        Size = "Large"
                    };
                    var message = new BrokeredMessage(order);
                    taskList.Add(client.SendAsync(message));
                }
            }
            Console.WriteLine("Sending Batch...");
            Task.WaitAll(taskList.ToArray());
            Console.WriteLine("Sent!");
        }  
    }
}
