using System;

namespace Alza_WebApi.Api.V1.ResponseModel
{
    /// <summary>
    /// Response model for product.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Image URI.
        /// </summary>
        public Uri ImgUri { get; set; }

        /// <summary>
        /// Price.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Available.
        /// </summary>
        public bool? Available { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Id: '{Id}'; Name: '{Name}'; ImgUri: '{ImgUri}'; Price: '{Price}'; Available: '{Available}'; Description: '{Description}'";
        }
    }
}
