using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using pixstock.apl.app.core.Infra;

namespace pixstock.apl.app.core
{
    public class IntentManager : IIntentManager
    {
        private readonly ILogger mLogger;

        readonly IBackgroundTaskQueue mBackgroundTaskQueue;

        readonly IServiceDistoributor mServiceDistoributor;

        /// <summary>
        ///コンストラクタ
        /// </summary>
        /// <param name="queue"></param>
        public IntentManager(IBackgroundTaskQueue queue, IServiceDistoributor distributor, ILoggerFactory loggerFactory)
        {
            this.mBackgroundTaskQueue = queue;
            this.mServiceDistoributor = distributor;
            this.mLogger = loggerFactory.CreateLogger(this.GetType().FullName);
        }

        public void AddIntent(ServiceType service, string intentName, object parameter)
        {
            mBackgroundTaskQueue.QueueBackgroundWorkItem(ExecuteItem);

            async Task ExecuteItem(CancellationToken token)
            {
                this.mLogger.LogDebug(LoggingEvents.Undefine, "[IntentManafer] IntentShori " + intentName);

                //await Task.Delay(TimeSpan.FromSeconds(5), token); //デバッグ用のウェイト

                // Distributorの呼び出し
                mServiceDistoributor.ExecuteService(service, intentName, parameter);
            }
        }
    }
}
