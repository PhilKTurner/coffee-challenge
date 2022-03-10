using System.Collections.Generic;
using System.Threading.Tasks;
using CoffeeChallenge.CoffeeFactory.Distribution;
using CoffeeChallenge.Contracts;

namespace CoffeeChallenge.CoffeeFactory.Tests;

public class OutgoingGoodsBackUpMock : IOutgoingGoodsBackUp
{
    private List<Coffee> coffees = new List<Coffee>();

    public Task<IEnumerable<Coffee>> ReadAsync()
    {
        return Task.FromResult(coffees as IEnumerable<Coffee>);
    }

    public Task WriteAsync(IEnumerable<Coffee> coffees)
    {
        this.coffees = new List<Coffee>(coffees);

        return Task.CompletedTask;
    }
}