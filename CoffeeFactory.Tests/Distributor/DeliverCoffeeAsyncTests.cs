using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CoffeeChallenge.CoffeeFactory.Distribution;
using CoffeeChallenge.Contracts;
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

    private const string expectedRelativeUri = "/coffee/deliver";

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

    private void CheckSendCall(string expectedRelativeUri, IEnumerable<Coffee> expectedContent)
    {
        if (httpClient.BaseAddress == null)
            throw new Exception("Test setup didn't provide BaseAddress.");

        var expectedUri = new Uri(httpClient.BaseAddress, expectedRelativeUri);

        A.CallTo(messageHandler).Where(x => IsMessageHandlerCallValid(x, expectedUri, expectedContent)).MustHaveHappenedOnceExactly();
    }

    private bool IsMessageHandlerCallValid(IFakeObjectCall call, Uri expectedUri, IEnumerable<Coffee> expectedContent)
    {
        var requestMessage = call.Arguments[0] as HttpRequestMessage;
        if (requestMessage == null)
            throw new Exception("Call argument is no valid HttpRequestMessage.");

        var contentString = requestMessage.Content?.ReadAsStringAsync().Result;
        if (contentString == null)
            throw new Exception("No valid content.");

        var content = JsonSerializer.Deserialize<List<Coffee>>(contentString);
        if (content == null)
            throw new Exception("No valid content");

        Assert.AreEqual(expectedContent.Count(), content.Count());
        Assert.IsTrue(content.All(c => expectedContent.Any(x => x.Id == c.Id)));

        return call.Method.Name == "SendAsync" && requestMessage.Method == HttpMethod.Put && requestMessage.RequestUri == expectedUri;
    }

    [TestCase(1)]
    [TestCase(23)]
    [TestCase(42)]
    public async Task SubjectCallsPutWithCorrectContent(int objectCount)
    {
        var testCollection = CoffeeCreator.CreateListOfCoffees(objectCount);

        A.CallTo(() => outgoingGoods.GetCoffeesAsync()).Returns(testCollection);

        await subject.DeliverCoffeeAsync();

        CheckSendCall(expectedRelativeUri, testCollection);
    }

    [Test]
    public async Task SubjectMakesNoHttpCallIfNoCoffeeAvailable()
    {
        A.CallTo(() => outgoingGoods.GetCoffeesAsync()).Returns(new List<Coffee>());

        await subject.DeliverCoffeeAsync();

        A.CallTo(messageHandler).Where(x => x.Method.Name == "SendAsync").MustNotHaveHappened();
    }

    [Test]
    public async Task SubjectRemovesCorrectCoffeesIfHttpCallSuccessful()
    {
        var testCollection = CoffeeCreator.CreateListOfCoffees(42);

        A.CallTo(() => outgoingGoods.GetCoffeesAsync()).Returns(testCollection);

        messageHandler.StatusCodeToReturn = HttpStatusCode.OK;
        A.CallTo(messageHandler).Where(x => x.Method.Name == "SendAsync").CallsBaseMethod();

        await subject.DeliverCoffeeAsync();

        A.CallTo(() => outgoingGoods.RemoveCoffeesAsync(A<IEnumerable<Coffee>>.Ignored)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public async Task SubjectRemovesNoCoffeeInOutgoingGoodsIfHttpCallFailsDueToConnectivity()
    {
        var testCollection = CoffeeCreator.CreateListOfCoffees(42);

        A.CallTo(messageHandler).Where(x => x.Method.Name == "SendAsync").Invokes(() => throw new HttpRequestException());

        A.CallTo(() => outgoingGoods.GetCoffeesAsync()).Returns(testCollection);

        await subject.DeliverCoffeeAsync();

        A.CallTo(() => outgoingGoods.RemoveCoffeesAsync(A<IEnumerable<Coffee>>.Ignored)).MustNotHaveHappened();
    }

    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.Unauthorized)]
    public async Task SubjectRestoresCorrectAmountIfHttpCallFailsWithStatusCodeOtherThanSuccess(HttpStatusCode statusCode)
    {
        var testCollection = CoffeeCreator.CreateListOfCoffees(42);

        messageHandler.StatusCodeToReturn = statusCode;
        A.CallTo(messageHandler).Where(x => x.Method.Name == "SendAsync").CallsBaseMethod();

        A.CallTo(() => outgoingGoods.GetCoffeesAsync()).Returns(testCollection);

        await subject.DeliverCoffeeAsync();

        A.CallTo(() => outgoingGoods.RemoveCoffeesAsync(A<IEnumerable<Coffee>>.Ignored)).MustNotHaveHappened();
    }
}