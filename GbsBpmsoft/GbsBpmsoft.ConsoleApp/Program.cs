namespace GbsBpmsoft.ConsoleApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var substring = Console.ReadLine();

			var accountService = new AccountService();
			Console.WriteLine(accountService.GetCountAccountFromService(substring));
			Console.WriteLine(accountService.GetCountAccountFromOData(substring));

			Console.ReadKey();
		}
	}
}
