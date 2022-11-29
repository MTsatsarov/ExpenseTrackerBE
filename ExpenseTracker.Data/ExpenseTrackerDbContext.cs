using ExpenseTracker.Data.Common;
using ExpenseTracker.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ExpenseTracker.Data
{
	public class ExpenseTrackerDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
	{
		private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
		   typeof(ExpenseTrackerDbContext).GetMethod(
			   nameof(SetIsDeletedQueryFilter),
			   BindingFlags.NonPublic | BindingFlags.Static);
		public ExpenseTrackerDbContext(DbContextOptions<ExpenseTrackerDbContext> options)
		 : base(options)
		{
		}

		public DbSet<Expense> Expenses { get; set; }

		public DbSet<Product> Products { get; set; }

		public DbSet<Store> Stores { get; set; }

		public DbSet<ExpenseProducts> ExpenseProducts { get; set; }

		public DbSet<Service> Services { get; set; }

		public DbSet<ExpenseServices> ExpenseServices { get; set; }

		public DbSet<Organization> Organizations { get; set; }

		public DbSet<Storage> Storages { get; set; }

		public DbSet<Settings> Settings { get; set; }

		public override int SaveChanges() => this.SaveChanges(true);

		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			this.ApplyAuditInfoRules();
			return base.SaveChanges(acceptAllChangesOnSuccess);
		}
		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
		  this.SaveChangesAsync(true, cancellationToken);

		public override Task<int> SaveChangesAsync(
		 bool acceptAllChangesOnSuccess,
		 CancellationToken cancellationToken = default)
		{
			this.ApplyAuditInfoRules();
			return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseLazyLoadingProxies();
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<Expense>()
				.HasOne(a => a.ExpenseService)
				.WithOne(b => b.Expense)
				.HasForeignKey<ExpenseServices>(b => b.ExpenseId);

			// Needed for Identity models configuration
			base.OnModelCreating(builder);

			this.ConfigureUserIdentityRelations(builder);

			EntityIndexesConfiguration.Configure(builder);

			var entityTypes = builder.Model.GetEntityTypes().ToList();

			// Set global query filter for not deleted entities only
			var deletableEntityTypes = entityTypes
				.Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
			foreach (var deletableEntityType in deletableEntityTypes)
			{
				var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
				method.Invoke(null, new object[] { builder });
			}

			// Disable cascade delete
			var foreignKeys = entityTypes
				.SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
			foreach (var foreignKey in foreignKeys)
			{
				foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
			}
		}

		private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
		   where T : class, IDeletableEntity
		{
			builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
		}

		private void ConfigureUserIdentityRelations(ModelBuilder builder)
		   => builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

		private void ApplyAuditInfoRules()
		{
			var changedEntries = this.ChangeTracker
				.Entries()
				.Where(e =>
					e.Entity is IAuditInfo &&
					(e.State == EntityState.Added || e.State == EntityState.Modified));

			foreach (var entry in changedEntries)
			{
				var entity = (IAuditInfo)entry.Entity;
				if (entry.State == EntityState.Added && entity.CreatedOn == default)
				{
					entity.CreatedOn = DateTime.UtcNow;
				}
				else
				{
					entity.ModifiedOn = DateTime.UtcNow;
				}
			}
		}
	}
}
