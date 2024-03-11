using Newtonsoft.Json;

namespace GbsBpmsoft.ConsoleApp.Models
{
	/// <summary>
	/// Модель результата выполнения GetCountAccountWithSubstring. 
	/// </summary>
	public class GetCountAccountWithSubstringModel
	{
		/// <summary>
		/// Результат выполнения.
		/// </summary>
		[JsonProperty("GetCountAccountWithSubstringResult")]
		public int Result { get; set; }
	}
}
