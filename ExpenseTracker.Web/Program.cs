using ExpenseTracker.Data;
using ExpenseTracker.Data.Entities;
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

builder.Services.AddDbContext<ExpenseTrackerDbContext>(opt =>
opt.UseSqlServer("name=ConnectionStrings:DefaultConnection").UseLazyLoadingProxies());

builder.Services.AddDistributedMemoryCache();

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
	.AddEntityFrameworkStores<ExpenseTrackerDbContext>()
	.AddDefaultTokenProviders();

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

builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<ITokenHandlerService, TokenHandlerService>();


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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
