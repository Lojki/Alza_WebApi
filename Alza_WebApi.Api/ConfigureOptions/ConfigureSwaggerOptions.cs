using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Alza_WebApi.Api.ConfigureOptions
{
    /// <summary>
    /// Configure Swagger options
    /// </summary>
    public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider VersionProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="versionProvider">Api version description provider.</param>
        public ConfigureSwaggerOptions(
            IApiVersionDescriptionProvider versionProvider)
        {
            VersionProvider = versionProvider;
        }

        /// <summary>
        /// Configure options.
        /// </summary>
        /// <param name="name">The name of the options instance being configured.</param>
        /// <param name="options">The options instance to configure.</param>
        public void Configure(string name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        /// <summary>
        /// Configure options.
        /// </summary>
        /// <param name="options">The options instance to configure.</param>
        public void Configure(SwaggerGenOptions options)
        {
            // Generate swagger documentation for all API versions
            foreach (var description in VersionProvider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateOpenApiInfo(description));
            }
        }

        /// <summary>
        /// Create <see cref="OpenApiInfo"/> from <see cref="ApiVersionDescription"/>.
        /// </summary>
        /// <param name="versionDescription">Api version description.</param>
        /// <returns>Instance of <see cref="OpenApiInfo"/>.</returns>
        private OpenApiInfo CreateOpenApiInfo(ApiVersionDescription versionDescription)
        {
            var apiInfo = new OpenApiInfo
            {
                Title = "Alza WebApi",
                Version = versionDescription.GroupName,
                Contact = new OpenApiContact
                {
                    Name = "Jakub Lojkásek",
                    Email = "jakub.lojkasek@gmail.com"
                },
                Description = versionDescription.IsDeprecated ? "Deprecated version" : "Supported version"
            };

            return apiInfo;
        }
    }
}
