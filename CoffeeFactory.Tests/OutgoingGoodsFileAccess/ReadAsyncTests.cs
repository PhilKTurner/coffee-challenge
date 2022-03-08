using System.IO;
using System.Threading.Tasks;
using CoffeeChallenge.CoffeeFactory.Distribution;
using NUnit.Framework;

namespace CoffeeChallenge.CoffeeFactory.Tests;

public class ReadAsyncTests
{
    private OutgoingGoodsFileAccess subject;

    private FileInfo testFile;

    [SetUp]
    public void SetUp()
    {
        var testFilePath = Path.GetTempFileName();
        testFile = new FileInfo(testFilePath);

        subject = new OutgoingGoodsFileAccess(testFilePath);
    }

    [Test]
    public void SubjectThrowsIfFileIsAlreadyOpen()
    {        
        using (var stream = testFile.OpenWrite())
        {
            Assert.ThrowsAsync<IOException>(() => subject.ReadAsync());
        }
    }

    [Test]
    public async Task SubjectReturnsZeroIfFileDoesNotExist()
    {
        testFile.Delete();

        var actualValue = await subject.ReadAsync();

        Assert.AreEqual(0, actualValue);
    }

    [Test]
    public async Task SubjectReturnsZeroIfFileIsEmpty()
    {
        var actualValue = await subject.ReadAsync();

        Assert.AreEqual(0, actualValue);
    }
}