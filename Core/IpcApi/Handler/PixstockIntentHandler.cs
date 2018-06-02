using System;
using Newtonsoft.Json;
using pixstock.apl.app.core.Infra;

namespace pixstock.apl.app.core.IpcApi.Handler
{
    /// <summary>
    /// PixstockのIntentメッセージとして処理するIPCハンドラ
    /// </summary>
    public class PixstockIntentHandler : IRequestHandler
    {
        readonly IIntentManager mIntentManager;

        public PixstockIntentHandler(IIntentManager intentManager)
        {
            this.mIntentManager = intentManager;
        }

        public void Handle(IpcMessage param)
        {
            Console.WriteLine("[PixstockIntentHandler][Handle] - IN " + param);

            // IPCメッセージから、PixstockのIntentメッセージを取り出す処理
            var message = JsonConvert.DeserializeObject<IntentMessage>(param.Body.ToString());
                        Console.WriteLine("   ServiceType=" + message.ServiceType);
            mIntentManager.AddIntent(message.ServiceType, message.MessageName, message.Parameter);
        }
    }
}
