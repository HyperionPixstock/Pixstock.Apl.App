using pixstock.apl.app.Models;

namespace pixstock.apl.app.core.Cache
{
    /// <summary>
    /// MemCacheのPreviewContentパラメータ
    /// </summary>
    public class PreviewContentParam
    {
        /// <summary>
        ///
        /// </summary>
        public Content Content;

        /// <summary>
        /// Contentプロパティが示すコンテント情報が所属しているカテゴリ情報の詳細
        /// </summary>
        public Category Category;
    }
}
