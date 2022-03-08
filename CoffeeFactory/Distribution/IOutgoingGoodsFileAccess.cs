namespace CoffeeChallenge.CoffeeFactory.Distribution;

public interface IOutgoingGoodsFileAccess
{
    Task WriteAsync(int coffeeCount);
    Task<int> ReadAsync();
}