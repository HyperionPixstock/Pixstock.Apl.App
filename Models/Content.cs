using Newtonsoft.Json;
using Pixstock.Common.Model;

namespace pixstock.apl.app.Models
{
    /// <summary>
    /// コンテント情報モデル(サービス側とインターフェースで同期する)
    /// </summary>
    public class Content : IContent
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string ThumbnailKey { get; set; }

        public string IdentifyKey { get; set; }

        public string ContentHash { get; set; }

        public string Caption { get; set; }

        public string Comment { get; set; }

        public bool ArchiveFlag { get; set; }

        public bool ReadableFlag { get; set; }

        public int StarRating { get; set; }

        /// <summary>
        /// UIでサムネイル画像を取得する際のURL
        /// </summary>
        /// <returns></returns>
        public string ThumbnailImageSrcUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string PreviewFileUrl { get; set; }

        /// <summary>
        /// リンクしているカテゴリ情報
        /// </summary>
        /// <remarks>
        /// このプロパティは、フロントエンドへのシリアライズ対象外です。
        /// </remarks>
        /// <returns></returns>
        [JsonIgnore]
        public Category LinkCategory { get; set; }
    }
}