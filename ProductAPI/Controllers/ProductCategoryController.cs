using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;
using ProductRatingApi.Services;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly ProductCategoryService _productCategoryService;

        public ProductCategoryController(ProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        [HttpGet]
        public ActionResult<List<ProductCategory>> Get() =>
            _productCategoryService.Get();

        [HttpGet("{id:length(24)}", Name = "GetProductCategory")]
        public ActionResult<ProductCategory> Get(string id)
        {
            var productCategory = _productCategoryService.Get(id);

            if (productCategory == null)
            {
                return NotFound();
            }

            return productCategory;
        }

        [HttpPost]
        public ActionResult<ProductCategory> Create(ProductCategory productCategory)
        {
            _productCategoryService.Create(productCategory);

            return CreatedAtRoute("GetProductCategory", new { id = productCategory.Id.ToString() }, productCategory);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, ProductCategory productCategoryIn)
        {
            var productCategory = _productCategoryService.Get(id);

            if (productCategory == null)
            {
                return NotFound();
            }

            _productCategoryService.Update(id, productCategoryIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var productCategory = _productCategoryService.Get(id);

            if (productCategory == null)
            {
                return NotFound();
            }

            _productCategoryService.Remove(productCategory.Id);

            return NoContent();
        }
    }
}