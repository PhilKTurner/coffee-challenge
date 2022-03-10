using CoffeeChallenge.CoffeeFactory.Distribution;
using CoffeeChallenge.CoffeeFactory.Production;
using CoffeeFactory;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<ICoffeeMachine, CoffeeMachine>();
        services.AddSingleton<IOutgoingGoods, OutgoingGoods>();
        services.AddSingleton<IDistributor, Distributor>();
        services.AddHttpClient("CoffeeStoreClient",
            client =>
            {
                client.BaseAddress = new Uri("http://cc-coffee-store:5000/");
            });
        services.AddSingleton<IOutgoingGoodsBackUp>((serviceProvider) => new OutgoingGoodsFileBackUp("/mnt/data/outgoing-goods"));
    })
    .Build();

await host.RunAsync();
