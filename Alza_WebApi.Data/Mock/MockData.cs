using Alza_WebApi.Data.Model;
using System;
using System.Collections.Generic;

namespace Alza_WebApi.Data.Mock
{
#pragma warning disable S1075 // URIs should not be hardcoded
    internal static class MockData
    {
        private static readonly IEnumerable<Product> _products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "First product",
                ImgUri = new Uri("http://subdomain.domain.tld/image?id=1"),
                Price = (decimal)1.11,
                Available = true,
                Description = "First product description"
            },
            new Product
            {
                Id = 2,
                Name = "Second product",
                ImgUri = new Uri("http://subdomain.domain.tld/image?id=2"),
                Price = (decimal)2.22,
                Available = false,
                Description = "Second product description"
            },
            new Product
            {
                Id = 3,
                Name = "Third product",
                ImgUri = new Uri("http://subdomain.domain.tld/image?id=3"),
                Price = (decimal)3.33,
                Available = true,
                Description = "Third product description"
            },
            new Product
            {
                Id = 4,
                Name = "Fourth product",
                ImgUri = new Uri("http://subdomain.domain.tld/image?id=4"),
                Price = (decimal)4.44,
                Available = true,
                Description = "Fourth product description"
            },
            new Product
            {
                Id = 5,
                Name = "Fifth product",
                ImgUri = new Uri("http://subdomain.domain.tld/image?id=5"),
                Price = (decimal)5.55,
                Available = false,
                Description = "Fifth product description"
            }
        };

        internal static IEnumerable<Product> Products
        {
            get
            {
                return _products;
            }
        }
    }
#pragma warning restore S1075 // URIs should not be hardcoded
}
