using System;
using System.Collections.Generic;

namespace StaffDirectory
{
    public class Program
    {
	    private readonly ICommandProcessor _commandProcessor;

		public Program(ICommandProcessor commandProcessor)
		{
			_commandProcessor = commandProcessor;
		}

		static void Main()
        {
            var repository = new SynchronizedCollection<Employee>
            {
                new Employee(1, "Amy Reid", "Admin Assistant", "amy@acme.com", 4),
                new Employee(2, "Amy Reid", "Admin Assistant", "amy@acme.com", 4),
                new Employee(3, "Tony King", "CTO", "tony@acme.com", 6),
                new Employee(4, "Steve Hunt", "CFO", "steve@acme.comm", 6),
                new Employee(5, "Paul Reid", "Software Developer", "paul@acme.com", 3),
                new Employee(6, "Sarah Kelly", "CEO", "sarah@acme.com", null)
            };

            new Program(
                new CommandProcessor(
                        new AllStaffRepository(repository), 
                        new Environment(), 
                        new Console(), 
                        new StaffToPrintRepository(new SynchronizedCollection<Employee>()),
                        new EmployeeTreeParser(new Console()))).RunDirectory();
        }

		private void RunDirectory()
		{
		    System.Console.WriteLine("Staff Directory");
            while (true)
			{
				System.Console.WriteLine("Enter command:");
				var command = System.Console.ReadLine();

			    try
			    {
			        _commandProcessor.Process(command);
			    }
			    catch (Exception e)
			    {
				    System.Console.WriteLine(e.Message);
                }
            }
		}
    }
}
