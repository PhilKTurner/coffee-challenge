using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoffeeChallenge.CoffeeFactory.Distribution;
using NUnit.Framework;

namespace CoffeeChallenge.CoffeeFactory.Tests;

public class ReadAsyncTests
{
    private OutgoingGoodsFileBackUp subject;

    private FileInfo testFile;

    [SetUp]
    public void SetUp()
    {
        var testFilePath = Path.GetTempFileName();
        testFile = new FileInfo(testFilePath);

        subject = new OutgoingGoodsFileBackUp(testFilePath);
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
    public async Task SubjectReturnsEmptyIfFileDoesNotExist()
    {
        testFile.Delete();

        var actualCollection = await subject.ReadAsync();

        Assert.AreEqual(0, actualCollection.Count());
    }

    [Test]
    public async Task SubjectReturnsEmptyIfFileIsEmpty()
    {
        var actualCollection = await subject.ReadAsync();

        Assert.AreEqual(0, actualCollection.Count());
    }
}