using CoffeeChallenge.Contracts;

namespace CoffeeChallenge.CoffeeFactory.Distribution;

public class OutgoingGoods : IOutgoingGoods
{
    private readonly IOutgoingGoodsBackUp goodsBackUp;

    public OutgoingGoods(IOutgoingGoodsBackUp goodsBackUp)
    {
        this.goodsBackUp = goodsBackUp;
    }

    // TODO On a 5s interval concurrency issues shouldn't be a problem here, ... right?

    public async Task DepositCoffeeAsync(Coffee coffee)
    {
        if (coffee is null)
            throw new ArgumentNullException(nameof(coffee));

        var currentCoffees = await goodsBackUp.ReadAsync();
        currentCoffees = currentCoffees.Append(coffee);

        await goodsBackUp.WriteAsync(currentCoffees);
    }

    public async Task RemoveCoffeesAsync(IEnumerable<Coffee> coffeesToRemove)
    {
        if (coffeesToRemove is null)
            throw new ArgumentNullException(nameof(coffeesToRemove));

        var currentCoffees = await goodsBackUp.ReadAsync();
        currentCoffees = currentCoffees.ExceptBy<Coffee, Guid>(coffeesToRemove.Select(c => c.Id), c => c.Id);

        await goodsBackUp.WriteAsync(currentCoffees);
    }

    public async Task<IEnumerable<Coffee>> GetCoffeesAsync()
    {
        var currentCoffees = await goodsBackUp.ReadAsync();

        return currentCoffees;
    }
}