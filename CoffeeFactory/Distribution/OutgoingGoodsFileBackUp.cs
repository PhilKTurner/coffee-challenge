using System.Text.Json;
using CoffeeChallenge.Contracts;

namespace CoffeeChallenge.CoffeeFactory.Distribution;

public class OutgoingGoodsFileBackUp : IOutgoingGoodsBackUp
{
    private readonly string filePath;

    public OutgoingGoodsFileBackUp(string filePath)
    {
        this.filePath = filePath;
    }

    public async Task<IEnumerable<Coffee>> ReadAsync()
    {
        var fileInfo = new FileInfo(filePath);

        if (!fileInfo.Exists)
            return new List<Coffee>();

        using (var reader = fileInfo.OpenText())
        {
            var fileContent = await reader.ReadToEndAsync();
            if (fileContent == null)
                return new List<Coffee>();

            try
            {
                var backedUpCoffees = JsonSerializer.Deserialize<List<Coffee>>(fileContent);
                if (backedUpCoffees == null)
                    return new List<Coffee>();

                return backedUpCoffees;
            }
            catch (JsonException)
            {
                return new List<Coffee>();
            }
        }
    }

    public async Task WriteAsync(IEnumerable<Coffee> coffees)
    {
        var fileInfo = new FileInfo(filePath);

        using (var writer = fileInfo.CreateText())
        {
            string jsonString = JsonSerializer.Serialize(coffees);
            await writer.WriteAsync(jsonString);
        }
    }
}