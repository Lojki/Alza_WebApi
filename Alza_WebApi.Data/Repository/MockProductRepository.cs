using Alza_WebApi.Data.Interface;
using Alza_WebApi.Data.Mock;
using Alza_WebApi.Data.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Alza_WebApi.Data.Repository
{
    /// <summary>
    /// Mock product repository.
    /// </summary>
    public class MockProductRepository : IProductRepository
    {
        /// <summary>
        /// Get all <see cref="Product"/>s.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains all <see cref="Product"/>s.</returns>
        public Task<IEnumerable<Product>> GetAllProducts(CancellationToken cancellationToken)
        {
            return Task.FromResult(MockData.Products);
        }

        /// <summary>
        /// Get available <see cref="Product"/>s.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains elements with available <see cref="Product"/>s.</returns>
        public Task<IEnumerable<Product>> GetAvailableProducts(CancellationToken cancellationToken)
        {
            return Task.FromResult(MockData.Products.Where(p => p.Available));
        }

        /// <summary>
        /// Get <see cref="Product"/> by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the product to be found.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="Product"/> if found; otherwise <i>null</i>.</returns>
        public Task<Product> GetProduct(int id, CancellationToken cancellationToken)
        {
            return Task.FromResult(MockData.Products.FirstOrDefault(p => p.Id == id));
        }

        /// <summary>
        /// Update <see cref="Product"/> description.
        /// </summary>
        /// <param name="id">The id of the product to be updated.</param>
        /// <param name="description">New product description.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><para>A value that indicates whether update was successfull or not.</para>
        /// <para>An error message in case update fails.</para></returns>
        public Task<(bool updated, string errorMessage)> UpdateProductDescription(int id, string description, CancellationToken cancellationToken)
        {
            var product = MockData.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return Task.FromResult((false, "Not found"));
            }

            try
            {
                product.Description = description;

                return Task.FromResult((true, string.Empty));
            }
            catch
            {
                return Task.FromResult((false, "System.Exception - Message"));
            }
        }
    }
}
