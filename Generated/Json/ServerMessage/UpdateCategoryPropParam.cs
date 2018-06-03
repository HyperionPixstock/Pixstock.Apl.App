using pixstock.apl.app.Models;

namespace pixstock.apl.app.json.ServerMessage {
    public class UpdateCategoryPropParam {
        public long CategoryId;

        /// <summary>
        /// 更新するカテゴリ情報のプロパティのみ含めたオブジェクト
        /// </summary>
        public object Category;
    }
}
