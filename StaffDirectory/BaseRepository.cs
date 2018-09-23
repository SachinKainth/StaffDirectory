using System.Collections.Generic;

namespace StaffDirectory
{
    public abstract class BaseRepository : IBaseRepository
    {
        protected SynchronizedCollection<Employee> Repository { get; set; }

        protected BaseRepository(SynchronizedCollection<Employee> repository)
        {
            Repository = repository;
        }

        public IEnumerable<Employee> Get()
        {
            return Repository;
        }

        public void Add(Employee e)
        {
            Repository.Add(e);
        }
    }
}
