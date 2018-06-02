namespace pixstock.apl.app.core.Infra
{
    public class IntentMessage
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public ServiceType ServiceType { get; set; }

        /// <summary>
        /// サービスに渡すメッセージ名
        /// </summary>
        /// <returns></returns>
        public string MessageName { get; set; }

        /// <summary>
        /// サービスに渡すパラメータ
        /// </summary>
        /// <returns></returns>
        public string Parameter { get; set; }
    }
}
