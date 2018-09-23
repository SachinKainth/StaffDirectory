using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace StaffDirectory.Tests
{
    [TestFixture]
    class AllStaffRepositoryTests
    {
        [TestCase(1, true)]
        [TestCase(2, false)]
        public void IsValidManagerId_ReturnsWhetherValidIdPassedIn(int managerId, bool isValid)
        {
            var repository = new SynchronizedCollection<Employee>
            {
                new Employee(1, "Amy Reid", "Admin Assistant", "amy@acme.com", 1),
            };

            var allStaffRepository = new AllStaffRepository(repository);

            allStaffRepository.IsValidManagerId(managerId).Should().Be(isValid);
        }

        [Test]
        public void GetNextId_WhenThereAreNoEmployees_Returns1()
        {
            var allStaffRepository = new AllStaffRepository(new SynchronizedCollection<Employee>());

            allStaffRepository.GetNextId().Should().Be(1);
        }

        [Test]
        public void GetNextId_WhenThereAreEmployees_ReturnsOneMoreThanTheHighestId()
        {
            var repository = new SynchronizedCollection<Employee>
            {
                new Employee(1, "Amy Reid", "Admin Assistant", "amy@acme.com", 1),
                new Employee(3, "Sachin Kainth", "Software Developer", "sachin@acme.com", 1)
            };

            var allStaffRepository = new AllStaffRepository(repository);

            allStaffRepository.GetNextId().Should().Be(4);
        }
    }
}
