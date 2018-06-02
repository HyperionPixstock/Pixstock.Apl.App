using SimpleInjector;

namespace pixstock.apl.app.core.Infra
{
    public interface IMessagingServiceExtention
    {
        /// <summary>
        /// サービスの種類を取得します
        /// </summary>
        /// <returns></returns>
        ServiceType ServiceType { get; }

        Container Container { get; set; }

        /// <summary>
        /// 拡張機能の初期化
        /// </summary>
        void InitializeExtention();

        /// <summary>
        /// サービス処理を実行します
        /// </summary>
        /// <param name="context"></param>
        /// <param name="intentMessage"></param>
        /// <param name="parameter"></param>
        void Execute(string intentMessage, object parameter);

        void Verify();
    }
}
