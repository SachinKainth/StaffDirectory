namespace StaffDirectory
{
	public class Console : IConsole
	{
		public void WriteLine(string line)
		{
			System.Console.WriteLine(line);
		}

	    public string ReadLine()
	    {
	        return System.Console.ReadLine();
	    }
	}
}
