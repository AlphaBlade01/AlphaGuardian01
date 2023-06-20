using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace AlphaGuardian01.src.Logic.Models
{
    public class UserModel
    {
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.String)]
        public string _id { get; set; }

        [BsonElement("warnings")]
        public List<ModerationsModel> warnings { get; set; }

        [BsonElement("mutes")]
        public List<ModerationsModel> mutes { get; set; }

        [BsonElement("bans")]
        public List<ModerationsModel> bans { get; set; }

        [BsonElement("awards")]
        public List<ModerationsModel> awards { get; set; }
    }
}
