namespace CoffeeChallenge.CoffeeFactory.Distribution;

// TODO rework design of OutgoingGoods

public interface IOutgoingGoods
{
    Task DepositCoffeeAsync(int count);

    Task<int> CollectOutgoingGoodsAsync();
}
