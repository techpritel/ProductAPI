using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ProductAPI.Models
{
    public class Products
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }

        [BsonElement("ProductName")]
        [JsonProperty(PropertyName = "productName")]
        public string ProductName { get; set; }

        [BsonElement("Price")]
        [JsonProperty(PropertyName = "price")]
        public double Price { get; set; }

       
        [JsonProperty(PropertyName = "categoryId")]
        [BsonIgnore]
        public string categoryId { get; set; }

        [BsonElement("Category")]
        [JsonProperty(PropertyName = "category")]
        public ProductCategory Category { get; set; }

        [BsonElement("NumberinStocks")]
        [JsonProperty(PropertyName = "numberInStocks")]
        public double NumberInStock { get; set; }

        [BsonElement("Rating")]
        [BsonDefaultValue(0)]
        [JsonProperty(PropertyName = "productRating")]
        public float Rating { get; set; }
    }
}
