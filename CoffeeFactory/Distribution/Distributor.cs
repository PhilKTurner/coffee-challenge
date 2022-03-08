namespace CoffeeChallenge.CoffeeFactory.Distribution;

public class Distributor : IDistributor
{
    private const string requestUriFormatString = "/coffee/deliver/{0}";

    private readonly IOutgoingGoods outgoingGoods;
    private readonly IHttpClientFactory httpClientFactory;

    public Distributor(IOutgoingGoods outgoingGoods, IHttpClientFactory httpClientFactory)
    {
        this.outgoingGoods = outgoingGoods;
        this.httpClientFactory = httpClientFactory;
    }

    public async Task DeliverCoffeeAsync()
    {
        var amountToDeliver = await outgoingGoods.CollectOutgoingGoodsAsync();

        if (amountToDeliver == 0)
            return;

        if (amountToDeliver < 0)
            throw new InvalidOperationException();

        var requestUri = string.Format(requestUriFormatString, amountToDeliver);

        var deliverySuccess = false;
        try
        {
            var httpClient = httpClientFactory.CreateClient("CoffeeStoreClient");
            var response = await httpClient.PutAsync(requestUri, null);
            deliverySuccess = response.IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
            // can be considered handled by returning the coffee to OutgoingGoods
        }

        // TODO Should exceptions generally be caught here and handled by returning the coffee?

        if (!deliverySuccess)
        {
            await outgoingGoods.DepositCoffeeAsync(amountToDeliver);
        }
    }
}