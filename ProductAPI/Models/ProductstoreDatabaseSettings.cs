using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductAPI.Models
{
    public class ProductstoreDatabaseSettings : IProductstoreDatabaseSettings
    {
        public string ProductsCategoryCollectionName { get; set; }
        public string ProductsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
    public interface IProductstoreDatabaseSettings
    {
        string ProductsCategoryCollectionName { get; set; }
        string ProductsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
