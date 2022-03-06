namespace CoffeeChallenge.CoffeeFactory;

public class OutgoingGoods : IOutgoingGoods
{
    private readonly IOutgoingGoodsFileAccess fileAccess;

    public OutgoingGoods(IOutgoingGoodsFileAccess fileAccess)
    {
        this.fileAccess = fileAccess;
    }

    // TODO On a 5s interval concurrency issues shouldn't be a problem here, ... right?

    public async Task DepositCoffeeAsync(int count)
    {
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        var coffeeCount = await fileAccess.ReadAsync();
        var oldCoffeeCount = coffeeCount;
        coffeeCount += count;

        if (coffeeCount < oldCoffeeCount)
            throw new OverflowException("Coffee overflow!"); // TODO remove explicit overflow handling

        await fileAccess.WriteAsync(coffeeCount);
    }

    public async Task<int> CollectOutgoingGoodsAsync()
    {
        var coffeeCount = await fileAccess.ReadAsync();
        await fileAccess.WriteAsync(0);

        return coffeeCount;
    }
}