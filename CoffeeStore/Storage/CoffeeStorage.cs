using CoffeeChallenge.CoffeeStore.DataAccess;

namespace CoffeeChallenge.CoffeeStore.Storage;

public class CoffeeStorage : ICoffeeStorage
{
    private readonly CoffeeStoreContext context;

    public CoffeeStorage(CoffeeStoreContext context)
    {
        this.context = context;
    }

    public int GetCoffeeCount()
    {
        return context.Coffee.Single().Inventory;
    }

    public void RetrieveCoffee(int count)
    {
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        var coffee = context.Coffee.Single();

        if (count > coffee.Inventory)
            throw new InvalidOperationException("Not enough coffee available.");

        coffee.Inventory -= count;
        context.SaveChanges();
    }

    public void StoreCoffee(int count)
    {
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count));
        
        var coffee = context.Coffee.Single();

        coffee.Inventory += count;
        context.SaveChanges();
    }
}
