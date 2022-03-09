using Alza_WebApi.Data.Model;
using System;

namespace Alza_WebApi.Api.UnitTests.TestObject
{
    /// <summary>
    /// Contains Data test objects
    /// </summary>
    public static class DataTestObject
    {
        private static Product _product;
        public static Product Product
        {
            get
            {
                if (_product == null)
                {
                    _product = new Product
                    {
                        Id = 1,
                        Name = "Available product name",
                        ImgUri = new Uri("http://www.test.tld/image?id=1"),
                        Price = (decimal)123.45,
                        Available = true,
                        Description = "Available product description"
                    };
                }

                return _product;
            }
        }

        private static Product _notAvailableProduct;
        public static Product NotAvailableProduct
        {
            get
            {
                if (_notAvailableProduct == null)
                {
                    _notAvailableProduct = new Product
                    {
                        Id = 2,
                        Name = "Not available product name",
                        ImgUri = new Uri("http://www.test.tld/image?id=2"),
                        Price = (decimal)543.21,
                        Available = false,
                        Description = "Not available product description"
                    };
                }

                return _notAvailableProduct;
            }
        }
    }
}
