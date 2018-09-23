using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ServerlessApp.Triggers
{
    public static class BlobFunction
    {
        [FunctionName("BlobFunction")]
        public static void Run([BlobTrigger("signalr/{name}", Connection = "AzureBlobStorage")]Stream myBlob, 
            string name,
            [Queue("blobqueue", Connection = "AzureBlobStorage")]ICollector<string> outputQueueItem,
            ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
            outputQueueItem.Add(name);
        }
    }
}
