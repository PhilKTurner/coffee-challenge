using System;
using System.Collections.Generic;
using System.Linq;
using CoffeeChallenge.CoffeeStore.Sales;
using CoffeeChallenge.CoffeeStore.Storage;
using CoffeeChallenge.Contracts;
using FakeItEasy;
using NUnit.Framework;

namespace CoffeeChallenge.CoffeeStore.Tests;

public class BuyCoffeeTests
{
    private StoreClerk subject;

    private ICoffeeStorage storage;

    [SetUp]
    public void Setup()
    {
        storage = A.Fake<ICoffeeStorage>();
        subject = new StoreClerk(storage);

        A.CallTo<int>(() => storage.GetCoffeeCount()).Returns(int.MaxValue);
    }

    [TestCase(1)]
    [TestCase(42)]
    public void SubjectRetrievesRightAmountOfCoffeeFromStorage(int expectedAmount)
    {
        subject.BuyCoffee(expectedAmount);

        A.CallTo(() => storage.RetrieveCoffee(A<int>.That.IsEqualTo(expectedAmount))).MustHaveHappenedOnceExactly();
    }

    [TestCase(0, 1)]
    [TestCase(1, 23)]
    [TestCase(23, 42)]
    public void SubjectChecksInventoryAndThrowsIfNotEnoughCoffeeIsAvailable(int availableCoffee, int testValue)
    {
        A.CallTo<int>(() => storage.GetCoffeeCount()).Returns(availableCoffee);

        Assert.Throws<InvalidOperationException>(() => subject.BuyCoffee(testValue));
    }

    [TestCase(1)]
    [TestCase(42)]
    public void SubjectReturnsCoffeeObjectsFromStorage(int coffeeCount)
    {
        var testCollection = new List<Coffee>();
        for (int i = 0; i < coffeeCount; i++)
        {
            testCollection.Add(new Coffee { Id = Guid.NewGuid() });
        }

        A.CallTo<IEnumerable<Coffee>>(() => storage.RetrieveCoffee(coffeeCount)).Returns(testCollection);
        A.CallTo<int>(() => storage.GetCoffeeCount()).Returns(coffeeCount);

        var actualCoffees = subject.BuyCoffee(coffeeCount);

        Assert.IsTrue(actualCoffees.All(x => testCollection.Any(c => c.Id == x.Id)));
    }
}