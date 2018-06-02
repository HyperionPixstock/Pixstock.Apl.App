namespace pixstock.apl.app.core.Infra
{
    public interface IScreenManager
    {
        void ShowScreen(string screenName);

        void HideScreen(string screenName);

        /// <summary>
        /// ビュー層に画面遷移メッセージを送信する
        /// </summary>
        void UpdateScreenTransitionView(object param);
    }
}
