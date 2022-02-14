using System;
using System.Linq;
using CoffeeChallenge.CoffeeStore.DataAccess;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace CoffeeChallenge.CoffeeStore.Tests;

public class GetCoffeeCountTests
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

            Assert.Throws<InvalidOperationException>(() => subject.GetCoffeeCount());
        }
    }

    [Test]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(23)]
    [TestCase(42)]
    [TestCase(int.MaxValue)]
    public void SubjectRelaysInventoryEntryCorrectly(int expectedCount)
    {
        using (var testContext = CreateContext())
        {    
            testContext.Coffee.Single().Inventory = expectedCount;
            testContext.SaveChanges();

            var subject = new CoffeeStorage(testContext);

            var actualResult = subject.GetCoffeeCount();

            Assert.AreEqual(expectedCount, actualResult);
        }
    }
}
