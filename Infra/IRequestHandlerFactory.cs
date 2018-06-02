namespace pixstock.apl.app.core.Infra
{
    public interface IRequestHandlerFactory
    {
        IRequestHandler CreateNew(string name);
    }
}
