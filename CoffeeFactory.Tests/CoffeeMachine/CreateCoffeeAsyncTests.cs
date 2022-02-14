using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;

namespace CoffeeChallenge.CoffeeFactory.Tests;

public class CreateCoffeeAsyncTests
{
    private CoffeeMachine subject;

    private IOutgoingGoods outgoingGoods;

    [SetUp]
    public void Setup()
    {
        outgoingGoods = A.Fake<IOutgoingGoods>();
        subject = new CoffeeMachine(outgoingGoods);
    }

    [Test]
    public async Task SubjectDepositsOneCoffeeOnCall()
    {
        await subject.CreateCoffeeAsync();

        A.CallTo(() => outgoingGoods.DepositCoffeeAsync(1)).MustHaveHappenedOnceExactly();
    }
}