using System.Net.Http;

namespace Tournament.Web.BL
{
    public partial class MatchClient
    {
        public MatchClient(HttpClient httpClient, string baseUrl) : this(httpClient)
        {
            BaseUrl = baseUrl;
        }
    }
}
