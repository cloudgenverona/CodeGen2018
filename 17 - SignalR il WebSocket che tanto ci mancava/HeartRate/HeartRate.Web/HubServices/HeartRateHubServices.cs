using Microsoft.AspNetCore.SignalR;
using HeartRate.Web.Hubs;
using System;
using System.Threading.Tasks;

namespace HeartRate.Web.HubServices
{
    public interface IHeartRateHubServices
    {
        Task SendHeartRateToGroup(DateTime heartRateData, decimal data);
        Task SendAlert(DateTime heartRateData, decimal data);
    }

    public class HeartRateHubServices : IHeartRateHubServices
    {
        private IHubContext<HeartRateHub> _hub;
        public HeartRateHubServices(IHubContext<HeartRateHub> hub)
        {
            _hub = hub ?? throw new ArgumentNullException("HeartRateHub is null");
        }

        #region [Alert]
        public Task SendAlert(DateTime heartRateData, decimal data)
        {
            return _hub.Clients
                .All
                .SendAsync("ReceiveAlert", new ChartDataPoint(heartRateData, data));
        }
        #endregion

        #region [Temperature]
        public Task SendHeartRateToGroup(DateTime heartRateData, decimal data)
        {
            return _hub.Clients
                .Groups(GroupsUsers.HeartRateGroup)
                .SendAsync("ReceiveHeartRate", new ChartDataPoint(heartRateData, data));
        }
        #endregion
    }
}
