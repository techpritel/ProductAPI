using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;
using ProductRatingApi.Services;
using System.Collections.Generic;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    [EnableCors("CorsPolicy")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsService _productService;
        private readonly ProductCategoryService _productCategoryService;

        public ProductsController(ProductsService productService,ProductCategoryService productCategoryService)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<List<Products>> Get() =>
            _productService.Get();

        [HttpGet("{id:length(24)}", Name = "Getproduct")]
        [AllowAnonymous]
        public ActionResult<Products> Get(string id)
        {
            var product = _productService.Get(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPost]       
        public ActionResult<Products> Create(Products product)
        {
           //var _productCategoryService = new ProductCategoryService() ;
            var productCategory = _productCategoryService.Get(product.categoryId);

            if (productCategory == null)
            {
                return NotFound();
            }
            product.Category = productCategory;
           // product.Category ={ product.Category.Id=prouctca}
            _productService.Create(product);

            return CreatedAtRoute("Getproduct", new { id = product.Id.ToString() }, product);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Products productIn)
        {
            var product = _productService.Get(id);

            if (product == null)
            {
                return NotFound();
            }
            var productCategory = _productCategoryService.Get(productIn.categoryId);

            if (productCategory == null)
            {
                return NotFound();
            }
            productIn.Id = id;
            productIn.Category = productCategory;

            _productService.Update(id, productIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var product = _productService.Get(id);

            if (product == null)
            {
                return NotFound();
            }

            _productService.Remove(product.Id);

            return NoContent();
        }
    }
}