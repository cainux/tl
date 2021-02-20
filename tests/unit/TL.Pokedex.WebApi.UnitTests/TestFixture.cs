using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Moq.AutoMock;
using TL.Pokedex.Core.Interfaces;
using Xunit.Abstractions;

namespace TL.Pokedex.WebApi.UnitTests
{
    public class TestFixture : IDisposable
    {
        public AutoMocker Mocker { get; }
        public WebApplicationFactory<Startup> WebApplicationFactory { get; }
        public ITestOutputHelper Output { get; set; }

        public TestFixture()
        {
            Mocker = new AutoMocker();

            WebApplicationFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder
                        .ConfigureLogging(logging =>
                        {
                            logging.ClearProviders();
                            logging.AddXunit(Output);
                        })
                        .ConfigureTestServices(services =>
                        {
                            services.RemoveAll<IPokemonService>();
                            services.AddSingleton(Mocker.GetMock<IPokemonService>().Object);
                        });
                });
        }

        public void Dispose()
        {
            WebApplicationFactory?.Dispose();
        }
    }
}
