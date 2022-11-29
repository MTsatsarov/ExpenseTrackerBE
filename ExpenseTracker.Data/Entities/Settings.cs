using ExpenseTracker.Data.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Data.Entities
{
	public class Settings:BaseDeletableEntity<Guid>
	{
		public Settings()
		{
			this.Id = Guid.NewGuid();
		}
		public string Mode { get; set; }

		[ForeignKey("User")]
		public string UserId { get; set; }

		public virtual ApplicationUser User { get; set; }
	}
}
