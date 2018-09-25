using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using static System.Console;

namespace CodeGen2018.HandleNewReservationRequests
{
    static partial class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                 .Build();

            //var documentClient = new DocumentClient(new Uri(config["CosmosDb:ServiceUri"]), config["CosmosDb:Key"]);
            //var database = documentClient.CreateDatabaseQuery().ToList().SingleOrDefault(xx => xx.Id == id);
            //var documentCollection = documentClient.CreateDocumentCollectionQuery(database.SelfLink).ToList().SingleOrDefault(xx => xx.Id == id);

            var credentials = new TopicCredentials(config["EventGrid:Token"]);
            var client = new EventGridClient(credentials);

            var queueClient = new QueueClient(config["ServiceBusQueue:ConnectionString"], config["ServiceBusQueue:EntityName"]);
            queueClient.RegisterMessageHandler(async (m, c) =>
            {
                var jsonText = Encoding.UTF8.GetString(m.Body);
                var json = JsonConvert.DeserializeObject<JObject>(jsonText);
                //documentClient.CreateDocumentAsync(documentCollection, json);

                var eventData = string.Empty;

                var events = new List<EventGridEvent>();
                var ev = new EventGridEvent
                {
                    Id = Guid.NewGuid().ToString(),
                    EventTime = DateTime.UtcNow,
                    EventType = $"NewReservatonArrived",
                    Subject = $"{json.Value<string>("RoomId")}-{json.Value<DateTime>("StartDate").ToString("yyyyMMdd")}",
                    Data = $"{jsonText}",
                    DataVersion = "1.0"
                };
                events.Add(ev);
                await client.PublishEventsAsync(
                    config["EventGrid:NewReservationTopic"], events);

                WriteLine($"type={ev.EventType} subject={ev.Subject}");

            }, async (ex) => {
            });
;
            await Task.Delay(Timeout.Infinite);
        }
    }
}
