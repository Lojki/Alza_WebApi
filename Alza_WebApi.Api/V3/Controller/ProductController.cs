using Alza_WebApi.Api.V3.Map;
using Alza_WebApi.Api.V3.ResponseModel;
using Alza_WebApi.Data.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Alza_WebApi.Api.V3.Controller
{
    /// <summary>
    /// Product controller.
    /// </summary>
    [ApiController]
    [ApiVersion("3")]
    [Route("v{version:apiVersion}/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="productRepository">An instance of <see cref="IProductRepository"/>.</param>
        /// <param name="logger">An instance od <see cref="ILogger"/>.</param>
        public ProductController(IProductRepository productRepository, ILogger<ProductController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        /// <summary>
        /// Get <see cref="Product"/> by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the product to be found.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="Product"/> if found; otherwise <see cref="NotFoundResult"/>.</returns>
        [HttpGet("{id:int:required:min(1)}")]
        [SwaggerOperation("Get product", "Get product by id")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProduct(int id, CancellationToken cancellationToken)
        {
            LogCall(nameof(GetProduct), id);

            var product = await _productRepository.GetProduct(id, cancellationToken);

            if (product == null)
            {
                return NotFound();
            }

            return product.MapToResponseModel();
        }

        /// <summary>
        /// Get available <see cref="Product"/>s.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains elements with available <see cref="Product"/>s.</returns>
        [HttpGet("AvailableProducts")]
        [SwaggerOperation("Get available products", "Get available products")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        public async Task<IEnumerable<Product>> GetAvailableProducts(CancellationToken cancellationToken)
        {
            LogCall(nameof(GetAvailableProducts));

            return (await _productRepository.GetAvailableProducts(cancellationToken)).Select(ProductMap.MapToResponseModel);
        }

        /// <summary>
        /// Update <see cref="Product"/> description.
        /// </summary>
        /// <param name="id">The id of the product to be updated.</param>
        /// <param name="description">New product description.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><para><see cref="NoContentResult"/> if product was found and description updated successfully.</para>
        /// <para>Otherwise <see cref="NotFoundResult"/> or <see cref="BadRequestResult"/> with error message.</para></returns>
        [HttpPatch("{id:int:required:min(1)}/UpdateDescription")]
        [SwaggerOperation("Update product description", "Update product description")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProductDescription(int id, [FromBody] string description, CancellationToken cancellationToken)
        {
            LogCall(nameof(UpdateProductDescription), id, description);

            var productCheck = await _productRepository.GetProduct(id, cancellationToken);

            if (productCheck == null)
            {
                return NotFound();
            }

            var (updated, errorMessage) = await _productRepository.UpdateProductDescription(id, description, cancellationToken);

            if (updated)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(errorMessage);
            }
        }

        /// <summary>
        /// Update <see cref="Product"/> description.
        /// </summary>
        /// <param name="product">The product with new description to be updated.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><para><see cref="NoContentResult"/> if product was found and description updated successfully.</para>
        /// <para>Otherwise <see cref="NotFoundResult"/> or <see cref="BadRequestResult"/> with error message.</para>
        /// <para>If model validation fails <see cref="BadRequestResult"/> is returned.</para></returns>
        [HttpPatch("UpdateDescription")]
        [SwaggerOperation("Update product description", "Update product description")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProductDescription([FromBody] Product product, CancellationToken cancellationToken)
        {
            LogCall(nameof(UpdateProductDescription), product);

            var productCheck = await _productRepository.GetProduct(product.Id, cancellationToken);

            if (productCheck == null)
            {
                return NotFound();
            }

            var (updated, errorMessage) = await _productRepository.UpdateProductDescription(product.Id, product.Description, cancellationToken);

            if (updated)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(errorMessage);
            }
        }

        /// <summary>
        /// Log incoming WebApi call.
        /// </summary>
        /// <param name="action">Action name.</param>
        /// <param name="parameters">Input parameters.</param>
        private void LogCall(string action, params object[] parameters)
        {
            var apiVersion = HttpContext.GetRequestedApiVersion().ToString();

            _logger.LogInformation($"'{nameof(ProductController)}' api version '{apiVersion}' action '{action}' called with params: '{string.Join("; ", parameters.Where(p => p != null).Select(p => $"({p.GetType().Name}) '{p}'"))}' at '{DateTimeOffset.UtcNow.ToLocalTime()}'.");
        }
    }
}
