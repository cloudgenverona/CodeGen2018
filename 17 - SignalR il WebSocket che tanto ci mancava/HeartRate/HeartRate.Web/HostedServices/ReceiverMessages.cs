using Microsoft.ServiceBus.Messaging;
using StazioneMetereologica.Web.HubServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StazioneMetereologica.Web.HostedServices
{
    public interface IReceiveMessage
    {
        Task StartReceive(CancellationToken ct);
    }

    public class ReceiverMessages
    {
        private EventHubClient eventHubClient { get; set; }
        private const string EndPointServer = "messages/events";
        private ITemperatureHubService _temperatureHubService;

        public ReceiverMessages(string connectionString, ITemperatureHubService temperatureHubService)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            _temperatureHubService = temperatureHubService ?? throw new ArgumentNullException("Temperature Hub Services is null");

            eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, EndPointServer);
        }

        public Task StartReceive(CancellationToken ct)
        {
            List<Task> receiveTasks = new List<Task>();
            foreach (string partition in eventHubClient.GetRuntimeInformation().PartitionIds)
            {
                receiveTasks.Add(ReceiveMessagesFromDeviceAsync(partition, ct));
            }
            return Task.WhenAny(receiveTasks);
        }

        private async Task ReceiveMessagesFromDeviceAsync(string partition, CancellationToken ct)
        {
            var eventHubReceiver = eventHubClient.GetDefaultConsumerGroup().CreateReceiver(partition, DateTime.UtcNow);
            while (true)
            {
                if (ct.IsCancellationRequested) break;
                EventData eventData = await eventHubReceiver.ReceiveAsync();
                if (eventData == null) continue;

                string data = Encoding.UTF8.GetString(eventData.GetBytes());
                decimal temperature = 10;

                Console.WriteLine("Message received. Partition: {0} Data: '{1}'", partition, data);
                // TODO: switch sui tipi di messaggio
                await _temperatureHubService.SendTemperatureToGroup(eventData.EnqueuedTimeUtc, temperature);
            }
        }
    }
}
