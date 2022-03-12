using CoffeeChallenge.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CoffeeChallenge.CoffeeStore.DataAccess;

public class CoffeeStoreContext : DbContext
{
    public CoffeeStoreContext(DbContextOptions<CoffeeStoreContext> options) : base(options)
    {
    }

    public DbSet<Coffee> Coffees { get; set; } = null!;
}
