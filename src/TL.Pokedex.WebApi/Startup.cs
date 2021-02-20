using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TL.Pokedex.Core.Abstractions.Repositories;
using TL.Pokedex.Core.Abstractions.TranslationServices;
using TL.Pokedex.Core.Interfaces;
using TL.Pokedex.Core.Services;
using TL.Pokedex.Infrastructure.Repositories.PokeApi;
using TL.Pokedex.Infrastructure.TranslationServices;

namespace TL.Pokedex.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TL Pokedex", Version = "v1"});
            });

            services.AddTransient<IPokemonService, PokemonService>();
            services.AddHttpClient<IPokemonRepository, PokeApiClient>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(Configuration.GetValue<string>("Pokedex:PokemonRepositoryUri"));
                });
            services.AddHttpClient<ITranslationService, FunTranslationService>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(Configuration.GetValue<string>("Pokedex:TranslationServiceUri"));
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TL Pokedex v1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
