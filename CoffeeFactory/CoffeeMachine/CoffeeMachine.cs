namespace CoffeeChallenge.CoffeeFactory; // TODO make file structure and namespaces consistent

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