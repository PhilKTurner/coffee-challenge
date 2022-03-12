using CoffeeChallenge.CoffeeStore.DataAccess;
using CoffeeChallenge.Contracts;

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
        return context.Coffees.Count();
    }

    public void RetrieveCoffee(int count)
    {
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        var availableCoffeeCount = context.Coffees.Count();

        if (count > availableCoffeeCount)
            throw new InvalidOperationException("Not enough coffee available.");

        var coffeeToRemove = context.Coffees.Take(count);
        context.Coffees.RemoveRange(coffeeToRemove);
        context.SaveChanges();
    }

    public void StoreCoffee(IEnumerable<Coffee> coffeesToStore)
    {
        if (coffeesToStore is null)
            throw new ArgumentNullException(nameof(coffeesToStore));

        context.Coffees.AddRange(coffeesToStore);
        context.SaveChanges();
    }
}
