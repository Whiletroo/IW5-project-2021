using System;
using System.Net.Http;

namespace Tournament.Web.BL
{
    public partial class PlaceClient
    {
        public PlaceClient(HttpClient httpClient, string baseUrl) : this(httpClient)
        {
            BaseUrl = baseUrl;
        }
    }
}
