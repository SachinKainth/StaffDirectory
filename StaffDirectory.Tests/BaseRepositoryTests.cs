using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace StaffDirectory.Tests
{
    internal class Sut : BaseRepository
    {
        public Sut(SynchronizedCollection<Employee> repository) : base(repository)
        {
        }
    }

    [TestFixture]
    class BaseRepositoryTests
    {
        [Test]
        public void Get_WhenRequestingListOfStaffMembers_ReturnsThem()
        {
            var repository = new SynchronizedCollection<Employee>
            {
                new Employee(2, "Amy Reid", "Admin Assistant", "amy@acme.com", 1),
                new Employee(3, "Sachin Kainth", "Software Developer", "sachin@acme.com", 1)
            };

            var baseReposity = new Sut(repository);

            baseReposity.Get().Should().BeEquivalentTo(repository);
        }

        [Test]
        public void Add_WhenAddingStaffMemberToReposity_AddsMember()
        {
            var repository = new SynchronizedCollection<Employee>();

            var baseReposity = new Sut(repository);

            baseReposity.Add(new Employee(2, "Amy Reid", "Admin Assistant", "amy@acme.com", 1));

            baseReposity.Get().Should().BeEquivalentTo(repository);
        }
    }
}
