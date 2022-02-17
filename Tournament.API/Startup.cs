using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using NSwag.AspNetCore;
using Tournament.DAL;
using Tournament.DAL.Repositories;
using Tournament.DAL.Entities;
using Tournament.API.Mappers;

    namespace Tournament.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerDocument();

            if (_env.IsDevelopment())
            {
                services.AddDbContextFactory<TournamentDbContext, TournamentDbContextDevFactory>();
            }
            else
            {
                services.AddDbContextFactory<TournamentDbContext, TournamentDbContextFactory>();
            }

            services.AddAutoMapper(config => config.AddProfile(new MappingProfile()));

            services.AddScoped<IRepository<PersonEntity>, PersonRepository>();
            services.AddScoped<IRepository<PlaceEntity>, PlaceRepository>();
            services.AddScoped<IRepository<TeamEntity>, TeamRepository>();
            services.AddScoped<IRepository<MatchEntity>, MatchRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOpenApi();
            app.UseSwaggerUi3(settings =>
            {
                settings.DocumentTitle = "Tournament Manager Swagger UI";
                settings.SwaggerRoutes.Add(new SwaggerUi3Route("v1.0", "/swagger/v1/swagger.json"));
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
