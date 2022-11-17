
using ExpenseTracker.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Data.Entities
{
	public class Service:BaseDeletableEntity<Guid>
	{
		public Service()
		{
			this.Id = Guid.NewGuid();
		}

		[Required]
		public string Name { get; set; }

		public virtual ICollection<ExpenseServices> ExpenseServices { get; set; }
	}
}
