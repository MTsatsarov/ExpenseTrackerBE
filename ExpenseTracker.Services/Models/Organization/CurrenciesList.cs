using System.Text.Json.Serialization;

namespace ExpenseTracker.Services.Models.Organization
{
	public class CurrenciesList
	{
		[JsonPropertyName("currency")]
		public string Currency { get; set; }

		[JsonPropertyName("abbreviation")]
		public string Abbreviation { get; set; }

		[JsonPropertyName("symbol")]
		public string Symbol { get; set; }
	}
}
