using System.Collections.Generic;
using System.Linq;

namespace StaffDirectory
{
    public class EmployeeTreeParser : IEmployeeTreeParser
    {
        private readonly IConsole _console;

        public EmployeeTreeParser(IConsole console)
        {
            _console = console;
        }

        public void Print(IList<Employee> employees)
        {
            if (employees.Count(_ => _.ManagerId == null) == 0)
            {
                throw new NoCEOException("This organisation does not have a CEO");
            }
            if (employees.Count(_ => _.ManagerId == null) > 1)
            {
                throw new MultipleCEOsException(
                    "This company has more than 1 CEO.  Contact the software development team if you need to support this scenario.");
            }

            var ceo = employees.SingleOrDefault(_ => _.ManagerId == null);
            var topOfTree = new EmployeeTree(ceo.Id, ceo.Name);

            CreateOrgChart(employees, topOfTree);
            Print(topOfTree, 0);
        }

        private void Print(EmployeeTree tree, int level)
        {
            var hyphens = string.Join("", Enumerable.Repeat("-", 2*level));
            _console.WriteLine($"{hyphens}{tree.Id} {tree.Name}");
   
            foreach (var employee in tree.DirectReports)
            {
                Print(employee, level+1);
            }
        }

        private static void CreateOrgChart(IList<Employee> employees, EmployeeTree topOfTree)
        {
            foreach (var employee in employees)
            {
                if (employee.ManagerId == topOfTree.Id)
                {
                    topOfTree.AddDirectReport(employee.Id, employee.Name);
                    CreateOrgChart(employees, topOfTree.DirectReports.Last());
                }
            }
        }
    }
}
