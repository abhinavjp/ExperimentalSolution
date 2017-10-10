using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA.Test
{
    [TestClass]
    public class JustLikeTest
    {
        [TestMethod]
        public void GroupTester()
        {
            var randomGroupIds = new[] { 1, 2, 3, 4, 5 };
            var randomNames = new[] { "Test", "Dev", "Namer", "Prod" };
            var testList = new List<TestObject>();

            var random = new Random();

            Parallel.For(1, 101, (index, state) =>
            {
                var name = randomNames[random.Next(randomNames.Length)];
                var groupId = random.Next(randomGroupIds.Length);

                lock ("JustTestingLock")
                {
                    testList.Add(new TestObject(index, groupId + 1, $"{name}{index}"));
                }
            });

            var groupedList = testList.GroupBy(g => new TestGrouper
            {
                GroupId = g.GroupId
            }).ToList();
        }
    }

    public class TestObject
    {
        public TestObject()
        {

        }
        public TestObject(int id, int groupId, string name)
        {
            Id = id;
            GroupId = groupId;
            Name = name;
        }
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public class TestGrouper
    {
        public int GroupId { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is TestGrouper) {
                var test = (obj as TestGrouper);
                return GroupId == test?.GroupId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return GroupId;
        }
    }
}
