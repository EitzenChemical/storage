using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace storage.Services
{
    public interface IDataService {
        void Create(string key, string value);
        string GetValue(string key);
        void DeleteValue(string key);
        List<string> GetKeys();
    }

    public class DataService : IDataService
    {
        IDbService _dbService;

        public DataService()
        {
            switch (Program.mode)
            {
                case Mode.InMemory:
                    _dbService = new InMemoryDbService();
                    break;
                case Mode.Persistent:
                    _dbService = new DbService();
                    break;
                default:
                    throw new ArgumentException("Incorrect mode");
            }
        }

        public void Create(string key, string value)
        {
            _dbService?.SetValue(key, value);
        }

        public string GetValue(string key)
        {
           return _dbService?.GetValue(key);
        }

        public void DeleteValue(string key)
        {
            _dbService?.DeleteValue(key);
        }

        public List<string> GetKeys()
        {
            return _dbService?.GetKeys();
        }
    }

    public class TestDataService : IDataService
    {
        IDbService _dbService;

        public TestDataService(ConcurrentDictionary<string, string> collection)
        {
            _dbService = new InMemoryDbService(collection);
        }

        public void Create(string key, string value)
        {
            _dbService?.SetValue(key, value);
        }

        public string GetValue(string key)
        {
            return _dbService?.GetValue(key);
        }

        public void DeleteValue(string key)
        {
            _dbService?.DeleteValue(key);
        }

        public List<string> GetKeys()
        {
            return _dbService?.GetKeys();
        }
    }

    public enum Mode
    {
        InMemory, Persistent
    }
}
