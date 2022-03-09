using Alza_WebApi.Api.V3.ResponseModel;
using System;

namespace Alza_WebApi.Api.UnitTests.V3.TestObject
{
    /// <summary>
    /// Contains Response model V3 test objects
    /// </summary>
    public static class BasicTestObject
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
                        Name = "Response model product",
                        ImgUri = new Uri("http://www.test.tld/image?id=1"),
                        Price = (decimal)123.45,
                        Description = "Response model product description"
                    };
                }

                return _product;
            }
        }
    }
}
