using RestSharp;

namespace pixstock.apl.app.core.Dao
{
    public abstract class DaoBase
    {
        protected const string BASEURL = "http://localhost:5080/aapi";

        protected readonly RestClient mClient;

        public DaoBase()
        {
            mClient = new RestClient(BASEURL);
        }
    }
}