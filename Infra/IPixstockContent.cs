using SimpleInjector;

namespace pixstock.apl.app.core.Infra
{
    public interface IPixstockContent
    {
        /// <summary>
        /// コンテンツが管理するワークフローに、ワークフローイベントを処理する
        /// </summary>
        /// <remarks>
        /// コンテントが複数のワークフローを持っている場合もあるが、どのワークフローを処理するかはコンテンツが判断する。
        /// </remarks>
        /// <param name="workflowEvent">ワークフローイベント名</param>
        /// <param name="param">パラメータ</param>
        void FireWorkflowEvent(Container context, string workflowEvent, object param);
    }
}
