using System;
using System.Net.Http;

namespace Tournament.Web.BL
{
    public partial class PersonClient
    {
        public PersonClient(HttpClient httpClient, string baseUrl) : this(httpClient)
        {
            BaseUrl = baseUrl;
        }
    }
}
