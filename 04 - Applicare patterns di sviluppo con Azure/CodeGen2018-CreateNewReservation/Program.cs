using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using static System.Console;

namespace CodeGen2018.CreateNewReservation
{
    static partial class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                 .Build();

            var client = new QueueClient(config["ServiceBusQueue:ConnectionString"], config["ServiceBusQueue:EntityName"]);

            var reservationObject = new
            {
                UserId = "myself",
                RoomId = "R1",
                StartDate = DateTime.Today.AddHours(10),
                Length = 1.5M,
                Title = "Meeting A"
            };
            var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(reservationObject)));
            await client.SendAsync(message);

            WriteLine($"Reservation sent");
        }
    }
}
