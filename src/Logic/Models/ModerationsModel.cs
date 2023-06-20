using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace AlphaGuardian01.src.Logic.Models
{
    public class ModerationsModel
    {
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        [BsonElement("description")]
        [BsonRepresentation(BsonType.String)]
        public string description { get; set; }

        [BsonElement("moderator_id")]
        [BsonRepresentation(BsonType.String)]
        public string moderator_id { get; set; }

        [BsonElement("timestamp")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime timestamp { get; set; }

        [BsonElement("length")]
        [BsonRepresentation(BsonType.Int32)]
        public int? length { get; set; }
    }
}
