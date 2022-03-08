using System;
using CoffeeChallenge.CoffeeStore.Sales;
using CoffeeChallenge.CoffeeStore.Storage;
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

    [Test]
    public void SubjectRetrievesCoffeeFromStorage()
    {
        subject.BuyCoffee(42);

        A.CallTo(() => storage.RetrieveCoffee(A<int>.Ignored)).MustHaveHappenedOnceExactly();
    }

    [Test]
    [TestCase(1)]
    [TestCase(42)]
    [TestCase(int.MaxValue)]
    public void SubjectRetrievesRightAmountOfCoffeeFromStorage(int expectedAmount)
    {
        subject.BuyCoffee(expectedAmount);

        A.CallTo(() => storage.RetrieveCoffee(A<int>.That.IsEqualTo(expectedAmount))).MustHaveHappenedOnceExactly();
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-42)]
    [TestCase(int.MinValue)]
    public void SubjectThrowsIfCountIsInvalid(int testValue)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => subject.BuyCoffee(testValue));
    }

    [Test]
    [TestCase(0, 1)]
    [TestCase(1, 42)]
    [TestCase(42, int.MaxValue)]
    public void SubjectChecksInventoryAndThrowsIfNotEnoughCoffeeIsAvailable(int availableCoffee, int testValue)
    {
        A.CallTo<int>(() => storage.GetCoffeeCount()).Returns(availableCoffee);

        Assert.Throws<InvalidOperationException>(() => subject.BuyCoffee(testValue));
    }
}