using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tournament.API.Controllers;

namespace Tournament.API.Tests
{
    public class TournamentApiApplicationFactory : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(collection =>
            {
                var controllerAssemblyName = typeof(PersonController).Assembly.FullName;
                collection.AddMvc().AddApplicationPart(Assembly.Load(controllerAssemblyName));
            });
            return base.CreateHost(builder);
        }
    }
}
