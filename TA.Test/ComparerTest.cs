using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TA.PracticeService.ComparerHelper;
using System.Collections.Generic;

namespace TA.Test
{
    [TestClass]
    public class ComparerTest
    {
        [TestMethod]
        public void CompareTest()
        {
            var test1 = new TestObject1
            {
                Id = null,
                Forename = "Lionel",
                Surname = "Messi",
                DateOfBirth = new DateTime(1985, 4, 4)
            };
            var test2 = new TestObject2
            {
                Id = 1,
                FirstName = "Andres",
                LastName = "Iniesta",
                DateOfBirth = new DateTime(1987, 4, 4)
            };
            var mapOptions = new List<MapOptions<TestObject1, TestObject2>>
            {
                new MapOptions<TestObject1, TestObject2>(t=>t.Forename,t=>t.FirstName)
            };
            var comparerResult = test1.Compare(test2, mapOptions);

        }
    }

    public class TestObject1
    {
        public int? Id { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        [IgnoreComparision]
        public DateTime DateOfBirth { get; set; }
        public TestObject1 TestObject { get; set; }
    }

    public class TestObject2
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
