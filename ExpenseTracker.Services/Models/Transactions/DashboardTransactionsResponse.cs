using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Services.Models.Transactions
{
	public class DashboardTransactionsResponse
	{
		public DashboardTransactionsResponse()
		{
			TransactionsByStore = new List<TransactionByStore>();
			CurrentMonthTransactions= new List<TransactionsByDate>();
			LastYearTransactions = MapProperties().ToList();
		}
		public List<TransactionsByDate> CurrentMonthTransactions { get; set; }

		public ICollection<LastYearTransactions> LastYearTransactions { get; set; }

		public ICollection<TransactionByStore> TransactionsByStore { get; set; }

		private List<LastYearTransactions> MapProperties()
		{
			var list =new List<LastYearTransactions>()
			{
				new LastYearTransactions()
				{
					Month = "January"
				},
				new LastYearTransactions()
				{
					Month = "February"
				},
				new LastYearTransactions()
				{
					Month = "March"
				},
				new LastYearTransactions()
				{
					Month = "April"
				},
				new LastYearTransactions()
				{
					Month = "May"
				},
				new LastYearTransactions()
				{
					Month = "June"
				},
				new LastYearTransactions()
				{
					Month = "July"
				},
				new LastYearTransactions()
				{
					Month = "August"
				},
				new LastYearTransactions()
				{
					Month = "September"
				},
				new LastYearTransactions()
				{
					Month = "October"
				},
				new LastYearTransactions()
				{
					Month = "November"
				},
				new LastYearTransactions()
				{
					Month = "December"
				},
			};
			return list;
		}
	}

	public class TransactionsByDate
	{
		public string Name { get; set; }
		public decimal Sum { get; set; }
	}

	public class LastYearTransactions
	{
		public string Month { get; set; }

		public decimal Sum { get; set; }

	}

	public class TransactionByStore
	{
		public string Name { get; set; }

		public int Count { get; set; }
	}
}
