using Microsoft.Azure.EventHubs.Processor;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using HeartRate.Web.HubServices;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;

namespace HeartRate.Web.HostedServices
{
    public class HeartRateHostedService : BackgroudServices, IHostedService, IDisposable
    {
        private ILogger<HeartRateHostedService> _logger;
        private IHeartRateHubServices _temperatureHubService;
        private readonly string EhConnectionString;
        private readonly string EhEntityPath = "temperature";
        private readonly string StorageContainerName = "eventhub";
        private readonly string StorageConnectionString;
        private EventProcessorHost eventProcessorHost;
        private ProcessorsFactory _processorsFactory;

        public HeartRateHostedService(string connectionString, string storageConnectionString, ILogger<HeartRateHostedService> logger, IHeartRateHubServices temperatureHubService)
        {
            _logger = logger ?? throw new ArgumentNullException("Logger ILogger<HeartRateHostedService> is null");
            _temperatureHubService = temperatureHubService ?? throw new ArgumentNullException("TemperatureHub service is null");
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));
            EhConnectionString = connectionString;
            StorageConnectionString = storageConnectionString;

            eventProcessorHost = new EventProcessorHost(
              EhEntityPath,
              PartitionReceiver.DefaultConsumerGroupName,
              EhConnectionString,
              StorageConnectionString,
              StorageContainerName);
        }

        protected override async Task BackgroundProcessing(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if(_processorsFactory == null)
                {
                    _processorsFactory = new ProcessorsFactory(_temperatureHubService);
                    await eventProcessorHost.RegisterEventProcessorFactoryAsync(_processorsFactory);
                }
                await Task.FromResult<object>(null);
            }
            await eventProcessorHost.UnregisterEventProcessorAsync();
        }
    }
}
