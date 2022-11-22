using ExpenseTracker.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Data.Entities
{
	public class Storage:BaseDeletableEntity<Guid>
	{
		public Storage()
		{
			this.Id = Guid.NewGuid();
		}

		[Required]
		public string Product { get; set; }

		[Required]
		public decimal Quantity { get; set; }

		[Required]
		public string UpdatedBy { get; set; }

		public Guid OrganizationId{ get; set; }

		public virtual Organization Organization { get; set; }
	}
}
