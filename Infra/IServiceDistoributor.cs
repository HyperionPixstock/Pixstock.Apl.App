namespace pixstock.apl.app.core.Infra
{
    public interface IServiceDistoributor
    {
        /// <summary>
        /// Intentメッセージを適切なサービスで処理を行う
        /// </summary>
        /// <param name="message"></param>
        void ExecuteService(ServiceType service, string intentName, object parameter);
    }
}
