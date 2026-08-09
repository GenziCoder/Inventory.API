using Inventory.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            modelBuilder.Entity<RefreshToken>()
               .HasOne(x => x.User)
               .WithMany(x => x.RefreshTokens)
               .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Purchases)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PurchaseDetail>()
                .HasOne(pd => pd.Purchase)
                .WithMany(p => p.PurchaseDetails)
                .HasForeignKey(pd => pd.PurchaseId);

            modelBuilder.Entity<PurchaseDetail>()
                .HasOne(pd => pd.Product)
                .WithMany(p => p.PurchaseDetails)
                .HasForeignKey(pd => pd.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SaleDetail>()
                .HasOne(sd => sd.Sale)
                .WithMany(s => s.SaleDetails)
                .HasForeignKey(sd => sd.SaleId);

            modelBuilder.Entity<SaleDetail>()
                .HasOne(sd => sd.Product)
                .WithMany(p => p.SaleDetails)
                .HasForeignKey(sd => sd.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Customer)
                .WithMany(c => c.Sales)
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<User>()
                .Property(x => x.Role)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .HasIndex(x => x.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(x => x.Username)
                .IsUnique();
        }
      
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseDetail> PurchaseDetails { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}