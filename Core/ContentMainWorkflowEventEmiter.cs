using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ElectronNET.API;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using NLog;
using pixstock.apl.app.core.Dao;
using pixstock.apl.app.core.IpcApi.Response;
using pixstock.apl.app.Models;
using Pixstock.Base.AppIf.Sdk;
using RestSharp;

namespace pixstock.apl.app.core
{
    public class ContentMainWorkflowEventEmiter
    {
        //private static Logger _logger = LogManager.GetCurrentClassLogger();

        //static string BASEURL = "http://localhost:5080/aapi";

        /// <summary>
        ///
        /// </summary>
        public void Initialize()
        {
            Electron.IpcMain.OnSync("EAV_GETCATEGORY", OnEAV_GETCATEGORY);
            Electron.IpcMain.OnSync("EAV_GETCONTENT", OnEAV_GETCONTENT);
            Electron.IpcMain.OnSync("EAV_GETLABELLIST", OnEAV_GETLABELLIST);
            Electron.IpcMain.OnSync("EAV_GETLABELLINKCATEGORYLIST", OnEAV_GETLABELLINKCATEGORYLIST);
        }

        public void Dispose()
        {
            Electron.IpcMain.RemoveAllListeners("EAV_GETCATEGORY");
            Electron.IpcMain.RemoveAllListeners("EAV_GETCONTENT");
            Electron.IpcMain.RemoveAllListeners("EAV_GETLABELLIST");
            Electron.IpcMain.RemoveAllListeners("EAV_GETLABELLINKCATEGORYLIST");
        }

        private string OnEAV_GETCATEGORY(object args)
        {
            try
            {
                var requestParam = ((JObject)args).ToObject<PARAM_EAV_GETCATEGORY>();
                if (requestParam.LimitSubCategory == 0)
                    requestParam.LimitSubCategory = CategoryDao.MAXLIMIT;
                var dao_cat = new CategoryDao();
                var category = dao_cat.LoadCategory(requestParam.CategoryId, (int)requestParam.OffsetSubCategory, (int)requestParam.LimitSubCategory);

                // IPCレスポンス作成
                var response = new CategoryDetailResponse();
                response.Category = category;
                response.SubCategory = category.LinkSubCategoryList.ToArray();
                response.Content = category.LinkContentList.ToArray();
                return JsonConvert.SerializeObject(response);
            }
            catch (Exception expr)
            {
                //_logger.Error(expr, "OnEAV_GETCATEGORYの例外");

                var response = new CategoryDetailResponse();
                return JsonConvert.SerializeObject(response);
            }
        }

        private string OnEAV_GETCONTENT(object args)
        {
            try
            {
                var contentId = long.Parse(args.ToString());
                var dao_content = new ContentDao();
                var content = dao_content.LoadContent(contentId);
                var response = new ContentDetailResponse()
                {
                    Content = content,
                    Category = content.LinkCategory
                };
                return JsonConvert.SerializeObject(response);
            }
            catch (Exception expr)
            {
                //_logger.Error(expr, "OnEAV_GETCONTENTの例外");

                var response = new ContentDetailResponse();
                return JsonConvert.SerializeObject(response);
            }
        }

        private string OnEAV_GETLABELLIST(object args)
        {
            var dao_label = new LabelDao();
            var labels = dao_label.LoadLabel();
            return JsonConvert.SerializeObject(labels);
        }

        private string OnEAV_GETLABELLINKCATEGORYLIST(object args) {
            var query = args.ToString();
            var dao_label = new LabelDao();
            var categoryList = dao_label.LoadLabelLinkCategory(query,0,0);
            return JsonConvert.SerializeObject(categoryList);
        }

        private class PARAM_EAV_GETCATEGORY
        {
            public int CategoryId { get; set; }

            public int OffsetSubCategory { get; set; }

            public int LimitSubCategory { get; set; }

            public int OffsetContent { get; set; }
        }
    }
}
