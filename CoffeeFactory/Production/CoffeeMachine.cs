using CoffeeChallenge.CoffeeFactory.Distribution;

namespace CoffeeChallenge.CoffeeFactory.Production;

public class CoffeeMachine : ICoffeeMachine
{
    private readonly IOutgoingGoods outgoingGoods;

    public CoffeeMachine(IOutgoingGoods outgoingGoods)
    {
        this.outgoingGoods = outgoingGoods;
    }

    public async Task CreateCoffeeAsync()
    {
        // TODO generate objects
        await outgoingGoods.DepositCoffeeAsync(1);
    }
}