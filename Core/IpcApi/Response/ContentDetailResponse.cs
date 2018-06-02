using pixstock.apl.app.Models;

namespace pixstock.apl.app.core.IpcApi.Response
{
    public class ContentDetailResponse
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Content Content { get; set; }

        /// <summary>
        /// コンテント情報が所属しているカテゴリを取得します
        /// </summary>
        /// <returns></returns>
        public Category Category { get; set; }
    }
}