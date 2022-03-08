namespace CoffeeChallenge.CoffeeFactory.Distribution;

public class OutgoingGoodsFileAccess : IOutgoingGoodsFileAccess
{
    private readonly string filePath;

    public OutgoingGoodsFileAccess(string filePath)
    {
        this.filePath = filePath;
    }

    public async Task<int> ReadAsync()
    {
        var fileInfo = new FileInfo(filePath);

        if (!fileInfo.Exists)
            return 0;

        using (var reader = fileInfo.OpenText())
        {
            var fileContent = await reader.ReadLineAsync();
            if (fileContent == null)
                return 0;

            return int.Parse(fileContent);
        }
    }

    public async Task WriteAsync(int coffeeCount)
    {
        var fileInfo = new FileInfo(filePath);

        using (var writer = fileInfo.CreateText())
        {
            await writer.WriteLineAsync($"{coffeeCount}");
        }
    }
}