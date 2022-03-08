using System.Threading.Tasks;
using CoffeeChallenge.CoffeeFactory.Distribution;

namespace CoffeeChallenge.CoffeeFactory.Tests;

public class OutgoingGoodsFileAccessMock : IOutgoingGoodsFileAccess
{
    private int coffeeCount = 0;

    public Task<int> ReadAsync()
    {
        return Task.FromResult(coffeeCount);
    }

    public Task WriteAsync(int coffeeCount)
    {
        this.coffeeCount = coffeeCount;

        return Task.CompletedTask;
    }
}