using Newtonsoft.Json;
using Pixstock.Base.AppIf.Sdk;

namespace pixstock.apl.app.core.Dao
{
    public class PixstockResponseAapi<T> : ResponseAapi<T>
    {
        public RT GetRelative<RT>(string key)
        {
            return JsonConvert.DeserializeObject<RT>(this.Rel[key]);
        }
    }
}