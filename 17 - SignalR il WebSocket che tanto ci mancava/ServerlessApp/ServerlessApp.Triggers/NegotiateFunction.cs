using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ServerlessApp.Triggers
{
    public static class NegotiateFunction
    {
        [FunctionName("negotiate")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")]HttpRequest req,
                                        [SignalRConnectionInfo(HubName = "broadcast")]SignalRConnectionInfo connectionInfo)//,
                                        //ILogger log)
        {
            //log.Log(LogLevel.Information, $"Negotiate info for endpoint: {info.Endpoint}");

            // Azure function doesn't support CORS well, workaround it by explicitly return CORS headers
            req.HttpContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            if (req.Headers["Origin"].Count > 0) req.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", req.Headers["Origin"][0]);
            if (req.Headers["Access-Control-Request-Headers"].Count > 0) req.HttpContext.Response.Headers.Add("Access-Control-Allow-Headers", req.Headers["access-control-request-headers"][0]);

            return new OkObjectResult(connectionInfo);

            //return info != null
            //    ? (ActionResult)new OkObjectResult(info)
            //    : new NotFoundObjectResult("Failed to load SignalR Info.");
        }
    }
}
