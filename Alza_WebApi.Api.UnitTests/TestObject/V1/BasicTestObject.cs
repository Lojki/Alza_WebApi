using Alza_WebApi.Api.V1.ResponseModel;
using System;

namespace Alza_WebApi.Api.UnitTests.V1.TestObject
{
    /// <summary>
    /// Contains Response model V1 test objects
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
                        Available = true,
                        Description = "Response model product description"
                    };
                }

                return _product;
            }
        }
    }
}
