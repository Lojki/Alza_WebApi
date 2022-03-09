using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Alza_WebApi.UnitTests.Shared
{
    /// <summary>
    /// Backend Test base
    /// </summary>
    public abstract class BackendTestBase : ApiTestBase
    {
        protected ITestOutputHelper Output { get; }

        protected BackendTestBase(ITestOutputHelper output)
        {
            Output = output;
        }

        /// <summary>
        /// Create server and configure Environment and Autofac.
        /// </summary>
        /// <typeparam name="TStartup">The type of Startup class.</typeparam>
        /// <param name="configureServicesCallback">ConfigureServices callback.</param>
        /// <param name="configureCallback">Configure callback.</param>
        /// <param name="configureAppConfigurationCallback">ConfigureAppConfiguration callback.</param>
        /// <returns>Test server.</returns>
        protected new TestServer CreateServer<TStartup>(
            Action<WebHostBuilderContext, IServiceCollection> configureServicesCallback = null,
            Action<WebHostBuilderContext, IApplicationBuilder> configureCallback = null,
            Action<WebHostBuilderContext, IConfigurationBuilder> configureAppConfigurationCallback = null) where TStartup : class
        {
            return base.CreateServer<TStartup>((builder, services) =>
            {
                services.AddSingleton<IServiceProviderFactory<ContainerBuilder>, AutofacServiceProviderFactory>();

                configureServicesCallback?.Invoke(builder, services);
            },
            configureCallback,
            (builder, configurationBuilder) =>
            {
                builder.HostingEnvironment.EnvironmentName = "Development";

                configureAppConfigurationCallback?.Invoke(builder, configurationBuilder);
            });
        }

        /// <summary>
        /// Assert <see cref="HttpResponseMessage"/> <see cref="HttpStatusCode"/>.
        /// </summary>
        /// <param name="response">HTTP response message.</param>
        /// <param name="expectedStatusCode">Excpected status code.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        protected async Task AssertStatus(HttpResponseMessage response, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            try
            {
                Assert.Equal(expectedStatusCode, response.StatusCode);
            }
            catch
            {
                Output.WriteLine(await response.Content.ReadAsStringAsync());

                throw;
            }
        }
    }
}
