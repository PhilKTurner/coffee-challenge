using System;
using System.Collections.Generic;
using CoffeeChallenge.CoffeeStore.DataAccess;
using CoffeeChallenge.CoffeeStore.Storage;
using CoffeeChallenge.Contracts;
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

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(42)]
    public void SubjectReturnsCorrectCount(int testCount)
    {
        using (var testContext = CreateContext())
        {
            var testCollection = new List<Coffee>();
            for (int i = 0; i < testCount; i++)
            {
                testCollection.Add(new Coffee { Id = Guid.NewGuid() });
            }

            testContext.AddRange(testCollection);
            testContext.SaveChanges();

            var subject = new CoffeeStorage(testContext);

            var actualCount = subject.GetCoffeeCount();

            Assert.AreEqual(testCount, actualCount);
        }
    }
}
