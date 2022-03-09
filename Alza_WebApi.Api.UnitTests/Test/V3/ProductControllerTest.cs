using Alza_WebApi.Api.UnitTests.Extension;
using Alza_WebApi.Api.UnitTests.Server;
using Alza_WebApi.Api.UnitTests.TestObject;
using Alza_WebApi.Api.UnitTests.V3.TestObject;
using Alza_WebApi.Api.V1.ResponseModel;
using Alza_WebApi.Data.Interface;
using Alza_WebApi.UnitTests.Shared;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Alza_WebApi.Api.UnitTests.Test.V3
{
    public class ProductControllerTests : BackendTestBase
    {
        private readonly string _version = "3";

        public ProductControllerTests(ITestOutputHelper output)
            : base(output) { }

        [Fact]
        public async Task ProductController_GetProduct_NotFound()
        {
            // arrange
            using (var server = CreateServer<TestStartup>((_, services) =>
            {
                services.AddSingleton(Substitute.For<IProductRepository>());
            }))
            using (var client = server.CreateClient())
            {
                // act & assert
                var response = await client.GetAsync($"v{_version}/Product/1");
                await AssertStatus(response, HttpStatusCode.NotFound);
            }
        }

        [Fact]
        public async Task ProductController_GetProduct_OK()
        {
            // arrange
            var product = DataTestObject.Product;

            var productRepositoryMock = Substitute.For<IProductRepository>();
            productRepositoryMock.GetProduct(Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns(product);

            using (var server = CreateServer<TestStartup>((_, services) =>
            {
                services.AddSingleton(productRepositoryMock);
            }))
            using (var client = server.CreateClient())
            {
                // act & assert
                var response = await client.GetAsync($"v{_version}/Product/1");
                await AssertStatus(response);

                var result = await response.Content.ReadAsAsync<Product>();
                Assert.NotNull(result);

                Assert.Equal(product.Name, result.Name);
            }
        }

        [Fact]
        public async Task ProductController_GetAllProducts_BadRequest()
        {
            // arrange
            using (var server = CreateServer<TestStartup>())
            using (var client = server.CreateClient())
            {
                // act & assert
                var response = await client.GetAsync($"v{_version}/Product/AllProducts");
                await AssertStatus(response, HttpStatusCode.BadRequest);
            }
        }

        [Fact]
        public async Task ProductController_GetAllProducts_Removed()
        {
            // arrange
            using (var server = CreateServer<TestStartup>())
            using (var client = server.CreateClient())
            {
                // act & assert
                var response = await client.GetAsync($"swagger/v{_version}/swagger.json");
                await AssertStatus(response);

                var result = await response.Content.ReadAsAsync<SwaggerResponse>();

                Assert.False(result.Paths.ContainsKey("AllProducts"));
            }
        }

        [Fact]
        public async Task ProductController_GetAvailableProducts_OK()
        {
            // arrange
            var products = new List<Data.Model.Product>
            {
                DataTestObject.Product
            };

            var productRepositoryMock = Substitute.For<IProductRepository>();
            productRepositoryMock.GetAvailableProducts(Arg.Any<CancellationToken>())
                .Returns(products);

            using (var server = CreateServer<TestStartup>((_, services) =>
            {
                services.AddSingleton(productRepositoryMock);
            }))
            using (var client = server.CreateClient())
            {
                // act & assert
                var response = await client.GetAsync($"v{_version}/Product/AvailableProducts");
                await AssertStatus(response);

                var result = await response.Content.ReadAsAsync<IEnumerable<Product>>();
                Assert.NotNull(result);
                Assert.NotEmpty(result);
                Assert.Single(result);

                Assert.Equal(products.First().Name, result.First().Name);
            }
        }

        [Fact]
        public async Task ProductController_UpdateProductDescription_NotFound()
        {
            // arrange
            using (var server = CreateServer<TestStartup>((_, services) =>
            {
                services.AddSingleton(Substitute.For<IProductRepository>());
            }))
            using (var client = server.CreateClient())
            {
                // act & assert
                var response = await client.PatchAsJsonAsync($"v{_version}/Product/1/UpdateDescription", "New description");
                await AssertStatus(response, HttpStatusCode.NotFound);
            }
        }

        [Fact]
        public async Task ProductController_UpdateProductDescription_UpdateFailed_BadRequest()
        {
            // arrange
            var product = DataTestObject.Product;
            var errorMessage = "Bad request - error message";

            var productRepositoryMock = Substitute.For<IProductRepository>();
            productRepositoryMock.GetProduct(Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns(product);
            productRepositoryMock.UpdateProductDescription(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns((false, errorMessage));

            using (var server = CreateServer<TestStartup>((_, services) =>
            {
                services.AddSingleton(productRepositoryMock);
            }))
            using (var client = server.CreateClient())
            {
                // act & assert
                var response = await client.PatchAsJsonAsync($"v{_version}/Product/1/UpdateDescription", "New description");
                await AssertStatus(response, HttpStatusCode.BadRequest);

                var badRequestMessage = await response.Content.ReadAsStringAsync();
                Assert.Equal(errorMessage, badRequestMessage.Trim('"'));
            }
        }

        [Fact]
        public async Task ProductController_UpdateProductDescription_NoContent()
        {
            // arrange
            var product = DataTestObject.Product;

            var productRepositoryMock = Substitute.For<IProductRepository>();
            productRepositoryMock.GetProduct(Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns(product);
            productRepositoryMock.UpdateProductDescription(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns((true, string.Empty));

            using (var server = CreateServer<TestStartup>((_, services) =>
            {
                services.AddSingleton(productRepositoryMock);
            }))
            using (var client = server.CreateClient())
            {
                // act & assert
                var response = await client.PatchAsJsonAsync($"v{_version}/Product/1/UpdateDescription", "New description");
                await AssertStatus(response, HttpStatusCode.NoContent);
            }
        }

        [Fact]
        public async Task ProductController_UpdateProductDescription_WithProductParam_ModelInvalid_BadRequest()
        {
            // arrange
            var product = new Product
            {
                Id = 1
            };

            using (var server = CreateServer<TestStartup>((_, services) =>
            {
                services.AddSingleton(Substitute.For<IProductRepository>());
            }))
            using (var client = server.CreateClient())
            {
                // act & assert
                var response = await client.PatchAsJsonAsync($"v{_version}/Product/UpdateDescription", product);
                await AssertStatus(response, HttpStatusCode.BadRequest);
            }
        }

        [Fact]
        public async Task ProductController_UpdateProductDescription_WithProductParam_UpdateFailed_BadRequest()
        {
            // arrange
            var product = DataTestObject.Product;
            var errorMessage = "Bad request - error message";

            var productRepositoryMock = Substitute.For<IProductRepository>();
            productRepositoryMock.GetProduct(Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns(product);
            productRepositoryMock.UpdateProductDescription(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns((false, errorMessage));

            using (var server = CreateServer<TestStartup>((_, services) =>
            {
                services.AddSingleton(productRepositoryMock);
            }))
            using (var client = server.CreateClient())
            {
                // act & assert
                var response = await client.PatchAsJsonAsync($"v{_version}/Product/UpdateDescription", product);
                await AssertStatus(response, HttpStatusCode.BadRequest);

                var badRequestMessage = await response.Content.ReadAsStringAsync();
                Assert.Equal(errorMessage, badRequestMessage.Trim('"'));
            }
        }

        [Fact]
        public async Task ProductController_UpdateProductDescription_WithProductParam_NotFound()
        {
            // arrange
            using (var server = CreateServer<TestStartup>((_, services) =>
            {
                services.AddSingleton(Substitute.For<IProductRepository>());
            }))
            using (var client = server.CreateClient())
            {
                // act & assert
                var response = await client.PatchAsJsonAsync($"v{_version}/Product/UpdateDescription", BasicTestObject.Product);
                await AssertStatus(response, HttpStatusCode.NotFound);
            }
        }

        [Fact]
        public async Task ProductController_UpdateProductDescription_WithProductParam_NoContent()
        {
            // arrange
            var product = DataTestObject.Product;

            var productRepositoryMock = Substitute.For<IProductRepository>();
            productRepositoryMock.GetProduct(Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns(product);
            productRepositoryMock.UpdateProductDescription(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns((true, string.Empty));

            using (var server = CreateServer<TestStartup>((_, services) =>
            {
                services.AddSingleton(productRepositoryMock);
            }))
            using (var client = server.CreateClient())
            {
                // act & assert
                var response = await client.PatchAsJsonAsync($"v{_version}/Product/UpdateDescription", BasicTestObject.Product);
                await AssertStatus(response, HttpStatusCode.NoContent);
            }
        }
    }

    [DataContract]
    public class SwaggerResponse
    {
        [DataMember(Name = "paths")]
        public IDictionary<string, IDictionary<string, Path>> Paths { get; set; }
    }

    [DataContract]
    public class Path
    {
        [DataMember(Name = "parameters")]
        public Parameter[] Parameters { get; set; }
    }

    [DataContract]
    public class Parameter
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "in")]
        public string In { get; set; }

        [DataMember(Name = "required")]
        public bool Required { get; set; }
    }
}
