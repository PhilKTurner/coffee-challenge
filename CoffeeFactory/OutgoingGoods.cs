namespace CoffeeChallenge.CoffeeFactory;

public class OutgoingGoods : IOutgoingGoods
{
    private int coffeeCount = 0;

    // TODO On a 5s interval concurrency issues shouldn't be a problem here, ... right?
    // TODO Optionally save coffeeCount to a file in a mounted volume to secure it against crashes

    public Task DepositCoffeeAsync(int count)
    {
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        var oldCoffeeCount = coffeeCount;
        coffeeCount += count;

        if (coffeeCount < oldCoffeeCount)
            throw new OverflowException("Coffee overflow!");

        return Task.CompletedTask;
    }

    public Task<int> CollectOutgoingGoodsAsync()
    {
        var availableCoffeeCount = coffeeCount;
        coffeeCount = 0;

        return Task.FromResult(availableCoffeeCount);
    }
}