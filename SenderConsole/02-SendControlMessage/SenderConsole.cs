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
            
            SendControlMessage();            

            Console.WriteLine("Sender Console - Complete");
            Console.ReadLine();
        }        

        private static void SendControlMessage()
        {
            // Create a message with no body
            var message = new BrokeredMessage()
            {
                Label = "Control"
            };

            // Add some properties to the property collection
            message.Properties.Add("SystemId", 1462);
            message.Properties.Add("Command", "Pending Restart");
            message.Properties.Add("ActionTime", DateTime.UtcNow.AddHours(2));

            // Send the message
            var client = QueueClient.CreateFromConnectionString
                (Settings.ConnectionString, Settings.QueueName);
            Console.Write("Sending control message...");
            client.Send(message);
            Console.WriteLine("Done!");

            Console.WriteLine("Send again?");
            var response = Console.ReadLine();

            if (response.ToLower().StartsWith("y"))
            {
                // Try to send the message a second time...
                Console.Write("Sending control message again...");
                message = message.Clone();
                client.Send(message);
                Console.WriteLine("Done!");
            }
            client.Close();
        }        
    }
}
