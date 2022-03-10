using CoffeeChallenge.Contracts;

namespace CoffeeChallenge.CoffeeFactory.Distribution;

public interface IOutgoingGoodsBackUp
{
    Task WriteAsync(IEnumerable<Coffee> coffees);
    Task<IEnumerable<Coffee>> ReadAsync();
}