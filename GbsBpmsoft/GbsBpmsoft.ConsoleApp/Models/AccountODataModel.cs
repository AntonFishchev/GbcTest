using Newtonsoft.Json;

namespace GbsBpmsoft.ConsoleApp.Models
{
	/// <summary>
	/// Модель получения данных о Контрагентах через OData.
	/// </summary>
	public class AccountODataModel
	{
		/// <summary>
		/// Количество контрагентов.
		/// </summary>
		[JsonProperty("@odata.count")]
		public int Count { get; set; }
	}
}
