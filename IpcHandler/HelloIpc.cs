using System;
using pixstock.apl.app.core.Infra;
using pixstock.apl.app.core.IpcApi.Handler;

namespace pixstock.apl.app.core.IpcHandler
{
    /// <summary>
    /// この名前空間に、IPCメッセージ処理用のプラグインを追加してください。
    /// </summary>
    public class HelloIpc : IIpcExtention
    {
        public string IpcMessageName => "HELLO";

        public Type RequestHandler => typeof(LogHandler);
    }
}
