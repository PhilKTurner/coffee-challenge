using System;
using CoffeeChallenge.CoffeeFactory.Distribution;
using NUnit.Framework;

namespace CoffeeChallenge.CoffeeFactory.Tests;

public class DepositCoffeeAsyncTests
{
    private OutgoingGoods subject;

    [SetUp]
    public void SetUp()
    {
        subject = new OutgoingGoods(new OutgoingGoodsBackUpMock());
    }

    [Test]
    public void SubjectThrowsIfNull()
    {
        Assert.ThrowsAsync<ArgumentNullException>(() => subject.DepositCoffeeAsync(null!));
    }
}