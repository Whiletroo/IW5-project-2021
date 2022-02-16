using System;
using System.Net.Http;

namespace Tournament.Web.BL
{
    public partial class TeamClient
    {
        public TeamClient(HttpClient httpClient, string baseUrl) : this(httpClient)
        {
            BaseUrl = baseUrl;
        }
    }
}
