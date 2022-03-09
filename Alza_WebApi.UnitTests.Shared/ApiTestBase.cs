using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Alza_WebApi.UnitTests.Shared
{
    /// <summary>
    /// Test base for creating WebApi server
    /// </summary>
    public abstract class ApiTestBase
    {
        /// <summary>
        /// Create server.
        /// </summary>
        /// <typeparam name="TStartup">The type of Startup class.</typeparam>
        /// <param name="configureServicesCallback">ConfigureServices callback.</param>
        /// <param name="configureCallback">Configure callback.</param>
        /// <param name="configureAppConfigurationCallback">ConfigureAppConfiguration callback.</param>
        /// <returns>Test server.</returns>
        protected TestServer CreateServer<TStartup>(
            Action<WebHostBuilderContext, IServiceCollection> configureServicesCallback = null,
            Action<WebHostBuilderContext, IApplicationBuilder> configureCallback = null,
            Action<WebHostBuilderContext, IConfigurationBuilder> configureAppConfigurationCallback = null)
            where TStartup : class
        {
            var webHostBuilder = new WebHostBuilder()
                .UseStartup<TStartup>();

            if (configureAppConfigurationCallback != null)
            {
                webHostBuilder
                    .ConfigureAppConfiguration(configureAppConfigurationCallback);
            }

            if (configureServicesCallback != null)
            {
                webHostBuilder
                    .ConfigureServices(configureServicesCallback);
            }

            if (configureCallback != null)
            {
                webHostBuilder
                    .Configure(configureCallback);
            }

            var server = new TestServer(webHostBuilder);

            return server;
        }
    }
}
