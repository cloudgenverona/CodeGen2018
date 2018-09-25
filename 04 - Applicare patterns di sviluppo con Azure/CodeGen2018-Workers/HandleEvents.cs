// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;

namespace CodeGen2018_Workers
{
    public static class HandleEvents
    {
        //[FunctionName("HandleEvents")]
        //public static void Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        //{
        //    switch (eventGridEvent.EventType)
        //    {
        //        case "NewReservatonArrived":
        //            var roomId = eventGridEvent.Subject;
        //            // rebuild Materialized View
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }
}
