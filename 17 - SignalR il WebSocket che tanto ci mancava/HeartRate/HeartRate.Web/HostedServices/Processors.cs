using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using HeartRate.Web.HubServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HeartRate.Web.HostedServices
{
    public class ProcessorsFactory : IEventProcessorFactory
    {
        public IHeartRateHubServices _temperatureHubService;
        public ProcessorsFactory(IHeartRateHubServices temperatureHubService)
        {
            _temperatureHubService = temperatureHubService ?? throw new ArgumentNullException("TemperatureHubServices is null");
        }
        public IEventProcessor CreateEventProcessor(PartitionContext context)
        {
            return new EventHubProcessor(_temperatureHubService);
        }
    }

    public class EventHubProcessor : IEventProcessor
    {
        public IHeartRateHubServices _temperatureHubService;

        public EventHubProcessor(IHeartRateHubServices temperatureHubService)
        {
            _temperatureHubService = temperatureHubService ?? throw new ArgumentNullException("TemperatureHubServices is null");
        }

        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine($"Processor Shutting Down. Partition '{context.PartitionId}', Reason: '{reason}'.");
            return Task.CompletedTask;
        }

        public Task OpenAsync(PartitionContext context)
        {
            Console.WriteLine($"SimpleEventProcessor initialized. Partition: '{context.PartitionId}'");
            return Task.CompletedTask;
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            Console.WriteLine($"Error on Partition: {context.PartitionId}, Error: {error.Message}");
            return Task.CompletedTask;
        }

        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach (var eventData in messages)
            {
                var data = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
                decimal value = decimal.Parse(data);
                _temperatureHubService.SendHeartRateToGroup(eventData.SystemProperties.EnqueuedTimeUtc.ToLocalTime(), value);
                Console.WriteLine($"Message received. Partition: '{context.PartitionId}', Data: '{data}'");

                if(value < 65 || value > 110)
                {
                    _temperatureHubService.SendAlert(eventData.SystemProperties.EnqueuedTimeUtc.ToLocalTime(), value);
                }
            }

            return context.CheckpointAsync();
        }
    }
}
