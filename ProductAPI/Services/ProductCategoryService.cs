using MongoDB.Driver;
using ProductAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProductRatingApi.Services
{
    public class ProductCategoryService
    {
        private readonly IMongoCollection<ProductCategory> _productCategory;

        public ProductCategoryService(IProductstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _productCategory = database.GetCollection<ProductCategory>(settings.ProductsCategoryCollectionName);
        }

        public List<ProductCategory> Get() =>
            _productCategory.Find(productCategory => true).ToList();

        public ProductCategory Get(string id) =>
            _productCategory.Find<ProductCategory>(productCategory => productCategory.Id == id).FirstOrDefault();

        public ProductCategory Create(ProductCategory productCategory)
        {
            _productCategory.InsertOne(productCategory);
            return productCategory;
        }

        public void Update(string id, ProductCategory productCategoryIn) =>
            _productCategory.ReplaceOne(productCategory => productCategory.Id == id, productCategoryIn);

        public void Remove(ProductCategory productCategoryIn) =>
            _productCategory.DeleteOne(productCategory => productCategory.Id == productCategoryIn.Id);

        public void Remove(string id) =>
            _productCategory.DeleteOne(productCategory => productCategory.Id == id);
    }
}
