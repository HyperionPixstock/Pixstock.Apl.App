using System;
using System.Linq;
using ElectronNET.API;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using pixstock.apl.app.core.Infra;
using SimpleInjector;
using static pixstock.apl.app.core.ScreenManager;

namespace pixstock.apl.app.core.Service
{
    public class IpcSendService : IMessagingServiceExtention
    {
        private ILogger mLogger;

        public ServiceType ServiceType => ServiceType.FrontendIpc;

        public Container Container { get; set; }

        public void Execute(string message, object parameter)
        {
            this.mLogger.LogDebug(LoggingEvents.Undefine, "[IpcSendService][Execute]");
            this.mLogger.LogDebug(LoggingEvents.Undefine, "    Parameter=" + message);

            if (message == "UpdateView")
            {
                // IPCメッセージを作成する
                var messageparameter = parameter as UpdateViewResponse;
                var ipcMessage = new IpcMessage();
                object obj = new
                {
                    UpdateList = messageparameter.ViewEventList,
                    Parameter = messageparameter.Parameter
                };
                ipcMessage.Body = JsonConvert.SerializeObject(obj, Formatting.Indented);
                Console.WriteLine("    Body(JSON)=" + ipcMessage.Body);

                var mainWindow = Electron.WindowManager.BrowserWindows.First();
                Electron.IpcMain.Send(mainWindow, "IPC_UPDATEVIEW", ipcMessage);
            }
            else if (message == "UpdateProp")
            {
                // UpdateProp
                //    キャッシュから任意のキーのオブジェクトを取得します。
                var propertyName = parameter.ToString();
                var memCache = Container.GetInstance<IMemoryCache>();

                object cachedObject;
                if (memCache.TryGetValue(propertyName, out cachedObject))
                {
                    var ipcMessage = new IpcMessage();
                    object obj = new
                    {
                        PropertyName = propertyName,
                        Value = JsonConvert.SerializeObject(cachedObject)
                    };

                    ipcMessage.Body = JsonConvert.SerializeObject(obj, Formatting.Indented);
                    var mainWindow = Electron.WindowManager.BrowserWindows.First();
                    Electron.IpcMain.Send(mainWindow, "IPC_UPDATEPROP", ipcMessage);
                }
                else
                {
                    Console.WriteLine("[IpcSendService][Execute] Faile MemCache");
                }
            }
        }

        public void InitializeExtention()
        {
            // EMPTY
        }

        public void Verify()
        {
            ILoggerFactory loggerFactory = this.Container.GetInstance<ILoggerFactory>();
            this.mLogger = loggerFactory.CreateLogger(this.GetType().FullName);
        }
    }
}
