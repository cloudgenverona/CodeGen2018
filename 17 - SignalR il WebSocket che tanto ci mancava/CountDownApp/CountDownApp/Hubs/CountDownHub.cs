using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CountDownApp.Hubs
{
    public class CountDownHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return Task.CompletedTask;
        }

        public ChannelReader<int> CountDownStream(int from)
        {
            var channel = Channel.CreateUnbounded<int>();

            _ = WriteToChannel(channel.Writer, from);

            return channel.Reader;

            async Task WriteToChannel(ChannelWriter<int> writer, int fromValue)
            {
                for (int i = fromValue; i >= 0; i--)
                {
                    await writer.WriteAsync(i);
                    await Task.Delay(1000);
                }
                writer.Complete();
            }
        }
        
    }
}
