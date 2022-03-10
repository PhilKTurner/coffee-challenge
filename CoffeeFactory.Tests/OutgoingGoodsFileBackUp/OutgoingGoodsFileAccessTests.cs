using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoffeeChallenge.CoffeeFactory.Distribution;
using CoffeeChallenge.Contracts;
using NUnit.Framework;

namespace CoffeeChallenge.CoffeeFactory.Tests;

public class OutgoingGoodsFileBackUpTests
{
    private OutgoingGoodsFileBackUp subject;

    [SetUp]
    public void SetUp()
    {
        var testFile = Path.GetTempFileName();

        subject = new OutgoingGoodsFileBackUp(testFile);
    }

    [Test]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(42)]
    public async Task SubjectWritesAndReadsCorrectly(int objectCount)
    {
        var testCollection = CoffeeCreator.CreateListOfCoffees(objectCount);

        await subject.WriteAsync(testCollection);

        var readCollection = await subject.ReadAsync();

        Assert.AreEqual(objectCount, readCollection.Count());
        Assert.IsTrue(readCollection.All(c => testCollection.Any(x => x.Id == c.Id)));
    }
}