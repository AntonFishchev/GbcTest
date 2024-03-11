namespace GbsBpmsoft.ConsoleApp
{
	/// <summary>
	/// Константы.
	/// </summary>
	public static class Constants
	{
		/// <summary>
		/// Основная ссылка.
		/// </summary>
		public static string MainUrl = "http://localhost:82";

		/// <summary>
		/// Ссылка авторизации.
		/// </summary>
		public static string LoginUrl = "http://localhost:82/ServiceModel/AuthService.svc/Login";

		/// <summary>
		/// Ссылка на метод GetCountAccountWithSubstring сервиса UsrAccountService.
		/// </summary>
		public static string AccountServiceGetCountUrl = "http://localhost:82/0/rest/UsrAccountService/GetCountAccountWithSubstring?substring=";

		/// <summary>
		/// Ссылка на получение количества Контрагентов с заданной подстрокой.
		/// </summary>
		public static string AccountODataSubstringUrl = "http://localhost:82/0/odata/Account?$count=true&$filter=contains(Name,'{0}')";
	}
}
