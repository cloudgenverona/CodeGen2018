using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HeartRate.Web.HostedServices
{
    public abstract class BackgroudServices : IHostedService, IDisposable
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private Task _backgroundTask;
        public BackgroudServices()
        {

        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Create a linked token so we can trigger cancellation outside of this token's cancellation
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _backgroundTask = BackgroundProcessing(_cts.Token);
            // If the task is completed then return it, otherwise it's running
            return _backgroundTask.IsCompleted ? _backgroundTask : Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_backgroundTask == null)
                return;
            _cts.Cancel();
            await Task.WhenAny(_backgroundTask, Task.Delay(Timeout.Infinite, cancellationToken));
            // Throw if cancellation triggered
            cancellationToken.ThrowIfCancellationRequested();
        }

        protected abstract Task BackgroundProcessing(CancellationToken cancellationToken);

        public void Dispose()
        {
            _cts.Cancel();
        }
    }
}
