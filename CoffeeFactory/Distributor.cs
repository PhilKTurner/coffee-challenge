namespace CoffeeChallenge.CoffeeFactory;

public class Distributor : IDistributor
{
    private const string requestUriFormatString = "/coffee/deliver/{0}";

    private readonly IOutgoingGoods outgoingGoods;
    private readonly HttpClient httpClient;

    public Distributor(IOutgoingGoods outgoingGoods, HttpClient httpClient)
    {
        this.outgoingGoods = outgoingGoods;
        this.httpClient = httpClient;
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