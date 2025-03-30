using DevConnect.Models;
using DevConnect.Services;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AspIdentityContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddDbContext<DevConnectDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Added for authentication
builder.Services.AddAuthentication(BearerTokenDefaults.AuthenticationScheme)
    .AddBearerToken(BearerTokenDefaults.AuthenticationScheme);

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<AspIdentityContext>()
    .AddApiEndpoints();

// Add OpenAPI services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DevConnectDbContext>();
builder.Services.AddScoped<IDevConnectService, DevConnectServiceImpl>();



var app = builder.Build();

// Configure the HTTP request pipeline.

ApplyMigrations(app);
app.UseSwagger();
app.UseSwaggerUI();
app.UseExceptionHandler("/Home/Error");
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();


//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//for auth
app.MapIdentityApi<User>();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();


static void ApplyMigrations(IApplicationBuilder app)
{
    using var scope = app.ApplicationServices.CreateScope();
    using var context = scope.ServiceProvider.GetRequiredService<AspIdentityContext>();
    context.Database.Migrate();
    
    using var context2 = scope.ServiceProvider.GetRequiredService<DevConnectDbContext>();
    context2.Database.Migrate();
    
    using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    
    DevConnectDbContextSeeder.SeedData(context2, userManager);
}