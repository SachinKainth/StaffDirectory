using System.Collections.Generic;
using System.Linq;

namespace StaffDirectory
{
	public class AllStaffRepository : BaseRepository, IAllStaffRepository
	{
	    public AllStaffRepository(SynchronizedCollection<Employee> repository) : base(repository)
	    {
	    }
        
	    public bool IsValidManagerId(int managerId)
	    {
	        return Repository.Count(_ => _.Id == managerId) == 1;
	    }

	    public int GetNextId()
	    {
	        return Repository.Any() ? Repository.Max(_ => _.Id) + 1 : 1;
	    }
	}
}
