using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Tournament.Web.BL.Facades;

namespace Tournament.Web.BL.Installers
{
    public class WebBLInstaller
    {
        public void Install(IServiceCollection serviceCollection, string apiBaseUrl)
        {
            serviceCollection.AddTransient<IPersonClient, PersonClient>(provider =>
            {
                var client = CreateApiHttpClient(provider, apiBaseUrl);
                return new PersonClient(client, apiBaseUrl);
            });
            serviceCollection.AddTransient<ITeamClient, TeamClient>(provider =>
            {
                var client = CreateApiHttpClient(provider, apiBaseUrl);
                return new TeamClient(client, apiBaseUrl);
            });
            serviceCollection.AddTransient<IPlaceClient, PlaceClient>(provider =>
            {
                var client = CreateApiHttpClient(provider, apiBaseUrl);
                return new PlaceClient(client, apiBaseUrl);
            });
            serviceCollection.AddTransient<IMatchClient, MatchClient>(provider =>
            {
                var client = CreateApiHttpClient(provider, apiBaseUrl);
                return new MatchClient(client, apiBaseUrl);
            });

            serviceCollection.AddTransient<PersonFacade>();
            serviceCollection.AddTransient<TeamFacade>();
            serviceCollection.AddTransient<PlaceFacade>();
            serviceCollection.AddTransient<MatchFacade>();
        }

        public HttpClient CreateApiHttpClient(IServiceProvider serviceProvider, string apiBaseUrl)
        {
            var client = new HttpClient() { BaseAddress = new Uri(apiBaseUrl) };
            client.BaseAddress = new Uri(apiBaseUrl);
            return client;
        }
    }
}
