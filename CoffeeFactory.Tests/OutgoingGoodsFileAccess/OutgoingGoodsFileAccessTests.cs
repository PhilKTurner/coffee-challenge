using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CoffeeChallenge.CoffeeFactory.Tests;

public class OutgoingGoodsFileAccessTests
{
    private OutgoingGoodsFileAccess subject;

    [SetUp]
    public void SetUp()
    {
        var testFile = Path.GetTempFileName();

        subject = new OutgoingGoodsFileAccess(testFile);
    }

    [Test]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(42)]
    [TestCase(int.MaxValue)]
    public async Task SubjectWritesAndReadsCorrectly(int expectedValue)
    {
        await subject.WriteAsync(expectedValue);

        var actualValue = await subject.ReadAsync();

        Assert.AreEqual(expectedValue, actualValue);
    }
}