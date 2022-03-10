using CoffeeChallenge.Contracts;

namespace CoffeeChallenge.CoffeeFactory.Distribution;

public interface IOutgoingGoods
{
    Task DepositCoffeeAsync(Coffee coffee);
    Task RemoveCoffeesAsync(IEnumerable<Coffee> coffeesToRemove);

    Task<IEnumerable<Coffee>> GetCoffeesAsync();
}
