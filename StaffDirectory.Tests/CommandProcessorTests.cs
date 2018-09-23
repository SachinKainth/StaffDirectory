using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace StaffDirectory.Tests
{
    [TestFixture]
    class CommandProcessorTests
    {
        private Mock<IEnvironment> _environmentMock;
        private Mock<IConsole> _consoleMock;
        private Mock<IAllStaffRepository> _allStaffRepositoryMock;
        private Mock<IStaffToPrintRepository> _staffToPrintRepositoryMock;
        private Mock<IEmployeeTreeParser> _employeeTreeParserMock;

        private CommandProcessor _commandProcessor;

        [SetUp]
        public void Setup()
        {
            _environmentMock = new Mock<IEnvironment>();
            _consoleMock = new Mock<IConsole>();
            _allStaffRepositoryMock = new Mock<IAllStaffRepository>();
            _staffToPrintRepositoryMock = new Mock<IStaffToPrintRepository>();
            _employeeTreeParserMock = new Mock<IEmployeeTreeParser>();

            _commandProcessor = new CommandProcessor(
                _allStaffRepositoryMock.Object, _environmentMock.Object, _consoleMock.Object,
                _staffToPrintRepositoryMock.Object, _employeeTreeParserMock.Object);
        }

        [Test]
        public void Process_WhenCommandIsExit_ExitsSystem()
        {
            _commandProcessor.Process("exit");

            _environmentMock.Verify(_ => _.Exit(0));
        }

        [Test]
        public void Process_WhenCommandIsDuplicatesAndThereAreNoDuplicates_DoesNotFindAnything()
        {
            _allStaffRepositoryMock.Setup(_ => _.Get()).Returns(new List<Employee>
            {
                new Employee(1, "Amy Reid", "Admin Assistant", "amy@acme.com", 4)
            });

            _commandProcessor.Process("duplicates");

            _consoleMock.Verify(_ => _.WriteLine(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Process_WhenCommandIsDuplicatesAndThereIsOneDuplicate_DoesFindThatDuplicate()
        {
            _allStaffRepositoryMock.Setup(_ => _.Get()).Returns(new List<Employee>
            {
                new Employee(1, "Amy Reid", "Admin Assistant", "amy@acme.com", 4),
                new Employee(2, "Amy Reid", "Admin Assistant", "amy@acme.com", 4)
            });

            _commandProcessor.Process("duplicates");

            _consoleMock.Verify(_ => _.WriteLine(It.Is<string>(s => s == "Found duplicate for Amy Reid")), Times.Once);
        }

        [Test]
        public void Process_WhenCommandIsDuplicatesAndThereAreManyDuplicates_DoesFindAllOfTheDuplicates()
        {
            _allStaffRepositoryMock.Setup(_ => _.Get()).Returns(new List<Employee>
            {
                new Employee(1, "Amy Reid", "Admin Assistant", "amy@acme.com", 4),
                new Employee(2, "Amy Reid", "Admin Assistant", "amy@acme.com", 4),
                new Employee(3, "Amy Reid", "Admin Assistant", "amy@acme.com", 4),
                new Employee(4, "Amy Reid", "Admin Assistant", "amy@acme.com", 4),
                new Employee(5, "Amy Reid", "Admin Assistant", "amy@acme.com", 4)
            });

            _commandProcessor.Process("duplicates");

            _consoleMock.Verify(_ => _.WriteLine(It.Is<string>(s => s == "Found duplicate for Amy Reid")),
                Times.Exactly(4));
        }

        [Test]
        public void Process_WhenCommandIsPrintAndThereAreNoEmployeesFound_DoesNotPrintAnyEmployeeDetails()
        {
            _commandProcessor.Process("print");

            _consoleMock.Verify(_ => _.WriteLine(It.Is<string>(s => s == "--- START OF STAFF LIST ---")), Times.Once);
            _consoleMock.Verify(_ => _.WriteLine(It.Is<string>(s => s == "--- END OF STAFF LIST ---")), Times.Once);
            _consoleMock.Verify(_ => _.WriteLine(It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        public void Process_WhenCommandIsPrintAndThereAreEmployeesFound_DoesPrintAllEmployeeDetails()
        {
            var employee1 = new Employee(1, "Sachin Kainth", "Software Developer", "sachin@acme.com", 4);
            var employee2 = new Employee(2, "Sachin Kainth", "Software Developer", "sachin@acme.com", 4);
            _staffToPrintRepositoryMock.Setup(_ => _.Get()).Returns(new List<Employee>
            {
                employee1,
                employee2
            });

            _commandProcessor.Process("print");

            _consoleMock.Verify(_ => _.WriteLine(It.Is<string>(s => s == "--- START OF STAFF LIST ---")), Times.Once);
            _consoleMock.Verify(_ => _.WriteLine(It.Is<string>(s => s == "--- END OF STAFF LIST ---")), Times.Once);
            _consoleMock.Verify(_ => _.WriteLine(It.Is<string>(s => s == employee1.ToString())), Times.Exactly(1));
            _consoleMock.Verify(_ => _.WriteLine(It.Is<string>(s => s == employee2.ToString())), Times.Exactly(1));
        }

        [TestCase("1.5")]
        [TestCase("a")]
        public void Process_WhenCommandIsAddAndTheManagerIdIsNotAnInteger_ThrowsException(string invalidManagerId)
        {
            _consoleMock.SetupSequence(_ => _.ReadLine())
                .Returns("")
                .Returns("")
                .Returns(invalidManagerId);

            Action act = () => _commandProcessor.Process("add");

            act.Should().Throw<ArgumentException>().WithMessage("The manager id must be a whole number");
        }

        [Test]
        public void Process_WhenCommandIsAddAndTheManagerIdDoesNotExist_ThrowsException()
        {
            const string nonExistantEmployeeId = "2";

            _allStaffRepositoryMock.Setup(_ => _.IsValidManagerId(
                It.Is<int>(id => id == int.Parse(nonExistantEmployeeId)))).Returns(false);

            _consoleMock.SetupSequence(_ => _.ReadLine())
                .Returns("")
                .Returns("")
                .Returns(nonExistantEmployeeId);

            Action act = () => _commandProcessor.Process("add");

            act.Should().Throw<ArgumentException>().WithMessage("The manager id must be an id of a valid staff member");
        }

        [Test]
        public void Process_WhenCommandIsAddAndHappyPath_AddsEmployeeToRepository()
        {
            _allStaffRepositoryMock.Setup(_ => _.IsValidManagerId(It.IsAny<int>())).Returns(true);
            _allStaffRepositoryMock.Setup(_ => _.GetNextId()).Returns(2);

            _consoleMock.SetupSequence(_ => _.ReadLine())
                .Returns("Sachin Kainth")
                .Returns("Software Developer")
                .Returns("1");

            _commandProcessor.Process("add");

            var employee = new Employee(2, "Sachin Kainth", "Software Developer", "sachin@acme.com", 1);
            _allStaffRepositoryMock.Verify(_ => _.Add(It.Is<Employee>(e => e.Equals(employee))));

            _consoleMock.Verify(_ => _.WriteLine(It.Is<string>(s => s == "Enter full name")));
            _consoleMock.Verify(_ => _.WriteLine(It.Is<string>(s => s == "Enter job title")));
            _consoleMock.Verify(_ => _.WriteLine(It.Is<string>(s => s == "Enter manager id")));
            _consoleMock.Verify(_ => _.WriteLine(It.Is<string>(s => s == employee.ToString())));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void Process_WhenCommandIsAddAndManagerIdIsEmpty_AddsEmployeeToRepositoryAtTopOfHierarchy(string managerId)
        {
            _allStaffRepositoryMock.Setup(_ => _.GetNextId()).Returns(2);

            _consoleMock.SetupSequence(_ => _.ReadLine())
                .Returns("Sachin Kainth")
                .Returns("Software Developer")
                .Returns(managerId);

            _commandProcessor.Process("add");

            var employee = new Employee(2, "Sachin Kainth", "Software Developer", "sachin@acme.com", null);
            _allStaffRepositoryMock.Verify(_ => _.Add(It.Is<Employee>(e => e.Equals(employee))));

            _consoleMock.Verify(_ => _.WriteLine(It.Is<string>(s => s == "Enter full name")));
            _consoleMock.Verify(_ => _.WriteLine(It.Is<string>(s => s == "Enter job title")));
            _consoleMock.Verify(_ => _.WriteLine(It.Is<string>(s => s == "Enter manager id")));
            _consoleMock.Verify(_ => _.WriteLine(It.Is<string>(s => s == employee.ToString())));
        }

        [Test]
        public void Process_WhenDefaultCommandAndNoEmployeesFound_DoesNothing()
        {
            _commandProcessor.Process("Sachin Kainth");

            _consoleMock.Verify(_ => _.WriteLine(It.IsAny<string>()), Times.Never);
            _consoleMock.Verify(_ => _.ReadLine(), Times.Never);
            _allStaffRepositoryMock.Verify(_ => _.Add(It.IsAny<Employee>()), Times.Never);
        }

        [TestCase("  yes  ")]
        [TestCase("  YES  ")]
        [TestCase("yes")]
        [TestCase("YES")]
        [TestCase("yEs")]
        public void Process_WhenDefaultCommandAndEmployeeFoundAndUserWantsToPrintIt_AddsEmployeeToListToBePrinted(string yes)
        {
            _allStaffRepositoryMock.Setup(_ => _.Get()).Returns(new List<Employee>
            {
                new Employee(2, "Sachin Kainth", "Software Developer", "sachin@acme.com", 1)
            });

            _consoleMock.Setup(_ => _.ReadLine()).Returns(yes);

            _commandProcessor.Process("Sachin Kainth");

            _consoleMock.Verify(_ => _.WriteLine(It.Is<string>(s => s == "Found Staff Member, add to list?")), Times.Once);
            _consoleMock.Verify(_ => _.WriteLine(It.Is<string>(s => s == "Staff member added to list")), Times.Once);
            _staffToPrintRepositoryMock.Verify(_ => _.Add(It.IsAny<Employee>()), Times.Once);
        }

        [TestCase("  no  ")]
        [TestCase("  NO  ")]
        [TestCase("no")]
        [TestCase("NO")]
        [TestCase("nO")]
        public void Process_WhenDefaultCommandAndEmployeeFoundAndUserDoesntWantToPrintIt_DoesntAddsEmployeeToListToBePrinted(string no)
        {
            _allStaffRepositoryMock.Setup(_ => _.Get()).Returns(new List<Employee>
            {
                new Employee(2, "Sachin Kainth", "Software Developer", "sachin@acme.com", 1)
            });

            _consoleMock.Setup(_ => _.ReadLine()).Returns(no);

            _commandProcessor.Process("Sachin Kainth");

            _consoleMock.Verify(_ => _.WriteLine(It.Is<string>(s => s == "Found Staff Member, add to list?")), Times.Once);
            _consoleMock.Verify(_ => _.WriteLine(It.Is<string>(s => s == "Staff member not added to list")), Times.Once);
            _staffToPrintRepositoryMock.Verify(_ => _.Add(It.IsAny<Employee>()), Times.Never);
        }

        [Test]
        public void Process_WhenDefaultCommandAndEmployeeFoundAndUserDoesntSayYesOrNo_DoesntAddsEmployeeToListToBePrinted()
        {
            _allStaffRepositoryMock.Setup(_ => _.Get()).Returns(new List<Employee>
            {
                new Employee(2, "Sachin Kainth", "Software Developer", "sachin@acme.com", 1)
            });

            _consoleMock.Setup(_ => _.ReadLine()).Returns("y");

            _commandProcessor.Process("Sachin Kainth");

            _consoleMock.Verify(_ => _.WriteLine(It.Is<string>(s => s == "Found Staff Member, add to list?")), Times.Once);
            _consoleMock.Verify(_ => _.WriteLine(It.Is<string>(s => s == "Staff member added to list")), Times.Never);
            _consoleMock.Verify(_ => _.WriteLine(It.Is<string>(s => s == "Staff member not added to list")), Times.Never);
            _staffToPrintRepositoryMock.Verify(_ => _.Add(It.IsAny<Employee>()), Times.Never);
        }

        [Test]
        public void Process_WhenDefaultCommandAndMultipleEmployeesFoundAndUserWantsToPrintThem_AddsAllOfThemToListToBePrinted()
        {
            _allStaffRepositoryMock.Setup(_ => _.Get()).Returns(new List<Employee>
            {
                new Employee(2, "Sachin Kainth", "Software Developer", "sachin@acme.com", 1),
                new Employee(3, "Sachin Kainth", "Software Developer", "sachin@acme.com", 1)
            });

            _consoleMock.Setup(_ => _.ReadLine()).Returns("yes");

            _commandProcessor.Process("Sachin Kainth");

            _consoleMock.Verify(_ => _.WriteLine(It.Is<string>(s => s == "Found Staff Member, add to list?")), Times.Exactly(2));
            _consoleMock.Verify(_ => _.WriteLine(It.Is<string>(s => s == "Staff member added to list")), Times.Exactly(2));
            _staffToPrintRepositoryMock.Verify(_ => _.Add(It.IsAny<Employee>()), Times.Exactly(2));
        }

        [Test]
        public void Process_WhenCommandIsOrgChart_PrintsOrgChart()
        {
            var employees = new List<Employee>
            {
                new Employee(1, "Sachin Kainth", "Software Developer", "sachin@acme.com", null),
                new Employee(2, "Amy Reid", "Admin Assistant", "amy@acme.com", 1)
            };
            _allStaffRepositoryMock.Setup(_ => _.Get()).Returns(employees);

            _commandProcessor.Process("orgchart");

            _employeeTreeParserMock.Verify(_ => _.Print(It.Is<IList<Employee>>(l => l.Count == 2)));
        }
    }
}
