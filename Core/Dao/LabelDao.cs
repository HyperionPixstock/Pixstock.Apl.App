using System.Collections.Generic;
//using NLog;
using pixstock.apl.app.Models;
using RestSharp;

namespace pixstock.apl.app.core.Dao
{
    public class LabelDao : DaoBase
    {
        //private static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// ラベル情報を読み込みます
        /// </summary>
        /// <returns></returns>
        public ICollection<Label> LoadLabel()
        {
            var request = new RestRequest("label", Method.GET);
            var response = mClient.Execute<PixstockResponseAapi<List<Label>>>(request);
            return response.Data.Value;
        }

        /// <summary>
        /// サービスへラベルリンクデータ情報取得API(カテゴリ情報)を要求します。
        /// </summary>
        /// <param name="query">現時点ではラベル情報を示すキー</param>
        /// <param name="offset">未実装</param>
        /// <param name="limit">未実装</param>
        /// <returns></returns>
        public ICollection<Category> LoadLabelLinkCategory(string query, int offset, int limit)
        {
            var request = new RestRequest("label/{query}/category", Method.GET);
            request.AddUrlSegment("query", query);
            //request.AddQueryParameter("offset", offset.ToString()); 実装したら使用する

            //_logger.Info("Execute Request");
            var response = mClient.Execute<PixstockResponseAapi<List<Category>>>(request);

            //_logger.Info("Execute Respose");
            return response.Data.Value;
        }
    }
}
