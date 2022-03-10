using System.Threading.Tasks;
using CoffeeChallenge.CoffeeFactory.Distribution;
using CoffeeChallenge.CoffeeFactory.Production;
using CoffeeChallenge.Contracts;
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

        A.CallTo(() => outgoingGoods.DepositCoffeeAsync(A<Coffee>.Ignored)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public async Task SubjectDepositsCoffeeWithId()
    {
        await subject.CreateCoffeeAsync();

        A.CallTo(() => outgoingGoods.DepositCoffeeAsync(A<Coffee>.That.Matches(c => c.Id !=  System.Guid.Empty))).MustHaveHappenedOnceExactly();
    }
}