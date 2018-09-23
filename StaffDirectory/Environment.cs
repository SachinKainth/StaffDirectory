namespace StaffDirectory
{
	public class Environment : IEnvironment
	{
		public void Exit(int exitCode)
		{
			System.Environment.Exit(exitCode);
		}
	}
}
