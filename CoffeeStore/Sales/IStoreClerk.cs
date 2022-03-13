using CoffeeChallenge.Contracts;

namespace CoffeeChallenge.CoffeeStore.Sales;

public interface IStoreClerk
{
    IEnumerable<Coffee> BuyCoffee(int count);
}
