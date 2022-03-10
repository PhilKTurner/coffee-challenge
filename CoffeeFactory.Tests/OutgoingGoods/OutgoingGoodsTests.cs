using System.Linq;
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
        subject = new OutgoingGoods(new OutgoingGoodsBackUpMock());
    }

    [Test]
    [TestCase(1)]
    [TestCase(23)]
    [TestCase(42)]
    public async Task SubjectProvidesDepositedCoffees(int objectCount)
    {
        var testCollection = CoffeeCreator.CreateListOfCoffees(objectCount);

        foreach (var coffee in testCollection)
        {
            await subject.DepositCoffeeAsync(coffee);
        }

        var returnedCollection = await subject.GetCoffeesAsync();

        CollectionAssert.AreEquivalent(testCollection, returnedCollection);
    }
}