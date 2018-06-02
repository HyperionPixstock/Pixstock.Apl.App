using System;
using System.Threading;
using System.Threading.Tasks;

namespace pixstock.apl.app.core.Infra
{
    public interface IBackgroundTaskQueue
    {
        void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem);

        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}
