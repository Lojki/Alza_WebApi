using System;

namespace Alza_WebApi.Data.Model
{
    /// <summary>
    /// Model for product.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets image URI.
        /// </summary>
        public Uri ImgUri { get; set; }

        /// <summary>
        /// Gets or sets price.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether product is available or not.
        /// </summary>
        public bool Available { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string Description { get; set; }
    }
}
