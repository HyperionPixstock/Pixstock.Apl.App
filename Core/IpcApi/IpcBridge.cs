using System;
using System.Collections.Generic;
using System.Linq;
using ElectronNET.API;
using Newtonsoft.Json.Linq;
//using NLog;
using pixstock.apl.app.core.Infra;
using SimpleInjector;

namespace pixstock.apl.app.core.IpcApi
{
    public class IpcBridge
    {
        const string IPCEXTENTION_NAMESPACE = "pixstock.apl.app.core.IpcHandler";

        //private static Logger _logger = LogManager.GetCurrentClassLogger();

        readonly Container mContainer;

        /// <summary>
        ///
        /// </summary>
        /// <param name="container"></param>
        public IpcBridge(Container container)
        {
            this.mContainer = container;
        }

        /// <summary>
        /// Ipcハンドラの初期化
        /// </summary>
        public RequestHandlerFactory Initialize()
        {
            RequestHandlerFactory requestHandlerFactory = new RequestHandlerFactory(mContainer);
            Container localContainer = new Container();

            var repositoryAssembly = typeof(IpcBridge).Assembly;

            // IPCメッセージ処理プラグイン登録
            // IIpcExtentionインターフェースを実装するクラスのみ登録可能とする
            var registrations =
                from type in repositoryAssembly.GetExportedTypes()
                where type.Namespace == IPCEXTENTION_NAMESPACE
                where type.GetInterfaces().Contains(typeof(IIpcExtention))
                select new { Service = type.GetInterfaces().Single(), Implementation = type };

            List<Type> implementationList = new List<Type>();
            foreach (var reg in registrations)
            {
                Console.WriteLine("[IpcBridge][Initialize] Register");

                implementationList.Add(reg.Implementation);
            }
            localContainer.RegisterCollection<IIpcExtention>(implementationList);
            Console.WriteLine("[IpcBridge][Initialize] Register Complete");

            localContainer.Verify();

            // IPCメッセージハンドラ登録
            foreach (var ext in localContainer.GetAllInstances<IIpcExtention>())
            {
                requestHandlerFactory.Add(ext.IpcMessageName, ext.RequestHandler);
                Electron.IpcMain.On(ext.IpcMessageName, (param) =>
                {
                    var factory = mContainer.GetInstance<IRequestHandlerFactory>();
                    var handler = factory.CreateNew(ext.IpcMessageName);

                    var requestParam = ((JObject)param).ToObject<IpcMessage>();
                    handler.Handle(requestParam);
                });
            }

            return requestHandlerFactory;
        }
    }
}
