using Microsoft.EntityFrameworkCore;

namespace CoffeeChallenge.CoffeeStore.DataAccess;

public class CoffeeStoreContext : DbContext
{
    public CoffeeStoreContext(DbContextOptions<CoffeeStoreContext> options) : base(options)
    {
    }

    public DbSet<Coffee> Coffee { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coffee>().HasData(new Coffee { Id = 1, Inventory = 0 });
    }
}
