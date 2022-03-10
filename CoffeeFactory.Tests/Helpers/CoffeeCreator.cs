using System;
using System.Collections.Generic;
using CoffeeChallenge.Contracts;

namespace CoffeeChallenge.CoffeeFactory.Tests;

public static class CoffeeCreator
{
    public static List<Coffee> CreateListOfCoffees(int coffeeCount)
    {
        var coffees = new List<Coffee>();
        for (int i = 0; i < coffeeCount; i++)
        {
            coffees.Add(new Coffee{Id = Guid.NewGuid()});
        }

        return coffees;
    }
}
