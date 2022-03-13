using CoffeeChallenge.Contracts;

namespace CoffeeChallenge.CoffeeStore.Storage;

public interface ICoffeeStorage
{
    int GetCoffeeCount();
    void StoreCoffee(IEnumerable<Coffee> coffeesToStore);
    IEnumerable<Coffee> RetrieveCoffee(int count);
}
