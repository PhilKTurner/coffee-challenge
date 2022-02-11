using CoffeeChallenge.CoffeeFactory;

namespace CoffeeFactory;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ICoffeeMachine coffeeMachine;
    private readonly IDistributor distributor;

    public Worker(ILogger<Worker> logger, ICoffeeMachine coffeeMachine, IDistributor distributor)
    {
        _logger = logger;
        this.coffeeMachine = coffeeMachine;
        this.distributor = distributor;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await coffeeMachine.CreateCoffeeAsync();
                await distributor.DeliverCoffeeAsync();

                await Task.Delay(5000, stoppingToken);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Exception occured during working cycle.");
            }
        }
    }
}
