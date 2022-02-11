namespace CoffeeChallenge.CoffeeFactory;

public interface IOutgoingGoods
{
    Task DepositCoffeeAsync();
    
    Task<int> CollectOutgoingGoodsAsync();
}
