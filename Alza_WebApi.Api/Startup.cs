using System;
using System.IO;
using System.Linq;
using Alza_WebApi.Api.ConfigureOptions;
using Alza_WebApi.Data.Interface;
using Alza_WebApi.Data.Repository;
using Autofac;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Alza_WebApi.Api
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        private IWebHostEnvironment Environment { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="environment">Web host environment.</param>
        public Startup(IWebHostEnvironment environment)
        {
            Environment = environment;
        }

        /// <summary>
        /// Configure services.
        /// </summary>
        /// <param name="services">Service collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
                {
                    options.Filters.Add(new ProducesAttribute("application/json"));
                    if (!Environment.IsDevelopment())
                    {
                        options.Filters.Add(new RequireHttpsAttribute());
                    }

                    var noContentFormatter = options.OutputFormatters.OfType<HttpNoContentOutputFormatter>().FirstOrDefault();
                    if (noContentFormatter != null)
                    {
                        noContentFormatter.TreatNullValueAsNoContent = false;
                    }
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    var builtInFactory = options.InvalidModelStateResponseFactory;

                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var logger = context.HttpContext.RequestServices
                            .GetService<ILoggerFactory>()
                            .CreateLogger("BadRequestLogger");

                        var errorMessages = context.ModelState.SelectMany(e => e.Value.Errors.Select(ee => ee.ErrorMessage));

                        logger.LogDebug(string.Join(", ", errorMessages));

                        return builtInFactory(context);
                    };
                })
                .AddNewtonsoftJson(options =>
                {
                    if (!Environment.IsDevelopment())
                    {
                        options.AllowInputFormatterExceptionMessages = false;
                    }

                    var serializerSettings = options.SerializerSettings;
                    serializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    serializerSettings.Converters.Add(new StringEnumConverter());
                    serializerSettings.Converters.Add(new VersionConverter());
                    serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    serializerSettings.NullValueHandling = NullValueHandling.Ignore;
                })
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssemblyContaining<Startup>();
                    options.DisableDataAnnotationsValidation = true;
                })
                .AddApplicationPart(typeof(Startup).Assembly);
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(3, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();

                c.SchemaGeneratorOptions.SchemaIdSelector = type => type.FullName;

                c.IgnoreObsoleteActions();
                c.IgnoreObsoleteProperties();

                var docsXmlPath = Path.Combine(AppContext.BaseDirectory, $"{typeof(Startup).Assembly.GetName().Name}.xml");
                c.IncludeXmlComments(docsXmlPath);

            });
            services.AddFluentValidationRulesToSwagger();
            services.ConfigureOptions<ConfigureSwaggerOptions>();
            services.AddSwaggerGenNewtonsoftSupport();

            AddServices(services);
        }

        /// <summary>
        /// Add Alza_WebApi services.
        /// </summary>
        /// <param name="services">Service collection.</param>
        protected virtual void AddServices(IServiceCollection services)
        {
            services.AddSingleton<IProductRepository, MockProductRepository>();
        }

        /// <summary>
        /// Configure container.
        /// </summary>
        /// <param name="builder">Container builder.</param>
        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
        }

        /// <summary>
        /// Configure HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="env">Web host environment.</param>
        /// <param name="versionProvider">Api version description provider.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider versionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    foreach (var version in versionProvider.ApiVersionDescriptions.Select(avd => avd.GroupName).Reverse())
                    {
                        options.SwaggerEndpoint($"{version}/swagger.json", $"Alza WebApi - {version}");
                    }

                    options.InjectStylesheet("Styles/SwaggerUICustom.css");
                    options.DisplayRequestDuration();
                    options.DisplayOperationId();
                });
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
