using CoffeeChallenge.Contracts;
using CoffeeChallenge.CoffeeStore.Sales;
using CoffeeChallenge.CoffeeStore.Storage;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CoffeeChallenge.CoffeeStore.Controllers;

[ApiController]
[Route("[controller]")]
public class CoffeeController : ControllerBase
{
    private readonly ILogger<CoffeeController> _logger;
    private readonly ICoffeeStorage storage;
    private readonly IStoreClerk clerk;

    public CoffeeController(ILogger<CoffeeController> logger, ICoffeeStorage storage, IStoreClerk clerk)
    {
        _logger = logger;
        this.storage = storage;
        this.clerk = clerk;
    }

    /// <summary>
    /// Delivers coffee to the store's storage.
    /// </summary>
    /// <param name="coffees">The delivery, a collection of coffees</param>
    /// <response code="200">Delivery was successfully stored</response>
    /// <response code="400">Coffee payload invalid</response>
    /// <response code="500">Database error</response>
    // TODO Prevent others than CoffeeFactory from making deliveries?
    [HttpPut("[action]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult Deliver(IEnumerable<Coffee> coffees)
    {
        if (coffees is null)
            return this.BadRequest("No coffees found in request body.");

        storage.StoreCoffee(coffees);
        return this.Ok();
    }

    /// <summary>
    /// Checks how many coffees are available in the store's storage.
    /// </summary>
    /// <returns>The number of coffees in storage</returns>
    /// <response code="200">Returns the number of coffees in storage</response>
    /// <response code="500">Database error</response>
    [HttpGet("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public int Available()
    {
        return storage.GetCoffeeCount();
    }

    /// <summary>
    /// Buys coffee.
    /// </summary>
    /// <param name="requestedAmount">Requested amount of coffee</param>
    /// <returns>The purchased coffees</returns>
    /// <response code="200">Purchase successful</response>
    /// <response code="400"><paramref name="requestedAmount"/> is too small</response>
    /// <response code="409">Not enough coffee in storage to fill the order</response>
    [HttpGet("[action]")]
    [ProducesResponseType(typeof(IEnumerable<Coffee>), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    public ActionResult<IEnumerable<Coffee>> Buy(int requestedAmount = 1)
    {
        if (requestedAmount <= 0)
            return this.BadRequest("requestedAmount has to be at least 1");

        try
        {
            var purchasedCoffees = clerk.BuyCoffee(requestedAmount);
            return this.Ok(purchasedCoffees);
        }
        catch (InvalidOperationException exception)
        {
            return this.Conflict(exception.Message);
        }
    }
}
