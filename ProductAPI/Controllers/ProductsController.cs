using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;
using ProductRatingApi.Services;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
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
        public ActionResult<List<Products>> Get() =>
            _productService.Get();

        [HttpGet("{id:length(24)}", Name = "Getproduct")]
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