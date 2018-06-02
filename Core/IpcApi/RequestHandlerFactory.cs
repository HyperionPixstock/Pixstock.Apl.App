using System;
using System.Collections.Generic;
using pixstock.apl.app.core.Infra;
using SimpleInjector;

namespace pixstock.apl.app.core.IpcApi
{
    public class RequestHandlerFactory : Dictionary<string, Type>, IRequestHandlerFactory
    {
        private readonly Container container;

        public RequestHandlerFactory(Container container)
        {
            this.container = container;
        }

        public IRequestHandler CreateNew(string name) =>
            (IRequestHandler)this.container.GetInstance(this[name]);
    }
}
