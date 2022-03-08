using System;
using System.Threading.Tasks;
using CoffeeChallenge.CoffeeFactory.Distribution;
using NUnit.Framework;

namespace CoffeeChallenge.CoffeeFactory.Tests;

public class DepositCoffeeAsyncTests
{
    private OutgoingGoods subject;

    [SetUp]
    public void SetUp()
    {
        subject = new OutgoingGoods(new OutgoingGoodsFileAccessMock());
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-42)]
    [TestCase(int.MinValue)]
    public void SubjectThrowsIfCountInvalid(int testCount)
    {
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => subject.DepositCoffeeAsync(testCount));
    }

    [Test]
    public async Task SubjectThrowsOnOverflow()
    {
        await subject.DepositCoffeeAsync(int.MaxValue);

        Assert.ThrowsAsync<OverflowException>(() => subject.DepositCoffeeAsync(1));
    }
}