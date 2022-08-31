namespace ExpenseTracker.Data.Seeding
{
	public interface ISeeder
	{
		Task SeedAsync(ExpenseTrackerDbContext dbContext, IServiceProvider serviceProvider);
	}
}
