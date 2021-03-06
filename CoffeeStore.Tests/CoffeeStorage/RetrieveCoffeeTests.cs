using System;
using System.Collections.Generic;
using System.Linq;
using CoffeeChallenge.CoffeeStore.DataAccess;
using CoffeeChallenge.CoffeeStore.Storage;
using CoffeeChallenge.Contracts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace CoffeeChallenge.CoffeeStore.Tests;

public class RetrieveCoffeeTests
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

    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-42)]
    [TestCase(int.MinValue)]
    public void SubjectThrowsIfCountIsInvalid(int testValue)
    {
        using (var testContext = CreateContext())
        {
            var subject = new CoffeeStorage(testContext);

            Assert.Throws<ArgumentOutOfRangeException>(() => subject.RetrieveCoffee(testValue));
        }
    }

    [TestCase(42, 1)]
    [TestCase(42, 23)]
    [TestCase(42, 42)]
    [TestCase(23, 1)]
    [TestCase(23, 23)]
    [TestCase(1, 1)]
    public void SubjectSubtractsGivenAmountFromInventory(int currentCount, int subtractedCount)
    {
        using (var testContext = CreateContext())
        {
            var testCollection = new List<Coffee>();
            for (int i = 0; i < currentCount; i++)
            {
                testCollection.Add(new Coffee { Id = Guid.NewGuid() });
            }

            testContext.AddRange(testCollection);
            testContext.SaveChanges();

            var subject = new CoffeeStorage(testContext);

            subject.RetrieveCoffee(subtractedCount);

            var expectedCount = currentCount - subtractedCount;
            var actualCount = testContext.Coffees.Count();

            Assert.AreEqual(expectedCount, actualCount);
        }
    }

    [TestCase(0, 1)]
    [TestCase(0, 23)]
    [TestCase(0, 42)]
    [TestCase(1, 23)]
    [TestCase(1, 42)]
    [TestCase(23, 42)]
    public void SubjectThrowsIfInventoryInsufficient(int currentCount, int subtractedCount)
    {
        using (var testContext = CreateContext())
        {
            var testCollection = new List<Coffee>();
            for (int i = 0; i < currentCount; i++)
            {
                testCollection.Add(new Coffee { Id = Guid.NewGuid() });
            }

            testContext.AddRange(testCollection);
            testContext.SaveChanges();

            var subject = new CoffeeStorage(testContext);

            Assert.Throws<InvalidOperationException>(() => subject.RetrieveCoffee(subtractedCount));
        }
    }

    [TestCase(1)]
    [TestCase(42)]
    public void SubjectReturnsCoffeeObjectsFromInventory(int coffeeCount)
    {
        using (var testContext = CreateContext())
        {
            var testCollection = new List<Coffee>();
            for (int i = 0; i < coffeeCount; i++)
            {
                testCollection.Add(new Coffee { Id = Guid.NewGuid() });
            }

            testContext.AddRange(testCollection);
            testContext.SaveChanges();

            var subject = new CoffeeStorage(testContext);

            var actualCoffees = subject.RetrieveCoffee(coffeeCount);

            Assert.IsTrue(actualCoffees.All(x => testCollection.Any(c => c.Id == x.Id)));
        }
    }
}
