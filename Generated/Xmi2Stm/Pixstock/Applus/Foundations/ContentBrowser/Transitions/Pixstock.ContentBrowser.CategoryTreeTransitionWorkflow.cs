using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Hyperion.Pf.Workflow;
using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Infrastructure;
using Appccelerate.StateMachine.Machine;
using Appccelerate.StateMachine.Persistence;
using Appccelerate.StateMachine.Reports;
using Hyperion.Pf.Workflow.StateMachine;

using Pixstock.Core;
namespace Pixstock.Applus.Foundations.ContentBrowser.Transitions {
public partial class CategoryTreeTransitionWorkflow : FrameStateMachine<States, Events>, IAsyncPassiveStateMachine {
	public static string Name = "Pixstock.ContentBrowser.CategoryTreeTransitionWorkflow";
public void Setup() {
DefineHierarchyOn(States.ROOT)
.WithHistoryType(HistoryType.None)
.WithInitialSubState(States.HomePage)
;
DefineHierarchyOn(States.HomePage)
.WithHistoryType(HistoryType.None)
.WithInitialSubState(States.HomePageBase)
.WithSubState(States.ThumbnailListPage)
;
DefineHierarchyOn(States.ThumbnailListPage)
.WithHistoryType(HistoryType.None)
.WithInitialSubState(States.ThumbnailListPageBase)
.WithSubState(States.PreviewPage)
;
In(States.INIT)
.On(Events.TRNS_TOPSCREEN)
.Goto(States.HomePage);
In(States.ROOT)
.On(Events.TRNS_EXIT)
.Goto(States.INIT);
In(States.ROOT)
.On(Events.TRNS_DEBUG_BACK)
.Goto(States.ROOT);
In(States.HomePageBase)
.On(Events.TRNS_ThumbnailListPage)
.Goto(States.ThumbnailListPage);
In(States.ThumbnailListPageBase)
.On(Events.TRNS_PreviewPage)
.Goto(States.PreviewPage);
In(States.ThumbnailListPageBase)
.On(Events.TRNS_BACK)
.Goto(States.HomePage);
In(States.PreviewPage)
.On(Events.TRNS_BACK)
.Goto(States.ThumbnailListPageBase);
In(States.ROOT)
.On(Events.RESPONSE_GETCATEGORY)
.Execute<object>(RESPONSE_GETCATEGORY);
In(States.ROOT)
.On(Events.RESPONSE_GETCATEGORYCONTENT)
.Execute<object>(RESPONSE_GETCATEGORYCONTENT);
In(States.HomePage)
.ExecuteOnEntry(__FTC_Event_HomePage_Entry);
In(States.HomePage)
.ExecuteOnExit(__FTC_Event_HomePage_Exit);
In(States.HomePageBase)
.ExecuteOnEntry(HomePageBase_Entry);
In(States.HomePageBase)
.ExecuteOnExit(HomePageBase_Exit);
In(States.ThumbnailListPage)
.ExecuteOnEntry(ThumbnailListPage_Entry);
In(States.ThumbnailListPage)
.ExecuteOnExit(ThumbnailListPage_Exit);
In(States.ThumbnailListPageBase)
.On(Events.CategorySelectBtnClick)
.Execute<object>(CategorySelectBtnClick);
In(States.ThumbnailListPageBase)
.On(Events.ACT_ContinueCategoryList)
.Execute<object>(ACT_ContinueCategoryList);
In(States.PreviewPage)
.ExecuteOnEntry(__FTC_Event_PreviewPage_Entry);
In(States.PreviewPage)
.ExecuteOnExit(__FTC_Event_PreviewPage_Exit);
In(States.PreviewPage)
.On(Events.ACT_DISPLAY_PREVIEWCURRENTLIST)
.Execute<object>(ACT_DISPLAY_PREVIEWCURRENTLIST);
In(States.PreviewPage)
.On(Events.RESPONSE_GETCONTENT)
.Execute<object>(RESPONSE_GETCONTENT);
	Initialize(States.INIT);
}
public virtual async Task RESPONSE_GETCATEGORY(object param) {
	Events.RESPONSE_GETCATEGORY.FireInvokeWorkflowEvent(new WorkflowMessageEventArgs(param));
	await OnRESPONSE_GETCATEGORY(param);
	Events.RESPONSE_GETCATEGORY.FireCallbackWorkflowEvent(new WorkflowMessageEventArgs(param));
}
public virtual async Task RESPONSE_GETCATEGORYCONTENT(object param) {
	Events.RESPONSE_GETCATEGORYCONTENT.FireInvokeWorkflowEvent(new WorkflowMessageEventArgs(param));
	await OnRESPONSE_GETCATEGORYCONTENT(param);
	Events.RESPONSE_GETCATEGORYCONTENT.FireCallbackWorkflowEvent(new WorkflowMessageEventArgs(param));
}
public virtual async Task __FTC_Event_HomePage_Entry() {
ICollection<int> ribbonMenuEventId = new List<int>{  };
	ShowFrame("HomePage",ribbonMenuEventId);
}
public virtual async Task __FTC_Event_HomePage_Exit() {
ICollection<int> ribbonMenuEventId = new List<int>{  };
	HideFrame("HomePage", ribbonMenuEventId);
}
public virtual async Task HomePageBase_Entry() {
	await OnHomePageBase_Entry();
}
public virtual async Task HomePageBase_Exit() {
	await OnHomePageBase_Exit();
}
public virtual async Task ThumbnailListPage_Entry() {
	await OnThumbnailListPage_Entry();
ICollection<int> ribbonMenuEventId = new List<int>{  };
	ShowFrame("ThumbnailListPage",ribbonMenuEventId);
}
public virtual async Task ThumbnailListPage_Exit() {
	await OnThumbnailListPage_Exit();
ICollection<int> ribbonMenuEventId = new List<int>{  };
	HideFrame("ThumbnailListPage", ribbonMenuEventId);
}
public virtual async Task CategorySelectBtnClick(object param) {
	Events.CategorySelectBtnClick.FireInvokeWorkflowEvent(new WorkflowMessageEventArgs(param));
	await OnCategorySelectBtnClick(param);
	Events.CategorySelectBtnClick.FireCallbackWorkflowEvent(new WorkflowMessageEventArgs(param));
}
public virtual async Task ACT_ContinueCategoryList(object param) {
	Events.ACT_ContinueCategoryList.FireInvokeWorkflowEvent(new WorkflowMessageEventArgs(param));
	await OnACT_ContinueCategoryList(param);
	Events.ACT_ContinueCategoryList.FireCallbackWorkflowEvent(new WorkflowMessageEventArgs(param));
}
public virtual async Task __FTC_Event_PreviewPage_Entry() {
ICollection<int> ribbonMenuEventId = new List<int>{  };
	ShowFrame("PreviewPage",ribbonMenuEventId);
}
public virtual async Task __FTC_Event_PreviewPage_Exit() {
ICollection<int> ribbonMenuEventId = new List<int>{  };
	HideFrame("PreviewPage", ribbonMenuEventId);
}
public virtual async Task ACT_DISPLAY_PREVIEWCURRENTLIST(object param) {
	Events.ACT_DISPLAY_PREVIEWCURRENTLIST.FireInvokeWorkflowEvent(new WorkflowMessageEventArgs(param));
	await OnACT_DISPLAY_PREVIEWCURRENTLIST(param);
	Events.ACT_DISPLAY_PREVIEWCURRENTLIST.FireCallbackWorkflowEvent(new WorkflowMessageEventArgs(param));
}
public virtual async Task RESPONSE_GETCONTENT(object param) {
	Events.RESPONSE_GETCONTENT.FireInvokeWorkflowEvent(new WorkflowMessageEventArgs(param));
	await OnRESPONSE_GETCONTENT(param);
	Events.RESPONSE_GETCONTENT.FireCallbackWorkflowEvent(new WorkflowMessageEventArgs(param));
}
}
}
