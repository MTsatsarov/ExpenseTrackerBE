﻿namespace ExpenseTracker.Services.Models.Transactions
{
	public class TransactionResponse
	{
		public Guid Id { get; set; }

		public decimal TotalPrice { get; set; }

		public string Store { get; set; }

		public DateTime CreatedOn { get; set; }
	}
}