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
    public void SubjectThrowsIfNull()
    {
        using (var testContext = CreateContext())
        {
            var subject = new CoffeeStorage(testContext);

            Assert.Throws<ArgumentNullException>(() => subject.StoreCoffee(null!));
        }
    }

    [TestCase(0, 1)]
    [TestCase(0, 23)]
    [TestCase(0, 42)]
    [TestCase(23, 1)]
    [TestCase(23, 23)]
    [TestCase(23, 42)]
    public void SubjectAddsCoffeeToInventory(int currentCount, int addedCount)
    {
        using (var testContext = CreateContext())
        {
            var availableCoffees = new List<Coffee>();
            for (int i = 0; i < currentCount; i++)
            {
                availableCoffees.Add(new Coffee { Id = Guid.NewGuid() });
            }

            testContext.AddRange(availableCoffees);
            testContext.SaveChanges();

            var coffeesToStore = new List<Coffee>();
            for (int i = 0; i < addedCount; i++)
            {
                coffeesToStore.Add(new Coffee { Id = Guid.NewGuid() });
            }

            var subject = new CoffeeStorage(testContext);

            subject.StoreCoffee(coffeesToStore);

            var expectedCount = currentCount + addedCount;
            var actualCount = testContext.Coffees.Count();

            Assert.AreEqual(expectedCount, actualCount);
            Assert.IsTrue(coffeesToStore.All(x => testContext.Coffees.Any(c => c.Id == x.Id)));
        }
    }
}
