using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Alza_WebApi.Api.UnitTests.Server
{
    /// <summary>
    /// Test Startup
    /// </summary>
    public class TestStartup : Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestStartup"/> class.
        /// </summary>
        /// <param name="environment">Web host environment.</param>
        public TestStartup(IWebHostEnvironment environment)
            : base(environment) { }

        /// <summary>
        /// Add Alza_WebApi services.
        /// </summary>
        /// <param name="services">Service collection.</param>
        protected override void AddServices(IServiceCollection services)
        {
            // Method left empty to be able to mock (substitute) Alza_WebApi services in tests
        }
    }
}
