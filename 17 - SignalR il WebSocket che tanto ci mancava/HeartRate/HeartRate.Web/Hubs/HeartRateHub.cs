using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace HeartRate.Web.Hubs
{
    public class HeartRateHub : Hub
    {
        public HeartRateHub()
        {
        }

        #region [Connections]
        /// <summary>
        /// Connessione
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {

            await Groups.AddToGroupAsync(Context.ConnectionId, GroupsUsers.HeartRateGroup);
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Disconnessione Utente
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
        #endregion
    }
}

public static class GroupsUsers
{
    public static string HeartRateGroup = "HeartRate";
}

public class ChartDataPoint
{
    public ChartDataPoint(DateTime currentData, decimal value)
    {
        X = currentData;
        Y = value;
    }
    
    public DateTime X { get; }
    public decimal Y { get; }
}