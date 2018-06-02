using System;
using Hyperion.Pf.Workflow;
using Hyperion.Pf.Workflow.StateMachine;
using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Infrastructure;
using Appccelerate.StateMachine.Machine;
using Appccelerate.StateMachine.Persistence;
using Appccelerate.StateMachine.Reports;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
namespace Pixstock.Core {
public class CLS_INIT : States {}
public class CLS_ROOT : States {}
public class CLS_HomePage : States {}
public class CLS_HomePageBase : States {}
public class CLS_ThumbnailListPage : States {}
public class CLS_ThumbnailListPageBase : States {}
public class CLS_PreviewPage : States {}
public class CLS_RESPONSE_GETCATEGORY : Events {}
public class CLS_RESPONSE_GETCATEGORYCONTENT : Events {}
public class CLS_CategorySelectBtnClick : Events {}
public class CLS_ACT_ContinueCategoryList : Events {}
public class CLS_ACT_DISPLAY_PREVIEWCURRENTLIST : Events {}
public class CLS_RESPONSE_GETCONTENT : Events {}
public class CLS_TRNS_TOPSCREEN : Events {}
public class CLS_TRNS_EXIT : Events {}
public class CLS_TRNS_ThumbnailListPage : Events {}
public class CLS_TRNS_PreviewPage : Events {}
public class CLS_TRNS_BACK : Events {}
public class CLS_TRNS_DEBUG_BACK : Events {}
public class CLSINVALID_INVALID : Events {}
public partial class States : WorkflowStateBase {
	public static CLS_INIT INIT { get; } = new CLS_INIT();
	public static CLS_ROOT ROOT { get; } = new CLS_ROOT();
	public static CLS_HomePage HomePage { get; } = new CLS_HomePage();
	public static CLS_HomePageBase HomePageBase { get; } = new CLS_HomePageBase();
	public static CLS_ThumbnailListPage ThumbnailListPage { get; } = new CLS_ThumbnailListPage();
	public static CLS_ThumbnailListPageBase ThumbnailListPageBase { get; } = new CLS_ThumbnailListPageBase();
	public static CLS_PreviewPage PreviewPage { get; } = new CLS_PreviewPage();
}
public partial class Events : WorkflowEventBase {
        public static Dictionary<string, Events> cacheEventsDict = null;
        public static Events ForName(string name)
        {
            if (cacheEventsDict != null) return cacheEventsDict[name];

            cacheEventsDict = new Dictionary<string, Events>();
            var hogeType = typeof(Events);
            var names = hogeType.GetProperties(BindingFlags.Static | BindingFlags.Public)
                  .Where(x => x.PropertyType.IsSubclassOf(typeof(Events)))
                  .Select(x => new { Name = x.Name, Value = x.GetValue(hogeType, null) as Events }).ToList();
            foreach (var o in names)
            {
                cacheEventsDict.Add(o.Name, o.Value);
            }

            return cacheEventsDict[name];
        }
	public static CLS_RESPONSE_GETCATEGORY RESPONSE_GETCATEGORY { get; } = new CLS_RESPONSE_GETCATEGORY();
	public static CLS_RESPONSE_GETCATEGORYCONTENT RESPONSE_GETCATEGORYCONTENT { get; } = new CLS_RESPONSE_GETCATEGORYCONTENT();
	public static CLS_CategorySelectBtnClick CategorySelectBtnClick { get; } = new CLS_CategorySelectBtnClick();
	public static CLS_ACT_ContinueCategoryList ACT_ContinueCategoryList { get; } = new CLS_ACT_ContinueCategoryList();
	public static CLS_ACT_DISPLAY_PREVIEWCURRENTLIST ACT_DISPLAY_PREVIEWCURRENTLIST { get; } = new CLS_ACT_DISPLAY_PREVIEWCURRENTLIST();
	public static CLS_RESPONSE_GETCONTENT RESPONSE_GETCONTENT { get; } = new CLS_RESPONSE_GETCONTENT();
	public static CLS_TRNS_TOPSCREEN TRNS_TOPSCREEN { get; } = new CLS_TRNS_TOPSCREEN();
	public static CLS_TRNS_EXIT TRNS_EXIT { get; } = new CLS_TRNS_EXIT();
	public static CLS_TRNS_ThumbnailListPage TRNS_ThumbnailListPage { get; } = new CLS_TRNS_ThumbnailListPage();
	public static CLS_TRNS_PreviewPage TRNS_PreviewPage { get; } = new CLS_TRNS_PreviewPage();
	public static CLS_TRNS_BACK TRNS_BACK { get; } = new CLS_TRNS_BACK();
	public static CLS_TRNS_DEBUG_BACK TRNS_DEBUG_BACK { get; } = new CLS_TRNS_DEBUG_BACK();
	public static CLSINVALID_INVALID INVALID { get; } = new CLSINVALID_INVALID();
}
}
