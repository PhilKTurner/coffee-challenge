using System.IO;
using CoffeeChallenge.CoffeeFactory.Distribution;
using NUnit.Framework;

namespace CoffeeChallenge.CoffeeFactory.Tests;

public class WriteAsyncTests
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
}