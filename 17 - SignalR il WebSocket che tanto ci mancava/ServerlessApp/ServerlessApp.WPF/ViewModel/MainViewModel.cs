using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.SignalR.Client;
using GalaSoft.MvvmLight.Threading;
using ServerlessApp.Model;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Connections.Client;

namespace ServerlessApp.WPF.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private HubConnection connection;
        private ObservableCollection<SignalRData> _files = new ObservableCollection<SignalRData>();
        public ObservableCollection<SignalRData> Files
        {   get => _files;
            set
            {
                Set(ref _files, value);
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            Task.Run(async() =>
            {
                SignalRConnectionInfo connectionHub = await GetSignalrConnectionInfo();
                connection = new HubConnectionBuilder()
                 .WithUrl(connectionHub.url, 
                    (x) =>  x.AccessTokenProvider = () => Task.FromResult(connectionHub.accesstoken))
                 .Build();

                await connection.StartAsync();
            }).Wait();

            connection.On<SignalRData>("NotifyBlob", (file) =>
            {
                DispatcherHelper.RunAsync(() =>
                {
                    Files.Add(file);
                });
            });
        }

        private async Task<SignalRConnectionInfo> GetSignalrConnectionInfo()
        {
            SignalRConnectionInfo result = null;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://localhost:7071/api/negotiate");
            if (response.IsSuccessStatusCode)
            {
                var textResult = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<SignalRConnectionInfo>(textResult);
            }
            return result;
        }
    }

    public class SignalRConnectionInfo
    {
        public string url { get; set; }
        public string accesstoken { get; set; }
    }
}