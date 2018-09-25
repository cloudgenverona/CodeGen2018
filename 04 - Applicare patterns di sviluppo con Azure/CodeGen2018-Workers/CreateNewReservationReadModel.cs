using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using CodeGen2018_Workers.Models;
using System;
using System.Threading.Tasks;

namespace CodeGen2018_Workers
{
    public static class CreateNewReservationReadModel
    {
        const string Pattern = "/providers/Microsoft.EventGrid/topics/marparcodegen2018-";

        [FunctionName("CreateNewReservationReadModel")]
        public static async Task Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            var localTopic = eventGridEvent.Topic.Substring(eventGridEvent.Topic.IndexOf(Pattern) + Pattern.Length);
            log.LogInformation($"localTopic={localTopic} subject={eventGridEvent.Subject} data={eventGridEvent.Data} eventType={eventGridEvent.EventType}");

            var json = JsonConvert.DeserializeObject<JObject>(eventGridEvent.Data as string);

            var account = CloudStorageAccount.Parse("");
            var tableClient = account.CreateCloudTableClient();
            var table = tableClient.GetTableReference("roomreservations");

            var operation = TableOperation.InsertOrReplace(new RoomReservation
            {
                PartitionKey = eventGridEvent.Subject,
                RowKey = json.Value<DateTime>("StartDate").ToString("HHmm"),
                Title = json.Value<string>("Title"),
                UserId = json.Value<string>("UserId"),
                Length = json.Value<decimal>("Length")
            });

            await table.ExecuteAsync(operation);
        }
    }
}
