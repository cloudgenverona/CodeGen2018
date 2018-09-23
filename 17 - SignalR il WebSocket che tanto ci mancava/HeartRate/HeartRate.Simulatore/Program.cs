using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;


namespace HeartRate.Simulatore
{
    public class Program
    {
        private static EventHubClient eventHubClient;
        private const string EventHubConnectionString = "Endpoint=sb://codegen.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SWflHzFFDGtugOBjlu2jazqKuZDn78YsD76PEg4bSBg=";
        private const string EventHubName = "temperature";

        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            try
            {
                var connectionStringBuilder = new EventHubsConnectionStringBuilder(EventHubConnectionString)
                {
                    EntityPath = EventHubName
                };

                while (true)
                {
                    Console.WriteLine("Quanti eventi vuoi generare?");
                    eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
                    int eventsCount = int.Parse(Console.ReadLine());

                    await SendMessagesToEventHub(eventsCount);

                    await eventHubClient.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            } 
        }

        // Creates an Event Hub client and sends 100 messages to the event hub.
        private static async Task SendMessagesToEventHub(int numMessagesToSend)
        {
            foreach (int item in HeartrateGenerator.GenerateHeartrate(numMessagesToSend, 0))
            {
                try
                {
                    var message = item.ToString();
                    Console.WriteLine($"Sending message: {message}");
                    await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
                }

                await Task.Delay(200);
            }
           
            Console.WriteLine($"{numMessagesToSend} messages sent.");
        }
    }

    public static class HeartrateGenerator
    {
        static Random random = new Random();

        public static IEnumerable<int> GenerateHeartrate(
            int totalSequenceLength,
            int dropsBelow60After,
            int bouncesBackAfter = -1)
        {
            // NOTE: check input data

            int i = 0;

            // return values > 60
            while (i < dropsBelow60After)
            {
                i++;
                yield return 60 + random.Next() % 60;
            }

            if (bouncesBackAfter > 0)
                // return values < 60
                while (i < bouncesBackAfter)
                {
                    i++;
                    yield return random.Next() % 60;
                }

            // return normal values again
            while (i < totalSequenceLength)
            {
                i++;
                yield return 60 + random.Next() % 60;
            }
        }
    }
}
