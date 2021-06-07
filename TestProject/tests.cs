using NUnit.Framework;
using storage.Services;
using System.Collections.Concurrent;

namespace TestProject
{
    public class Tests
    {
        private TestDataService _testDataService;
        private ConcurrentDictionary<string, string> _collection = new ConcurrentDictionary<string, string>
        {
            ["a"] = "1",
            ["b"] = "2",
            ["c"] = "3",
            ["d"] = "4",
            ["e"] = "5"
        };

        [SetUp]
        public void Setup()
        {
            _testDataService = new TestDataService(_collection);
        }

        [Test, Order(1)]
        [TestCase("a", "1")]
        [TestCase("b", "2")]
        [TestCase("c", "3")]
        [TestCase("j", null)]
        public void GetValueTest(string key, string value)
        {
            var result = _testDataService.GetValue(key);
            Assert.IsTrue(result == value);
        }

        [Test, Order(2)]
        [TestCase("b", "1")]
        [TestCase("a", "2")]
        [TestCase("j", "4")]
        public void SetValueTest(string key, string value)
        {
            _testDataService.Create(key, value);
            _collection.TryGetValue(key, out string result);
            Assert.IsTrue(result == value);
        }

        [Test, Order(3)]
        [TestCase("b")]
        [TestCase("a")]
        [TestCase("j")]
        public void DeleteValueTest(string key)
        {
            _testDataService.DeleteValue(key);
            _collection.TryGetValue(key, out string result);
            Assert.IsTrue(result == null);
        }

        [Test, Order(4)]
        public void GetKeysTest()
        {
            var result = _testDataService.GetKeys();
            Assert.IsTrue(result.Count == _collection.Count);
        }

    }
}