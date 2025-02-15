using Entities;
using Entities.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Create DB context
// Create DB context
builder.Services.AddDbContext<WebStoreDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequiredUniqueChars = 3; // eg: AB12AB (unique characters are A,B,1,2)
}) // Config option password
    .AddEntityFrameworkStores<WebStoreDbContext>()
    .AddDefaultTokenProviders() // using setup new password,change email
                                // create repository of user and role de thao tac du lieu nguoi dung trong dbcontext
    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, WebStoreDbContext, int>>()
    .AddRoleStore<RoleStore<ApplicationRole, WebStoreDbContext, int>>();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "MyAreaAdmin",
    areaName: "Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "CategoryProduct",
    pattern: "san-pham/{id?}",
    new { controller = "Product", action = "index" });

app.MapControllerRoute(
    name: "Contact",
    pattern: "lien-he",
    new { controller = "Contact", action = "index" });

app.Run();

// Data Source=DESKTOP-S9HPOGD\K;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False
