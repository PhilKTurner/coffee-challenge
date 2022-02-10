using Microsoft.AspNetCore.Mvc;

namespace CoffeeChallenge.CoffeeStore.Controllers;

[ApiController]
[Route("[controller]")]
public class CoffeeController : ControllerBase
{
    private readonly ILogger<CoffeeController> _logger;

    public CoffeeController(ILogger<CoffeeController> logger)
    {
        _logger = logger;
    }

    [HttpPut("[action]")]
    public void Deliver(int deliverySize)
    {
        throw new NotImplementedException();
    }

    [HttpGet("[action]")]
    public int Available()
    {
        throw new NotImplementedException();
    }

    [HttpPut("[action]")]
    public void Buy(int requestedAmount = 1)
    {
        throw new NotImplementedException();
    }
}
