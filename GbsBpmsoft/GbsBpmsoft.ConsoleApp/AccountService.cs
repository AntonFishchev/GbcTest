using GbsBpmsoft.ConsoleApp.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace GbsBpmsoft.ConsoleApp
{
	/// <summary>
	/// Сервис Контрагентов.
	/// </summary>
	public class AccountService
	{
		/// <summary>
		/// Контейнер с Cookies.
		/// </summary>
		public CookieContainer _cookieContainer = new CookieContainer();

		/// <summary>
		/// Конструктор.
		/// </summary>
		public AccountService()
		{
			_cookieContainer.Add(GetAuthCookies());
		}

		/// <summary>
		/// Получает cookies авторизации.
		/// </summary>
		/// <returns>Контейнер с cookies.</returns>
		private CookieCollection GetAuthCookies()
		{
			var requestLogin = (HttpWebRequest)WebRequest.Create(Constants.LoginUrl);
			requestLogin.Method = "POST";
			requestLogin.ContentType = "application/json";

			using (var sw = new StreamWriter(requestLogin.GetRequestStream()))
			{
				var json =
					"{" +
						$"\"UserName\": \"Supervisor\"," +
						$"\"UserPassword\": \"Supervisor\"," +
					"}";

				sw.Write(json);
			}

			var response = (HttpWebResponse)requestLogin.GetResponse();

			var cookies = new CookieCollection();
			var regex = new Regex("(?<=.ASPXAUTH=)(.+?)(?=;)");
			var token = regex.Match(response.Headers[HttpResponseHeader.SetCookie]).ToString();
			cookies.Add(new Cookie(".ASPXAUTH", token, "/", "localhost"));

			return cookies;
		}

		/// <summary>
		/// Выполняет запрос.
		/// </summary>
		/// <param name="method">HTTP-метод запроса.</param>
		/// <param name="url">Ссылка запроса.</param>
		/// <returns>Стрим тела ответа.</returns>
		private StreamReader ExecuteRequest(string method, string url)
		{
			if (string.IsNullOrEmpty(method))
			{
				throw new ArgumentNullException("method");
			}

			if (string.IsNullOrEmpty(url))
			{
				throw new ArgumentNullException("url");
			}

			var request = (HttpWebRequest)WebRequest.Create(url);
			request.CookieContainer = _cookieContainer;
			request.Method = method;

			var response = (HttpWebResponse)request.GetResponse();
			return new StreamReader(response.GetResponseStream(), Encoding.ASCII);
		}

		/// <summary>
		/// Получает количество Контрагентов с заданной подстрокой в названии, через сервис.
		/// </summary>
		/// <param name="substring">Подстрока.</param>
		/// <returns>Количество Контрагентов.</returns>
		public int GetCountAccountFromService(string substring)
		{
			if (string.IsNullOrEmpty(substring))
			{
				throw new ArgumentNullException("substring");
			}

			using (var reader = ExecuteRequest("GET", Constants.AccountServiceGetCountUrl + substring))
			{
				var result = JsonConvert
					.DeserializeObject<GetCountAccountWithSubstringModel>(reader.ReadToEnd());
				return result.Result;
			}
		}

		/// <summary>
		/// Получает количество Контрагентов с заданной подстрокой в названии, через OData.
		/// </summary>
		/// <param name="substring">Подстрока.</param>
		/// <returns>Количество Контрагентов.</returns>
		public int GetCountAccountFromOData(string substring)
		{
			if (string.IsNullOrEmpty(substring))
			{
				throw new ArgumentNullException("substring");
			}

			using (var reader = ExecuteRequest("GET", string.Format(Constants.AccountODataSubstringUrl, substring)))
			{
				var result = JsonConvert
					.DeserializeObject<AccountODataModel>(reader.ReadToEnd());
				return result.Count;
			}
		}
	}
}
