using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductAPI.Models
{
    public class Users
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }

        [BsonElement("UserName")]
        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }

        [BsonElement("Email")]
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [BsonElement("Password")]
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [BsonElement("Hash")]
        [JsonProperty(PropertyName = "hash")]
        public byte[] Hash { get; set; }

        [BsonElement("Salt")]
        [JsonProperty(PropertyName = "salt")]
        public byte[] Salt { get; set; }


        [BsonElement("isAdmin")]
        [JsonProperty(PropertyName = "isAdmin")]
        public bool Admin { get; set; } 
    }
}
