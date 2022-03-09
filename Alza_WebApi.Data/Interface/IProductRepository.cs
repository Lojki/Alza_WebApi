using Alza_WebApi.Data.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Alza_WebApi.Data.Interface
{
    /// <summary>
    /// <see cref="Product"/> repository interface.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Get all <see cref="Product"/>s.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains all <see cref="Product"/>s.</returns>
        Task<IEnumerable<Product>> GetAllProducts(CancellationToken cancellationToken);

        /// <summary>
        /// Get available <see cref="Product"/>s.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains elements with available <see cref="Product"/>s.</returns>
        Task<IEnumerable<Product>> GetAvailableProducts(CancellationToken cancellationToken);

        /// <summary>
        /// Get <see cref="Product"/> by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the product to be found.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="Product"/> if found; otherwise <i>null</i>.</returns>
        Task<Product> GetProduct(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Update <see cref="Product"/> description.
        /// </summary>
        /// <param name="id">The id of the product to be updated.</param>
        /// <param name="description">New product description.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><para>A value that indicates whether update was successfull or not.</para>
        /// <para>An error message in case update fails.</para></returns>
        Task<(bool updated, string errorMessage)> UpdateProductDescription(int id, string description, CancellationToken cancellationToken);
    }
}
