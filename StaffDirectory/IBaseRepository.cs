using System.Collections.Generic;

namespace StaffDirectory
{
    public interface IBaseRepository
    {
        void Add(Employee e);
        IEnumerable<Employee> Get();
    }
}