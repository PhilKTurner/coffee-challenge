using System;
using System.Linq;
using CoffeeChallenge.CoffeeStore.DataAccess;
using CoffeeChallenge.CoffeeStore.Storage;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace CoffeeChallenge.CoffeeStore.Tests;

public class StoreCoffeeTests
{
    private SqliteConnection connection;
    private DbContextOptions<CoffeeStoreContext> contextOptions;

    [SetUp]
    public void Setup()
    {
        connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        contextOptions = new DbContextOptionsBuilder<CoffeeStoreContext>()
                .UseSqlite(connection)
                .Options;

        using (var context = new CoffeeStoreContext(contextOptions))
        {
            context.Database.EnsureCreated();
        }
    }

    [TearDown]
    public void TearDown()
    {
        connection.Dispose();
    }

    CoffeeStoreContext CreateContext() => new CoffeeStoreContext(contextOptions);

    [Test]
    public void SubjectThrowsIfInventoryEntryNotSingular()
    {
        using (var testContext = CreateContext())
        {    
            testContext.Coffee.Add(new Coffee());
            testContext.SaveChanges();

            var subject = new CoffeeStorage(testContext);

            Assert.Throws<InvalidOperationException>(() => subject.StoreCoffee(1));
        }
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-42)]
    [TestCase(int.MinValue)]
    public void SubjectThrowsIfCountIsInvalid(int testValue)
    {
        using (var testContext = CreateContext())
        {
            var subject = new CoffeeStorage(testContext);

            Assert.Throws<ArgumentOutOfRangeException>(() => subject.StoreCoffee(testValue));
        }
    }

    [Test]
    [TestCase(0, 1)]
    [TestCase(0, 23)]
    [TestCase(0, 42)]
    [TestCase(23, 1)]
    [TestCase(23, 23)]
    [TestCase(23, 42)]
    public void SubjectAddsGivenAmountToInventory(int currentCount, int addedCount)
    {
        using (var testContext = CreateContext())
        {
            testContext.Coffee.Single().Inventory = currentCount;
            testContext.SaveChanges();

            var subject = new CoffeeStorage(testContext);

            subject.StoreCoffee(addedCount);

            var expectedCount = currentCount + addedCount;
            var actualCount = testContext.Coffee.Single().Inventory;

            Assert.AreEqual(expectedCount, actualCount);
        }
    }
}
