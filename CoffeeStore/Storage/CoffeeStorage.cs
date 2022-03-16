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

    public IEnumerable<Coffee> RetrieveCoffee(int count)
    {
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        var availableCoffeeCount = context.Coffees.Count();

        // TODO Is this component responsible for doing this?
        if (count > availableCoffeeCount)
            throw new InvalidOperationException("Not enough coffee available.");

        var coffeeToRetrieve = context.Coffees.Take(count).ToList();
        context.Coffees.RemoveRange(coffeeToRetrieve);
        context.SaveChanges();

        return coffeeToRetrieve;
    }

    public void StoreCoffee(IEnumerable<Coffee> coffeesToStore)
    {
        if (coffeesToStore is null)
            throw new ArgumentNullException(nameof(coffeesToStore));

        context.Coffees.AddRange(coffeesToStore);
        context.SaveChanges();
    }
}
