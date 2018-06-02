using System.Collections.Generic;
//using NLog;
using pixstock.apl.app.Models;
using RestSharp;

namespace pixstock.apl.app.core.Dao
{
    /// <summary>
    /// サービスからコンテント情報の操作を行うDAOクラス
    /// </summary>
    public class ContentDao : DaoBase
    {
        //private static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// コンテント情報を読み込みます
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        public Content LoadContent(long contentId)
        {
            var request = new RestRequest("artifact/{id}", Method.GET);
            request.AddUrlSegment("id", contentId);

            var response = mClient.Execute<PixstockResponseAapi<Content>>(request);

            var content = response.Data.Value;
            // サムネイルが存在する場合は、サムネイルのURLを設定
            if (!string.IsNullOrEmpty(content.ThumbnailKey))
            {
                content.ThumbnailImageSrcUrl = BASEURL + "/thumbnail/" + content.ThumbnailKey;
            }
            content.PreviewFileUrl = BASEURL + "/artifact/" + content.Id + "/preview";
            content.LinkCategory = LinkGetCategory(content.Id, response.Data.Link);
            return content;
        }

        /// <summary>
        /// "artifact/{id}"のcategoryリンクデータを取得する
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="link"></param>
        /// <returns></returns>
        private Category LinkGetCategory(long contentId, Dictionary<string, object> link)
        {
            // リンクデータが取得できない場合は、リンクデータのリクエストを実施しない
            if (!link.ContainsKey("category")) return null;

            var request = new RestRequest("artifact/{id}/category", Method.GET);
            request.AddUrlSegment("id", contentId);
            var response = mClient.Execute<PixstockResponseAapi<Category>>(request);
            var category = response.Data.Value;
            category.Labels = response.Data.GetRelative<List<Label>>("labels");
            return category;
        }

    }
}
