namespace CoffeeChallenge.CoffeeFactory;

public class CoffeeMachine : ICoffeeMachine
{
    private readonly IOutgoingGoods outgoingGoods;

    public CoffeeMachine(IOutgoingGoods outgoingGoods)
    {
        this.outgoingGoods = outgoingGoods;
    }

    public async Task CreateCoffeeAsync()
    {
        await outgoingGoods.DepositCoffeeAsync(1);
    }
}