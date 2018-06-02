using pixstock.apl.app.Models;

namespace pixstock.apl.app.core.IpcApi.Response
{
    public class CategoryDetailResponse
    {
        public Category Category { get; set; }

        public Category[] SubCategory { get; set; }

        public Content[] Content { get; set; }
    }
}