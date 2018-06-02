namespace pixstock.apl.app.core.Infra
{
    public interface IRequestHandler
    {
        void Handle(IpcMessage param);
    }
}
