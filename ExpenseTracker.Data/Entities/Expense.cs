﻿using ExpenseTracker.Data.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Data.Entities
{
	public class Expense : BaseDeletableEntity<Guid>
	{
		public Expense()
		{
			this.Id = Guid.NewGuid();
		}

		public virtual ICollection<ExpenseProducts> ExpenseProducts { get; set; }

		public virtual ICollection<Store> Stores { get; set; }

		public string UserId { get; set; }

		public virtual ApplicationUser User { get; set; }
	}
}