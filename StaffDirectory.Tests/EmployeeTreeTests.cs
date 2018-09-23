using FluentAssertions;
using NUnit.Framework;

namespace StaffDirectory.Tests
{
    [TestFixture]
    class EmployeeTreeTests
    {
        [Test]
        public void AddDirectReport_WhenAddingAnEmployeeAsADirectReport_AddsToListOfDirectReports()
        {
            var employeeTree = new EmployeeTree(1, "Amy Reid");

            employeeTree.AddDirectReport(2, "Sachin Kainth");

            employeeTree.DirectReports.Count.Should().Be(1);
            employeeTree.DirectReports[0].Id.Should().Be(2);
            employeeTree.DirectReports[0].Name.Should().Be("Sachin Kainth");
        }
    }
}
