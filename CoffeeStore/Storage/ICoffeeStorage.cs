namespace CoffeeChallenge.CoffeeStore.Storage;

public interface ICoffeeStorage
{
    int GetCoffeeCount();
    void StoreCoffee(int count);
    void RetrieveCoffee(int count);
}
