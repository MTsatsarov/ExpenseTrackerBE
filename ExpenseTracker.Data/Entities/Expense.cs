using ExpenseTracker.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Data.Entities
{
	public class Expense : BaseDeletableEntity<Guid>
	{
		public Expense()
		{
			this.Id = Guid.NewGuid();
			this.ExpenseProducts = new HashSet<ExpenseProducts>();
			this.Stores = new HashSet<Store>();
		}

		public virtual ICollection<ExpenseProducts> ExpenseProducts { get; set; }

		public virtual ICollection<Store> Stores { get; set; }

		public string UserId { get; set; }

		[Required]
		public string Type { get; set; }

		public virtual ApplicationUser User { get; set; }

		public Guid? ExpenseServiceId { get; set; }

		public virtual ExpenseServices ExpenseService{ get; set; }

		public Guid OrganizationId { get; set; }

		public virtual Organization Organization { get; set; }
	}
}
