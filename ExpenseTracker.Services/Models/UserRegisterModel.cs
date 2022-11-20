using ExpenseTracker.Services.Models.Organization;
using ExpenseTracker.Services.Models.User;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExpenseTracker.Services.Models
{
	public class UserRegisterModel : BaseUserRegistrationModel
	{
		[Required]
		[StringLength(50, MinimumLength = 2)]
		public string Organization { get; set; }


		[JsonPropertyName("currency")]
		public CurrenciesList Currency { get; set; }
	}
}
