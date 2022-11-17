using ExpenseTracker.Common;
using ExpenseTracker.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Data.Seeding.Seeders
{
	public class RoleSeeder:ISeeder
	{
        public async Task SeedAsync(ExpenseTrackerDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            await SeedRoleAsync(roleManager, RoleConstants.Client);
            await SeedRoleAsync(roleManager, RoleConstants.Admin);
            await SeedRoleAsync(roleManager, RoleConstants.Owner);

		}
        private static async Task SeedRoleAsync(RoleManager<ApplicationRole> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                var result = await roleManager.CreateAsync(new ApplicationRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
