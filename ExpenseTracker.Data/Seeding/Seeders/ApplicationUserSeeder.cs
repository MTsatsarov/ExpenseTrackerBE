using ExpenseTracker.Common;
using ExpenseTracker.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Data.Seeding.Seeders
{
	public class ApplicationUserSeeder : ISeeder
	{
		public async Task SeedAsync(ExpenseTrackerDbContext dbContext, IServiceProvider serviceProvider)
		{
			var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
			var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			var password = "AaBbCc1!";
			var user = new ApplicationUser()
			{
				UserName = "Admin",
				Email = "Admin@admin.bg",
				FirstName = "Admin",
				LastName = "Admin",
				Organization = new Organization()
				{
					Name = "Admin organization",
					Currency = "Bulgaria Lev",
					Abbreviation = "BGN",
					CurrencySymbol = "лв"
				},
				Settings = new Settings()
				{
					Mode = "dark"
				}

			};

			await userManager.CreateAsync(user, password);


			await userManager.AddToRoleAsync(user, RoleConstants.Admin);
		}
	}
}
