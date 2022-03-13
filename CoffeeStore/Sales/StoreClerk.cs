using CoffeeChallenge.CoffeeStore.Storage;
using CoffeeChallenge.Contracts;

namespace CoffeeChallenge.CoffeeStore.Sales;

public class StoreClerk : IStoreClerk
{
    private readonly ICoffeeStorage storage;

    public StoreClerk(ICoffeeStorage storage)
    {
        this.storage = storage;
    }

    public IEnumerable<Coffee> BuyCoffee(int count)
    {
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        var availableCoffee = storage.GetCoffeeCount();
        if (count > availableCoffee)
            throw new InvalidOperationException("Not enough coffee available.");

        var retrievedCoffee = storage.RetrieveCoffee(count);

        return retrievedCoffee;
    }
}
