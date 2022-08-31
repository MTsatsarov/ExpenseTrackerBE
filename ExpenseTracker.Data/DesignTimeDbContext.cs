using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ExpenseTracker.Data
{
	public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ExpenseTrackerDbContext>
	{
		public ExpenseTrackerDbContext CreateDbContext(string[] args)
		{
			var configuration = new ConfigurationBuilder()
				 .SetBasePath(Directory.GetCurrentDirectory())
				 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				 .Build();

			var builder = new DbContextOptionsBuilder<ExpenseTrackerDbContext>();
			var connectionString = configuration.GetConnectionString("DefaultConnection");
			builder.UseSqlServer(connectionString);

			return new ExpenseTrackerDbContext(builder.Options);
		}
	}
}
