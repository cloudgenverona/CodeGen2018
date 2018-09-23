using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using ServerlessApp.Model;

namespace ServerlessApp.Triggers
{
    public static class QueueFunction
    {
        [FunctionName("QueueFunction")]
        public static async Task Run([QueueTrigger("blobqueue", Connection = "AzureBlobStorage")]string blobName,
            [SignalR(HubName = "broadcast")]IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            await signalRMessages.AddAsync(new SignalRMessage()
            {
                Target = "NotifyBlob",
                Arguments = new object[] { new SignalRData { Name = blobName, Date = DateTime.UtcNow} }
            });
        }
    }
}
