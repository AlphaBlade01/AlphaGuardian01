using AlphaGuardian01.src.Logic.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaGuardian01.src.Logic
{
    class DataHandler
    {
        readonly MongoClient _client;
        readonly IMongoDatabase _discord_db;
        readonly IMongoCollection<UserModel> _users_collection;
        readonly string _db_login = Environment.GetEnvironmentVariable("DB_LOGIN");

        private UserModel initialiseData(string userId)
        {
            return new UserModel
            {
                _id = userId,
                warnings = new List<ModerationsModel>(),
                mutes = new List<ModerationsModel>(),
                awards = new List<ModerationsModel>(),
                bans = new List<ModerationsModel>(),
            };
        }

        public DataHandler()
        {
            _client = new MongoClient(_db_login);
            _discord_db = _client.GetDatabase("discord");
            _users_collection = _discord_db.GetCollection<UserModel>("users");
        }

        public async Task<UserModel> GetData(string userId)
        {
            IAsyncCursor<UserModel> user_data = await _users_collection.FindAsync<UserModel>(new BsonDocument("_id", userId));
            UserModel extracted_data = user_data.FirstOrDefault() ?? initialiseData(userId);

            return extracted_data;
        }

        public async Task<bool> SetData(string userId, UserModel data)
        {
            var filter = Builders<UserModel>.Filter.Eq(doc => doc._id, userId);
            IAsyncCursor<UserModel> found = await _users_collection.FindAsync(filter);

            try
            {
                if (found.Any())
                    await _users_collection.ReplaceOneAsync(filter, data);
                else
                    await _users_collection.InsertOneAsync(data);
                return true;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
