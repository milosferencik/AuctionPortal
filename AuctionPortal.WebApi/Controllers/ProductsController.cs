using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.BusinessLayer.Services.Products;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace AuctionPortal.WebApi.Controllers
{
    public class ProductsController : ApiController
    {
        public ProductService ProductServ {get;set;}

        // GET api/products/{name}
        public async Task<ProductDto> Get(string name)
        {
            var product = await ProductServ.GetProductByNameAsync(name);
            if (product == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            product.Id = Guid.Empty;
            return product;
        }
    }
}