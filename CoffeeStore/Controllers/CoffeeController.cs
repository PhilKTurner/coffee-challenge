using Microsoft.AspNetCore.Mvc;

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
    /// <param name="deliverySize">Size of the delivery (must be greater than 0)</param>
    /// <response code="200">Delivery was successfully stored.</response>
    // TODO Prevent others than CoffeeFactory from making deliveries?
    [HttpPut("[action]/{deliverySize}")]
    public void Deliver(int deliverySize)
    {
        storage.StoreCoffee(deliverySize);
    }

    /// <summary>
    /// Checks how many coffees are available in the store's storage.
    /// </summary>
    /// <returns>The number of coffees in storage</returns>
    /// <response code="200">Returns the number of coffees in storage</response>
    [HttpGet("[action]")]
    public int Available()
    {
        return storage.GetCoffeeCount();
    }

    /// <summary>
    /// Buys coffee.
    /// </summary>
    /// <param name="requestedAmount">Requested amount of coffee</param>
    /// <response code="200">Purchase successful</response>
    [HttpPut("[action]")]
    public void Buy(int requestedAmount = 1)
    {
        clerk.BuyCoffee(requestedAmount);
    }
}
