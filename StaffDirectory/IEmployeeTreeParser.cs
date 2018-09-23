using System.Collections.Generic;

namespace StaffDirectory
{
    public interface IEmployeeTreeParser
    {
        void Print(IList<Employee> employees);
    }
}