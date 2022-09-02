using ExpenseTracker.Data;
using ExpenseTracker.Data.Entities;
using ExpenseTracker.Data.Seeding;
using ExpenseTracker.Services;
using ExpenseTracker.Services.Config;
using ExpenseTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//DB
builder.Services.AddDbContext<ExpenseTrackerDbContext>(opt =>
opt.UseSqlServer("name=ConnectionStrings:DefaultConnection").UseLazyLoadingProxies());

//Add cache
builder.Services.AddDistributedMemoryCache();

//Add identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
	.AddEntityFrameworkStores<ExpenseTrackerDbContext>()
	.AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
	// Default Lockout settings.
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
	options.Lockout.MaxFailedAccessAttempts = 5;
	options.Lockout.AllowedForNewUsers = true;
});


builder.Services.Configure<IdentityOptions>(options =>
{
	// Default Password settings.
	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireNonAlphanumeric = true;
	options.Password.RequireUppercase = true;
	options.Password.RequiredLength = 8;
	options.Password.RequiredUniqueChars = 1;
});

//Token auth
var tokenSection = builder.Configuration.GetSection("Token");
var tokenConfig = tokenSection.Get<TokenConfig>();
builder.Services.Configure<TokenConfig>(tokenSection);

builder.Services.AddAuthentication(x =>
{
	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
	var key = Encoding.UTF8.GetBytes(tokenConfig.Key);
	o.SaveToken = true;
	o.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = false,
		ValidateAudience = false,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = tokenConfig.Issuer,
		ValidAudience = tokenConfig.Audience,
		IssuerSigningKey = new SymmetricSecurityKey(key)
	};
});

builder.Services.AddCors(c =>
{
	c.AddPolicy(name: "AllowOrigins",
	 options =>
	 {
		 options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
	 });

});

//Services
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<ITokenHandlerService, TokenHandlerService>();
builder.Services.AddTransient<IProductService, ProductService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
using (var serviceScope = app.Services.CreateScope())
{
	var dbContext = serviceScope.ServiceProvider.GetRequiredService<ExpenseTrackerDbContext>();
	dbContext.Database.Migrate();
	new ExpenseTrackerDbSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
