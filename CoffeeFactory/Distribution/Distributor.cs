using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace CoffeeChallenge.CoffeeFactory.Distribution;

public class Distributor : IDistributor
{
    private const string requestUri = "/coffee/deliver";

    private readonly IOutgoingGoods outgoingGoods;
    private readonly IHttpClientFactory httpClientFactory;

    public Distributor(IOutgoingGoods outgoingGoods, IHttpClientFactory httpClientFactory)
    {
        this.outgoingGoods = outgoingGoods;
        this.httpClientFactory = httpClientFactory;
    }

    public async Task DeliverCoffeeAsync()
    {
        var collectedCoffees = await outgoingGoods.GetCoffeesAsync();
        var amountToDeliver = collectedCoffees.Count();

        if (amountToDeliver == 0)
            return;

        var deliverySuccess = false;
        try
        {
            var httpClient = httpClientFactory.CreateClient("CoffeeStoreClient");

            var options = new JsonSerializerOptions() { WriteIndented = true };
            var coffeesAsJson = JsonSerializer.Serialize(collectedCoffees, options);

            var content = new StringContent(coffeesAsJson, Encoding.UTF8, MediaTypeNames.Application.Json);

            var response = await httpClient.PutAsync(requestUri, content);
            deliverySuccess = response.IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
            // can be considered handled by returning the coffee to OutgoingGoods
        }

        if (deliverySuccess)
        {
            await outgoingGoods.RemoveCoffeesAsync(collectedCoffees);
        }
    }
}