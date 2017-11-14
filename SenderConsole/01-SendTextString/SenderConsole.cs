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

            SendTextString(Settings.TextString, true);
            SendTextString(Settings.TextString, false);            

            Console.WriteLine("Sender Console - Complete");
            Console.ReadLine();
        }        

        static void SendTextString(string text, bool sendSync)
        {
            // Create a client
            var client = QueueClient.CreateFromConnectionString
                (Settings.ConnectionString, Settings.QueueName);

            Console.Write("Sending: ");

            var taskList = new List<Task>();

            foreach (var letter in text.ToCharArray())
            {
                // Create an empty message and set the label.
                var message = new BrokeredMessage();
                message.Label = letter.ToString();

                if (sendSync)
                {
                    // Send the message
                    client.Send(message);
                    Console.Write(message.Label);
                }
                else
                {
                    // Create a task to send the message
                    //var sendTask = new Task(() =>
                    //{
                    //    client.Send(message);
                    //    Console.Write(message.Label);
                    //});
                    //sendTask.Start();
                    taskList.Add(client.SendAsync(message).ContinueWith
                        (t => Console.WriteLine("Sent: " + message.Label)));
                }
            }

            if (!sendSync)
            {
                Console.WriteLine("Waiting...");
                Task.WaitAll(taskList.ToArray());
                Console.WriteLine("Complete!");
            }

            Console.ReadLine();
            Console.WriteLine();

            // Always close the client
            client.Close();
        }
    }
}
