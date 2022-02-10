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

    [HttpPut("[action]/{deliverySize}")]
    public void Deliver(int deliverySize)
    {
        storage.StoreCoffee(deliverySize);
    }

    [HttpGet("[action]")]
    public int Available()
    {
        return storage.GetCoffeeCount();
    }

    [HttpPut("[action]")]
    public void Buy(int requestedAmount = 1)
    {
        clerk.BuyCoffee(requestedAmount);
    }
}
