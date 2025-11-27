using Domain.Classes;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class CompanyDbContext(DbContextOptions<CompanyDbContext> options) : DbContext(options)
{
    public DbSet<Company> Companies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(Company.MaxNameLength);
        });

        base.OnModelCreating(modelBuilder);
    }
}