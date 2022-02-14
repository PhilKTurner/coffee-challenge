namespace CoffeeChallenge.CoffeeFactory;

public interface IOutgoingGoodsFileAccess
{
    Task WriteAsync(int coffeeCount);
    Task<int> ReadAsync();
}