using CoffeeChallenge.CoffeeFactory.Distribution;
using CoffeeChallenge.Contracts;

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
        var newCoffee = new Coffee()
        {
            Id = Guid.NewGuid()
        };

        await outgoingGoods.DepositCoffeeAsync(newCoffee);
    }
}