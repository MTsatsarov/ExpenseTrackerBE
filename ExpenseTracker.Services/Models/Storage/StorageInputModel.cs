using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.Storage
{
	public class StorageInputModel:BaseStorageModel
	{
		[Required]
		public string Email { get; set; }
	}
}
