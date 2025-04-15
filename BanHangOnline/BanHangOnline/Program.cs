using Entities;
using Entities.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using BanHangOnline;
using BanHangOnline.Middleware;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30); // Time out session
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

// Create DB context
// Create DB context
builder.Services.AddDbContext<WebStoreDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
	options.Password.RequiredLength = 6;
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

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    // enfores authoriation policy (user must be authenticated) for all the action methods
    options.AddPolicy("NotAuthenticated", policy =>
    {
        policy.RequireAssertion(context =>
        {
            return !context.User.Identity.IsAuthenticated;
        });
    });
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Admin/Account/Login";
});


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

app.UseSession();

app.UseMiddleware<SessionMiddleware>();

app.UseRouting();

app.UseAuthorization();

app.RegisterRoutes();

app.Run();

// Data Source=DESKTOP-S9HPOGD\K;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False
