using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Services.Models.Transactions
{
	public class DashboardTransactionsResponse
	{
		public IEnumerable<TransactionsByDate> CurrentMonthTransactions { get; set; }

		public IEnumerable<LastYearTransactions> LastYearTransactions { get; set; }

		public IEnumerable<TransactionsByStore> TransactionsByStore { get; set; }
	}

	public class TransactionsByDate
	{
		public int Day { get; set; }
		public decimal TotalSum { get; set; }
	}

	public class LastYearTransactions
	{
		public string Month { get; set; }

		public decimal TotalSum { get; set; }

	}

	public class TransactionsByStore
	{
		public string Store { get; set; }

		public int TotalTransactins { get; set; }
	}

}
