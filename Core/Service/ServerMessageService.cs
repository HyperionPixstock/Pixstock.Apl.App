using System;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using pixstock.apl.app.core.Dao;
using pixstock.apl.app.core.Infra;
using pixstock.apl.app.core.IpcApi.Response;
using pixstock.apl.app.json.ServerMessage;
using SimpleInjector;

namespace pixstock.apl.app.core.Service {
    public class ServerMessageService : IMessagingServiceExtention {
        private ILogger mLogger;

        public ServiceType ServiceType => ServiceType.Server;

        public Container Container { get; set; }

        public void Execute (string intentMessage, object parameter) {
            this.mLogger.LogDebug (LoggingEvents.Undefine, "[ServerMessageService][Execute] intentMessage={} parameter={}", intentMessage, parameter);

            try {
                var memCache = Container.GetInstance<IMemoryCache> ();
                var intentManager = Container.GetInstance<IIntentManager> ();

                if (intentMessage == "GETCATEGORY") {
                    var param = JsonConvert.DeserializeObject<GetCategoryParam> (parameter.ToString ());

                    var dao_cat = new CategoryDao ();
                    var category = dao_cat.LoadCategory (param.CategoryId, param.OffsetSubCategory, param.LimitOffsetSubCategory);

                    var response = new CategoryDetailResponse ();
                    response.Category = category;
                    response.SubCategory = category.LinkSubCategoryList.ToArray ();
                    response.Content = category.LinkContentList.ToArray ();

                    memCache.Set ("ResponseCategory", response);

                    this.mLogger.LogDebug (LoggingEvents.Undefine, "[ServerMessageService][Execute] Register RESPONSE_GETCATEGORY");
                    intentManager.AddIntent (ServiceType.Workflow, "RESPONSE_GETCATEGORY", null);
                } else if (intentMessage == "GETCATEGORYCONTENT") {
                    var categoryId = long.Parse (parameter.ToString ());

                    var dao_cat = new CategoryDao ();
                    var category = dao_cat.LoadCategory (categoryId, 0, CategoryDao.MAXLIMIT);

                    var response = new CategoryDetailResponse ();
                    response.Category = category;
                    response.Content = category.LinkContentList.ToArray ();

                    memCache.Set ("ResponseCategoryContent", response);

                    intentManager.AddIntent (ServiceType.Workflow, "RESPONSE_GETCATEGORYCONTENT", null);
                } else if (intentMessage == "GETCONTENT") {
                    var contentId = long.Parse (parameter.ToString ());

                    var dao_content = new ContentDao ();
                    var content = dao_content.LoadContent (contentId);

                    var response = new ContentDetailResponse () {
                        Content = content,
                        Category = content.LinkCategory
                    };

                    memCache.Set ("ResponsePreviewContent", response);

                    intentManager.AddIntent (ServiceType.Workflow, "RESPONSE_GETCONTENT", null);
                } else if (intentMessage == "UpdateReading") {
                    var contentId = long.Parse (parameter.ToString ());
                    var dao_content = new ContentDao ();
                    var content = dao_content.LoadContent (contentId);
                    var dao_category = new CategoryDao ();
                    dao_category.UpdateReading (content.LinkCategory.Id);
                } else if (intentMessage == "UpdateCategoryProp") {
                    var param = JsonConvert.DeserializeObject<UpdateCategoryPropParam> (parameter.ToString ());
                    var dao_category = new CategoryDao ();
                    dao_category.Update (param.CategoryId, param.Category);
                } else {
                    throw new ApplicationException("Unknown MessageName " + intentMessage);
                }
            } catch (Exception expr) {
                this.mLogger.LogDebug (LoggingEvents.Undefine, expr, "[ServerMessageService][Execute] ", expr.Message);
            }
        }

        public void InitializeExtention () {
            // EMPTY
        }

        public void Verify () {
            ILoggerFactory loggerFactory = this.Container.GetInstance<ILoggerFactory> ();
            this.mLogger = loggerFactory.CreateLogger (this.GetType ().FullName);
        }
    }
}
