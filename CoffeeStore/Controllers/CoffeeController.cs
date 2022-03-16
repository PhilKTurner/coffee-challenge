using CoffeeChallenge.Contracts;
using CoffeeChallenge.CoffeeStore.Sales;
using CoffeeChallenge.CoffeeStore.Storage;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeChallenge.CoffeeStore.Controllers;

// TODO exception handling & response codes

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
    // TODO Prevent others than CoffeeFactory from making deliveries?
    [HttpPut("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    [HttpGet("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    [HttpGet("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<IEnumerable<Coffee>> Buy(int requestedAmount = 1)
    {
        if (requestedAmount <= 0)
            return this.BadRequest("requestedAmount has to be at least 1");

        var purchasedCoffees = clerk.BuyCoffee(requestedAmount);

        return this.Ok(purchasedCoffees);
    }
}
