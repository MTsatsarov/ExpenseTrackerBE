using ExpenseTracker.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Data.Entities
{
	public class Store:BaseDeletableEntity<Guid>
	{
		public Store()
		{
			this.Id = Guid.NewGuid();
			this.Products = new HashSet<Product>();
		}

		[Required]
		public string Name { get; set; }

		public string Address { get; set; }

		[InverseProperty("Stores")]
		public virtual ICollection<Product> Products { get; set; }
	}
}