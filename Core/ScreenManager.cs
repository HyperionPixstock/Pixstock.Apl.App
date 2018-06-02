using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using pixstock.apl.app.core.Infra;
using pixstock.apl.app.core.Intent;

namespace pixstock.apl.app.core
{
    /// <summary>
    /// DIコンテナに登録する画面遷移状態保持および画面遷移イベント制御用の管理クラス
    /// </summary>
    public class ScreenManager : IScreenManager
    {
        private readonly ILogger mLogger;

        /// <summary>
        /// 画面遷移のスタック
        /// フロントエンドの画面スタックと同期すること。
        /// </summary>
        /// <typeparam name="ScreenItem"></typeparam>
        /// <returns></returns>
        readonly Stack<ScreenItem> mScreenStack = new Stack<ScreenItem>();

        /// <summary>
        /// ワークフローの画面遷移により、発生した画面表示非表示イベントのリスト
        /// このリストは、ビュー層へのみ処理の画面遷移を示している。
        /// ビュー層への画面処理メッセージ発行でリスト内容をクリアする。
        /// </summary>
        /// <typeparam name="ScreenEventItem"></typeparam>
        /// <returns></returns>
        List<ScreenEventItem> mScreenTransitionEventList = new List<ScreenEventItem>();

        readonly IIntentManager mIntentManger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="intentManager"></param>
        public ScreenManager(IIntentManager intentManager, ILoggerFactory loggerFactory)
        {
            this.mIntentManger = intentManager;
            this.mLogger = loggerFactory.CreateLogger(this.GetType().FullName);
        }

        public void ShowScreen(string screenName)
        {
            // スタックに画面情報を追加する
            mScreenTransitionEventList.Add(new ScreenEventItem
            {
                ScreenName = screenName,
                PushEventFlag = true
            });
        }

        public void HideScreen(string screenName)
        {
            mScreenTransitionEventList.Add(new ScreenEventItem
            {
                ScreenName = screenName,
                PushEventFlag = false
            });
        }

        public void UpdateScreenTransitionView(object param)
        {
            this.mLogger.LogDebug(LoggingEvents.Undefine, "[ScreenManager][UpdateScreenTransitionView] - IN");
            this.mLogger.LogDebug(LoggingEvents.Undefine, "   Count=" + mScreenTransitionEventList.Count);
            var viewEventList = new List<UpdateViewIntentParameter>();

            var copy = new List<ScreenEventItem>(mScreenTransitionEventList);
            mScreenTransitionEventList.Clear();

            ScreenEventItem lastItem = null;
            foreach (var item in copy)
            {
                if (mScreenStack.Count == 1 &&
                    !item.PushEventFlag &&
                    lastItem == null)
                {
                    lastItem = item;
                }
                else if (mScreenStack.Count == 1 &&
                         lastItem != null &&
                         lastItem.ScreenName == item.ScreenName)
                {
                    // set
                    viewEventList.Add(new UpdateViewIntentParameter
                    {
                        ScreenName = item.ScreenName,
                        UpdateType = UpdateType.SET
                    });

                    lastItem = null;
                }
                else
                {
                    if (lastItem != null)
                    {
                        if (lastItem.PushEventFlag)
                        {
                            // push
                            viewEventList.Add(new UpdateViewIntentParameter
                            {
                                ScreenName = lastItem.ScreenName,
                                UpdateType = UpdateType.PUSH
                            });

                            mScreenStack.Push(new ScreenItem());
                        }
                        else
                        {
                            // pop
                            viewEventList.Add(new UpdateViewIntentParameter
                            {
                                ScreenName = lastItem.ScreenName,
                                UpdateType = UpdateType.POP
                            });
                            mScreenStack.Pop();
                        }
                    }

                    if (item.PushEventFlag)
                    {
                        // push
                        var viewEvent = new UpdateViewIntentParameter
                        {
                            ScreenName = item.ScreenName,
                            UpdateType = UpdateType.PUSH
                        };

                        if (mScreenStack.Count == 0)
                        {
                            viewEvent.UpdateType = UpdateType.SET;
                        }

                        viewEventList.Add(viewEvent);
                        mScreenStack.Push(new ScreenItem());
                    }
                    else
                    {
                        // pop
                        viewEventList.Add(new UpdateViewIntentParameter
                        {
                            ScreenName = item.ScreenName,
                            UpdateType = UpdateType.POP
                        });
                        mScreenStack.Pop();
                    }

                    lastItem = null;
                }
            }

            // 画面表示の切り替えがある場合のみ、UpdateViewメッセージを送信する
            if (viewEventList.Count > 0)
            {
                mIntentManger.AddIntent(ServiceType.FrontendIpc, "UpdateView", new UpdateViewResponse
                {
                    ViewEventList = viewEventList,
                    Parameter = param
                });
            }
        }

        public class ScreenItem
        {

        }

        public class ScreenEventItem
        {
            public string ScreenName;

            public bool PushEventFlag;
        }

        public class UpdateViewResponse
        {
            public List<UpdateViewIntentParameter> ViewEventList;

            public object Parameter;
        }
    }

}
