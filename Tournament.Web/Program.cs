using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tournament.Web.BL.Extensions;
using Tournament.Web.BL.Installers;

namespace Tournament.Web
{
    public class Program
    {
        const string defaultCultureString = "en";

        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            var apiBaseUrl = builder.Configuration.GetValue<string>("ApiBaseUrl");

            builder.Services.AddInstaller<WebBLInstaller>(apiBaseUrl);
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddLocalization();

            var host = builder.Build();

            var culture = new CultureInfo(defaultCultureString);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            await host.RunAsync();
        }
    }
}
