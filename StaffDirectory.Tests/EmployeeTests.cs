using FluentAssertions;
using NUnit.Framework;

namespace StaffDirectory.Tests
{
    [TestFixture]
    class EmployeeTests
    {
        [TestCase(1)]
        [TestCase(null)]
        public void ToString_PrintsEmployee(int? managerId)
        {
            var employee = new Employee(2, "A B", "C D", "a@somewhere.com", managerId);

            employee.ToString().Should().Be(
$@"Id: 2
Name: A B
Job Title: C D
Email Address: a@somewhere.com
Manager Id: {managerId}");
        }

        [Test]
        public void Equals_WhenObjectIsNotEmployee_ReturnsFalse()
        {
            var employee = new Employee(2, "A B", "C D", "a@somewhere.com", 1);

            employee.Equals(new object()).Should().BeFalse();
        }

        [Test]
        public void Equals_WhenObjectIsNull_ReturnsFalse()
        {
            var employee = new Employee(2, "A B", "C D", "a@somewhere.com", 1);

            employee.Equals(null).Should().BeFalse();
        }

        [TestCase(1, "A B", "C D", "a@somewhere.com", 1)]
        [TestCase(2, "A", "C D", "a@somewhere.com", 1)]
        [TestCase(2, "A B", "C", "a@somewhere.com", 1)]
        [TestCase(2, "A B", "C D", "b@somewhere.com", 1)]
        [TestCase(2, "A B", "C D", "a@somewhere.com", null)]
        [TestCase(2, "A B", "C D", "a@somewhere.com", 3)]
        public void Equals_WhenOnePropertyDifferent_ReturnsFalse
            (int id, string name, string jobTitle, string emailAddress, int managerId)
        {
            var employee = new Employee(2, "A B", "C D", "a@somewhere.com", 1);

            employee.Equals(new Employee(id, name, jobTitle, emailAddress, managerId)).Should().BeFalse();
        }

        [Test]
        public void Equals_WhenAllPropertiesTheSame_ReturnsTrue()
        {
            var employee1 = new Employee(2, "A B", "C D", "a@somewhere.com", 1);
            var employee2 = new Employee(2, "A B", "C D", "a@somewhere.com", 1);

            employee1.Equals(employee2).Should().BeTrue();
        }
    }
}
