using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Moq.Sequences;
using NUnit.Framework;

namespace StaffDirectory.Tests
{
    [TestFixture]
    class EmployeeTreeParserTests
    {
        private Mock<IConsole> _consoleMock;

        private EmployeeTreeParser _employeeTreeParser;

        [SetUp]
        public void Setup()
        {
            _consoleMock = new Mock<IConsole>();

            _employeeTreeParser = new EmployeeTreeParser(_consoleMock.Object);
        }

        [Test]
        public void Print_WhenThereAreTwoCEOs_ThrowsException()
        {
            Action act = () => _employeeTreeParser.Print(new List<Employee>
            {
                new Employee(1, "Sachin Kainth", "Software Developer", "sachin@acme.com", null),
                new Employee(2, "Sachin Kainth", "Software Developer", "sachin@acme.com", null)
            });

            act.Should().Throw<MultipleCEOsException>().WithMessage(
                "This company has more than 1 CEO.  Contact the software development team if you need to support this scenario.");
        }

        [Test]
        public void Print_WhenThereIsNoCEO_ThrowsException()
        {
            Action act = () => _employeeTreeParser.Print(new List<Employee>());

            act.Should().Throw<NoCEOException>().WithMessage("This organisation does not have a CEO");
        }

        [Test]
        public void Print_WhenComplexHierarchy_PrintsHierarchy()
        {
            using (Sequence.Create())
            {
                _consoleMock.Setup(_ => _.WriteLine("6 Sarah Kelly")).InSequence();
                _consoleMock.Setup(_ => _.WriteLine("--3 Tony King")).InSequence();
                _consoleMock.Setup(_ => _.WriteLine("----5 Paul Reid")).InSequence();
                _consoleMock.Setup(_ => _.WriteLine("--4 Steve Hunt")).InSequence();
                _consoleMock.Setup(_ => _.WriteLine("----1 Amy Reid")).InSequence();
                _consoleMock.Setup(_ => _.WriteLine("----2 Amy Reid")).InSequence();

                _employeeTreeParser.Print(new List<Employee>
                {
                    new Employee(1, "Amy Reid", "Admin Assistant", "amy@acme.com", 4),
                    new Employee(2, "Amy Reid", "Admin Assistant", "amy@acme.com", 4),
                    new Employee(3, "Tony King", "CTO", "tony@acme.com", 6),
                    new Employee(4, "Steve Hunt", "CFO", "steve@acme.comm", 6),
                    new Employee(5, "Paul Reid", "Software Developer", "paul@acme.com", 3),
                    new Employee(6, "Sarah Kelly", "CEO", "sarah@acme.com", null)
                });
            }
        }
    }
}
