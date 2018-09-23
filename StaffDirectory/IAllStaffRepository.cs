namespace StaffDirectory
{
	public interface IAllStaffRepository: IBaseRepository
	{
	    bool IsValidManagerId(int managerId);
	    int GetNextId();
	}
}