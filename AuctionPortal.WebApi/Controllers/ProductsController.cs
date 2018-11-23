using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.BusinessLayer.Facades;
using AuctionPortal.BusinessLayer.Services.Products;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace AuctionPortal.WebApi.Controllers
{
    public class ProductsController : ApiController
    {
        public ProductFacade ProductFac {get;set;}

        // GET api/products/get/{name}
        [HttpGet,Route("api/Products/Get")]
        public async Task<ProductDto> Get(string name)
        {
            var product = await ProductFac.GetProductAsync(name);
            if (product == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            product.Id = Guid.Empty;
            return product;
        }

        [HttpPost,Route("api/Products/Post")]
        public async Task<string> Post([FromBody] ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var productId = await ProductFac.CreateProductWithCategoryNameAsync(productDto, productDto.Category.Name);
            if (productId.Equals(Guid.Empty))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            return $"Created product with id: {productId}";
        }

        [HttpPut,Route("api/Products/Put")]
        public async Task<string> Put(Guid id, [FromBody]ProductDto product)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var success = await ProductFac.EditProductAsync(product);
            if (!success)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return $"Updated product with id: {id}";
        }

    }
}