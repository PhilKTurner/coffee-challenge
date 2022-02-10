namespace CoffeeChallenge.CoffeeStore;

public class StoreClerk : IStoreClerk
{
    private readonly ICoffeeStorage storage;

    public StoreClerk(ICoffeeStorage storage)
    {
        this.storage = storage;
    }

    public void BuyCoffee(int count)
    {
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        var availableCoffee = storage.GetCoffeeCount();
        if (count > availableCoffee)
            throw new InvalidOperationException("Not enough coffee available.");

        storage.RetrieveCoffee(count);
    }
}
