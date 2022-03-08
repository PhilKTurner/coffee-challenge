using System;
using System.Threading.Tasks;
using CoffeeChallenge.CoffeeFactory.Distribution;
using NUnit.Framework;

namespace CoffeeChallenge.CoffeeFactory.Tests;

public class OutgoingGoodsTests
{
    private OutgoingGoods subject;

    [SetUp]
    public void SetUp()
    {
        subject = new OutgoingGoods(new OutgoingGoodsFileAccessMock());
    }

    [Test]
    [TestCase(1)]
    [TestCase(23)]
    [TestCase(42)]
    [TestCase(int.MaxValue)]
    public async Task SubjectProvidesDepositedAmountForCollection(int expectedAmount)
    {
        await subject.DepositCoffeeAsync(expectedAmount);

        var actualResult = await subject.CollectOutgoingGoodsAsync();

        Assert.AreEqual(expectedAmount, actualResult);
    }

    [Test]
    [TestCase(new[] {1,23}, 24)]
    [TestCase(new[] {1,42}, 43)]
    [TestCase(new[] {23,42}, 65)]
    [TestCase(new[] {1,23,42}, 66)]
    public async Task SubjectProvidesSumOfMultipleDepositsForCollection(int[] deposits, int expectedAmount)
    {
        await subject.DepositCoffeeAsync(expectedAmount);

        var actualResult = await subject.CollectOutgoingGoodsAsync();

        Assert.AreEqual(expectedAmount, actualResult);
    }

    [Test]
    public async Task SubjectRemovesCollectedCoffee()
    {
        await subject.DepositCoffeeAsync(42);

        await subject.CollectOutgoingGoodsAsync();
        var actualResult = await subject.CollectOutgoingGoodsAsync();

        Assert.AreEqual(0, actualResult);
    }
}