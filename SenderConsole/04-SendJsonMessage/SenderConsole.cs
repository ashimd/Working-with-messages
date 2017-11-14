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
            
            // Create a new pizza order
            var order = new PizzaOrderUnserialized()
            {
                CustomerName = "Alan",
                Type = "Kebab",
                Size = "Extra Large"
            };
			
			SendJsonMessage(order);            

            Console.WriteLine("Sender Console - Complete");
            Console.ReadLine();
        }        

         private static void SendJsonMessage(object content)
        {
            // Serailize the order to a JSON string
            string json = JsonConvert.SerializeObject(content);

            Console.WriteLine("Json Content: " + json);

            // Create a new brokered message using the JSON object as the body
            var message = new BrokeredMessage(json);

            // Set the content type and label to the object type\
            message.ContentType = "application/json";
            message.Label = content.GetType().ToString();

            Console.WriteLine(message.Label);

            Console.WriteLine("Sending message...");
            var client = QueueClient.CreateFromConnectionString
                (Settings.ConnectionString, Settings.QueueName);
            client.Send(message);
            client.Close();
            Console.WriteLine("Done!");

            Console.WriteLine("Order sent!");
        }    
    }
	
	class PizzaOrder
    {
        public string CustomerName { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
    }
}
