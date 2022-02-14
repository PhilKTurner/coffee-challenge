using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FakeItEasy;
using FakeItEasy.Core;
using NUnit.Framework;

namespace CoffeeChallenge.CoffeeFactory.Tests;

public class DeliverCoffeeAsyncTests
{
    private Distributor subject;

    private IOutgoingGoods outgoingGoods;
    private HttpClient httpClient;
    private TestMessageHandler messageHandler;

    [SetUp]
    public void Setup()
    {
        outgoingGoods = A.Fake<IOutgoingGoods>();

        messageHandler = A.Fake<TestMessageHandler>();
        httpClient = new HttpClient(messageHandler);
        httpClient.BaseAddress = new Uri("http://this.is.a.test/");

        var httpClientFactory = A.Fake<IHttpClientFactory>();
        A.CallTo(() => httpClientFactory.CreateClient(A<string>.Ignored)).Returns(httpClient);

        subject = new Distributor(outgoingGoods, httpClientFactory);
    }

    [TearDown]
    public void TearDown()
    {
        httpClient.Dispose();
    }

    private void CheckSendCall(string expectedRelativeUri)
    {
        if (httpClient.BaseAddress == null)
            throw new Exception("Test setup didn't provide BaseAddress.");

        var expectedUri = new Uri(httpClient.BaseAddress, expectedRelativeUri);

        A.CallTo(messageHandler).Where(x => IsMessageHandlerCallValid(x, expectedUri)).MustHaveHappenedOnceExactly();
    }

    private bool IsMessageHandlerCallValid(IFakeObjectCall call, Uri expectedUri)
    {
        var requestMessage = call.Arguments[0] as HttpRequestMessage;
        if (requestMessage == null)
            throw new Exception("Call argument is no valid HttpRequestMessage.");

        return call.Method.Name == "SendAsync" && requestMessage.Method == HttpMethod.Put && requestMessage.RequestUri == expectedUri;
    }

    [Test]
    [TestCase(1, "/coffee/deliver/1")]
    [TestCase(23, "/coffee/deliver/23")]
    [TestCase(42, "/coffee/deliver/42")]
    public async Task SubjectCallsPutWithCorrectAmount(int testValue, string expectedRelativeUri)
    {
        A.CallTo(() => outgoingGoods.CollectOutgoingGoodsAsync()).Returns(testValue);

        await subject.DeliverCoffeeAsync();

        CheckSendCall(expectedRelativeUri);
    }

    [Test]
    public async Task SubjectMakesNoHttpCallIfNoCoffeAvailable()
    {
        A.CallTo(() => outgoingGoods.CollectOutgoingGoodsAsync()).Returns(0);

        await subject.DeliverCoffeeAsync();

        A.CallTo(messageHandler).Where(x => x.Method.Name == "SendAsync").MustNotHaveHappened();
    }

    [Test]
    [TestCase(-1)]
    [TestCase(-42)]
    [TestCase(int.MinValue)]
    public void SubjectThrowsIfAmountIsInvalid(int testValue)
    {
        A.CallTo(() => outgoingGoods.CollectOutgoingGoodsAsync()).Returns(testValue);

        Assert.ThrowsAsync<InvalidOperationException>(async () => await subject.DeliverCoffeeAsync());
    }

    [Test]
    [TestCase(1)]
    [TestCase(23)]
    [TestCase(42)]
    public async Task SubjectRestoresCorrectAmountIfHttpCallFailsDueToConnectivity(int expectedAmount)
    {
        A.CallTo(() => outgoingGoods.CollectOutgoingGoodsAsync()).Returns(expectedAmount);
        A.CallTo(messageHandler).Where(x => x.Method.Name == "SendAsync").Invokes(() => throw new HttpRequestException());

        await subject.DeliverCoffeeAsync();

        A.CallTo(() => outgoingGoods.DepositCoffeeAsync(expectedAmount)).MustHaveHappenedOnceExactly();
    }

    [Test]
    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.Unauthorized)]
    public async Task SubjectRestoresCorrectAmountIfHttpCallFailsWithStatusCodeOtherThanSuccess(HttpStatusCode statusCode)
    {
        messageHandler.StatusCodeToReturn = statusCode;
        A.CallTo(messageHandler).Where(x => x.Method.Name == "SendAsync").CallsBaseMethod();

        A.CallTo(() => outgoingGoods.CollectOutgoingGoodsAsync()).Returns(42);

        await subject.DeliverCoffeeAsync();

        A.CallTo(() => outgoingGoods.DepositCoffeeAsync(42)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public async Task SubjectRestoresNoCoffeeInOutgoingGoodsIfSuccessful()
    {
        messageHandler.StatusCodeToReturn = HttpStatusCode.OK;
        A.CallTo(messageHandler).Where(x => x.Method.Name == "SendAsync").CallsBaseMethod();

        A.CallTo(() => outgoingGoods.CollectOutgoingGoodsAsync()).Returns(42);

        await subject.DeliverCoffeeAsync();

        A.CallTo(() => outgoingGoods.DepositCoffeeAsync(A<int>.Ignored)).MustNotHaveHappened();
    }
}