
using ExpenseTracker.Data.Seeding.Seeders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Data.Seeding
{
	public class ExpenseTrackerDbSeeder : ISeeder
	{
        public async Task SeedAsync(ExpenseTrackerDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger(typeof(ExpenseTrackerDbSeeder));

            var seeders = new List<ISeeder>
                          {
                              new RoleSeeder(),
                              new ApplicationUserSeeder(),
						  };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext, serviceProvider);
                await dbContext.SaveChangesAsync();
                logger.LogInformation($"Seeder {seeder.GetType().Name} done.");
            }
        }
    }
}
