using System.Collections.Generic;

namespace StaffDirectory
{
	public class StaffToPrintRepository : BaseRepository, IStaffToPrintRepository
    {
        public StaffToPrintRepository(SynchronizedCollection<Employee> repository) : base(repository)
        {
        }
    }
}
