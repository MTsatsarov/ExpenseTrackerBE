using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExpenseTracker.Services.Models.Storage
{
	public class StorePatchModel:BaseStorageModel
	{
		[JsonPropertyName("id")]
		public Guid StorageId { get; set; }


	}
}
