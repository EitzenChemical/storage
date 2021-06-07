using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Concurrent;
using System.Linq;

namespace storage.Services
{
    public interface IDbService
    {
        string GetValue(string key);
        
        void DeleteValue(string key);

        void SetValue(string key, string value);
        List<string> GetKeys();
    }

    public class DbService : IDbService
    {
        private const string dbConnection = "mongodb+srv://pudge:pudge@cluster0.z1fzn.mongodb.net/testDb?retryWrites=true&w=majority";
        private const string dbName = "testDb";
        private const string dbCollectionName = "testCollection";

        IMongoCollection<BsonDocument> _collection;
        IMongoDatabase _database;
        MongoClient _client;
        
        public DbService() {
            _client = new MongoClient(dbConnection);
            _database = _client.GetDatabase(dbName);
            _collection = _database.GetCollection<BsonDocument>(dbCollectionName);     
        }

        public string GetValue(string key) {
            var filter = new BsonDocument("key", key);
            var result = _collection.Find(filter).FirstOrDefault();
            if (result != null)
            {
                return result.GetValue("value").ToString();
            }
            return("No data");
        }

        public void DeleteValue(string key)
        {
            var filter = new BsonDocument("key", key);
            _collection.DeleteOne(filter);
            _collection = _database.GetCollection<BsonDocument>(dbCollectionName);
        }

        public void SetValue(string key, string value)
        {
            var keyExist = _collection.Find(new BsonDocument("key", key)).FirstOrDefault() != null;

            var newDoc = new BsonDocument(new Dictionary<string, string>() {
                { "key", key},
                { "value", value }
            });

            if (keyExist) {
                var filter = new BsonDocument("key", key);
                _collection.ReplaceOne(filter, newDoc);
            }
            else
            {
                _collection.InsertOne(newDoc);
            }
            _collection = _database.GetCollection<BsonDocument>(dbCollectionName);
        }

        public List<string> GetKeys()
        {
            var keyDocs = _collection.Find(_ => true).ToList();
            var result = keyDocs.Select(d => d.GetValue("key").ToString());
            return result.ToList();
        }
    }

    public class InMemoryDbService : IDbService
    {
        ConcurrentDictionary<string, string> _collection;
        public InMemoryDbService()
        {
            _collection = new ConcurrentDictionary<string, string>();
        }

        public InMemoryDbService(ConcurrentDictionary<string, string> collection)
        {
            _collection = collection;
        }

        public string GetValue(string key)
        {
            _collection.TryGetValue(key, out string value);
            return value?.ToString();
        }

        public void DeleteValue(string key)
        {
            _collection.TryRemove(key, out _);
        }

        public void SetValue(string key, string value)
        {
            _collection.AddOrUpdate(key, value, (k, v) => v = value);
        }

        public List<string> GetKeys()
        {
            var result =_collection.Keys.Where(k => _collection.TryGetValue(k, out _) == true);
            return result?.ToList();
        }
    }
}
