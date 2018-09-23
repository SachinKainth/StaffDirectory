using System;
using System.Linq;

namespace StaffDirectory
{
	public class CommandProcessor : ICommandProcessor
	{
		private readonly IAllStaffRepository _allStaffRepository;
		private readonly IEnvironment _environment;
		private readonly IConsole _console;
	    private readonly IStaffToPrintRepository _staffToPrintRepository;
	    private readonly IEmployeeTreeParser _employeeTreeParser;

	    public CommandProcessor(
            IAllStaffRepository allStaffRepository, IEnvironment environment, IConsole console, 
            IStaffToPrintRepository staffToPrintRepository, IEmployeeTreeParser employeeTreeParser)
		{
			_allStaffRepository = allStaffRepository;
			_environment = environment;
			_console = console;
		    _staffToPrintRepository = staffToPrintRepository;
		    _employeeTreeParser = employeeTreeParser;
		}

		public void Process(string command)
		{
            // TODO Use command pattern to separate these out
		    switch (command)
		    {
		        case "add":
		            _console.WriteLine("Enter full name");
		            var name = _console.ReadLine();

		            _console.WriteLine("Enter job title");
		            var jobTitle = _console.ReadLine();

		            _console.WriteLine("Enter manager id");
		            var managerIdInput = _console.ReadLine()?.Trim();

		            var managerIdOrNone = GetManagerId(managerIdInput);
                    
		            var employee = new Employee(
		                _allStaffRepository.GetNextId(), name, jobTitle,
		                name.Split(null)[0].ToLower() + "@acme.com", managerIdOrNone);

		            _allStaffRepository.Add(employee);

		            _console.WriteLine(employee.ToString());

		            break;
		        case "print":
		            _console.WriteLine("--- START OF STAFF LIST ---");

		            foreach (var e in _staffToPrintRepository.Get())
		            {
		                _console.WriteLine(e.ToString());
		            }

		            _console.WriteLine("--- END OF STAFF LIST ---");

		            break;
		        case "exit":
		            _environment.Exit(0);

		            break;
		        case "orgchart":
		            var employees = _allStaffRepository.Get().ToList();
		            _employeeTreeParser.Print(employees);

		            break;
		        case "duplicates":
		            var allStaff = _allStaffRepository.Get().ToList();

		            for (var i = 0; i < allStaff.Count - 1; i++)
		            {
		                var staffMember1 = allStaff[i];
		                var staffMember2 = allStaff[i + 1];
		                if (staffMember1.EmailAddress == staffMember2.EmailAddress)
		                {
		                    _console.WriteLine("Found duplicate for " + staffMember1.Name);
		                }
		            }

		            break;
		        default:
		            foreach (var e in _allStaffRepository.Get())
		            {
		                if (e.Name == command)
		                {
		                    _console.WriteLine("Found Staff Member, add to list?");
		                    var addOrNot = _console.ReadLine().Trim().ToLower();
		                    if (addOrNot == "yes")
		                    {
		                        _staffToPrintRepository.Add(e);
		                        _console.WriteLine("Staff member added to list");
		                    }
		                    else if (addOrNot == "no")
		                    {
		                        _console.WriteLine("Staff member not added to list");
		                    }
		                }
		            }

                    break;
		    }
		}

        private int? GetManagerId(string managerIdInput)
	    {
	        int? managerIdOrNone;
	        if (!string.IsNullOrEmpty(managerIdInput))
	        {
	            if (!int.TryParse(managerIdInput, out var managerId))
	            {
	                throw new ArgumentException("The manager id must be a whole number");
	            }

	            managerIdOrNone = managerId;

	            if (!_allStaffRepository.IsValidManagerId(managerId))
	            {
	                throw new ArgumentException("The manager id must be an id of a valid staff member");
	            }
	        }
	        else
	        {
	            managerIdOrNone = null;
	        }

	        return managerIdOrNone;
	    }
	}
}
