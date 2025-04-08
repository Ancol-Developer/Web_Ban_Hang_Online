using Entities.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class WebStoreDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    public WebStoreDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Adv> Adv { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<Contact> Contact { get; set; }
    public DbSet<News> News { get; set; }
    public DbSet<Order> Order { get; set; }
    public DbSet<OrderDetail> OrderDetail { get; set; }
    public DbSet<Posts> Posts { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<ProductCategory> ProductCategory { get; set; }
    public DbSet<ProductImage> ProductImage { get; set; }
    public DbSet<Subscribe> Subscribe { get; set; }
    public DbSet<SystemSetting> SystemSetting { get; set; }
    public DbSet<ThongKe> Thongke { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
