using System.Collections.Generic;

namespace StaffDirectory
{
    public class EmployeeTree
    {
        public int? Id { get; }
        public string Name { get; }
        public List<EmployeeTree> DirectReports { get; }

        public EmployeeTree(int? id, string name)
        {
            Id = id;
            Name = name;
            DirectReports = new List<EmployeeTree>();
        }

        public void AddDirectReport(int? id, string name)
        {
            DirectReports.Add(new EmployeeTree(id, name));
        }
    }
}
