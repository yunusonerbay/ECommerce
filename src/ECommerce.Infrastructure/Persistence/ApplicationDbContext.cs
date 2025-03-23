using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Status)
                    .HasConversion<string>();

                entity.Property(e => e.TotalAmount)
                    .HasPrecision(18, 2);

                entity.OwnsMany(e => e.Items, itemBuilder =>
                {
                    itemBuilder.WithOwner().HasForeignKey("OrderId");
                    itemBuilder.Property(e => e.UnitPrice).HasPrecision(18, 2);
                });
            });
        }
    }
}