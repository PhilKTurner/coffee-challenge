namespace CoffeeChallenge.CoffeeFactory;

public interface IOutgoingGoods
{
    Task DepositCoffeeAsync(int count);

    Task<int> CollectOutgoingGoodsAsync();
}
