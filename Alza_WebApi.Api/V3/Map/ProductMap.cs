using Alza_WebApi.Api.V3.ResponseModel;

namespace Alza_WebApi.Api.V3.Map
{
    /// <summary>
    /// Mapper for <see cref="Data.Model.Product"/>
    /// </summary>
    internal static class ProductMap
    {

        /// <summary>
        /// Helper mapper method to map internal <see cref="Data.Model.Product"/> model to public <see cref="Product"/> response model.
        /// </summary>
        /// <param name="product">Internal <see cref="Data.Model.Product"/> model.</param>
        /// <returns>Public <see cref="Product"/> response model.</returns>
        internal static Product MapToResponseModel(this Data.Model.Product product)
        {
            if (product == null)
            {
                return null;
            }

            return new Product
            {
                Id = product.Id,
                Name = product.Name,
                ImgUri = product.ImgUri,
                Price = product.Price,
                Description = product.Description
            };
        }
    }
}
