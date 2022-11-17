using ExpenseTracker.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Data.Entities
{
	public class ExpenseServices : BaseDeletableEntity<Guid>
	{
		public ExpenseServices()
		{
			this.Id = Guid.NewGuid();
		}

		public Guid ExpenseId { get; set; }

		public virtual Expense Expense { get; set; }

		public Guid ServiceId { get; set; }

		public decimal Price { get; set; }

		public virtual Service Service { get; set; }
	}
}
