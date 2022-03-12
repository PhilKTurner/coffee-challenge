using CoffeeChallenge.Contracts;

namespace CoffeeChallenge.CoffeeStore.Storage;

public interface ICoffeeStorage
{
    int GetCoffeeCount();
    void StoreCoffee(IEnumerable<Coffee> coffeesToStore);
    void RetrieveCoffee(int count);
}
