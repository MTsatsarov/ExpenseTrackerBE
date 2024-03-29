﻿using ExpenseTracker.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Data.Entities
{
	public class Product : BaseDeletableEntity<Guid>
	{
		public Product()
		{
			this.Id = Guid.NewGuid();
			this.Stores= new HashSet<Store>();
			this.ExpenseProducts = new HashSet<ExpenseProducts>();
		}

		[Required]
		public string Name { get; set; }

		[InverseProperty("Products")]
		public virtual ICollection<Store> Stores { get; set; }

		public virtual ICollection<ExpenseProducts> ExpenseProducts { get; set; }
	}
}
