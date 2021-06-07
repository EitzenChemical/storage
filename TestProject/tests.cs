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
        private int ElemsCount => _collection.Count;
        [SetUp]
        public void Setup()
        {
            _testDataService = new TestDataService(_collection);
        }

        [Test]
        public void GetValueTest()
        {
            var result = _testDataService.GetValue("a");
            Assert.Pass();
        }
    }
}