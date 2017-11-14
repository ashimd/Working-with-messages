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
            
            SendPizzaOrder();            

            Console.WriteLine("Sender Console - Complete");
            Console.ReadLine();
        }        

        private static void SendPizzaOrder()
        {
            var order = new PizzaOrder()
            {
                CustomerName = "Alan Smith",
                Type = "Hawaiian",
                Size = "Large"
            };

            // Create a brokered message
            var message = new BrokeredMessage(order)
            {
                Label = "PizzaOrder"
            };

            // What size is the message?
            Console.WriteLine("Message Size: " + message.Size);

            // Send the message...
            var client = QueueClient.CreateFromConnectionString
                (Settings.ConnectionString, Settings.QueueName);
            Console.Write("Sending order...");
            client.Send(message);
            Console.WriteLine("Done!");
            client.Close();

            // What size is the message now?
            Console.WriteLine("Message size: " + message.Size);
        }       
    }
}
