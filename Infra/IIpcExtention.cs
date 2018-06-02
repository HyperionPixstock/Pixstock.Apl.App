using System;

namespace pixstock.apl.app.core.Infra
{
    public interface IIpcExtention
    {
        string IpcMessageName { get; }

        Type RequestHandler { get; }
    }
}
