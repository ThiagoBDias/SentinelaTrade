using System.Text.Json;
using System.Globalization;

var client = new HttpClient();

var url = "https://api.bybit.com/v5/market/tickers?category=linear&symbol=BTCUSDT";

var response = await client.GetStringAsync(url);

using var document = JsonDocument.Parse(response);

var ticker = document.RootElement
    .GetProperty("result")
    .GetProperty("list")[0];

var symbol = ticker.GetProperty("symbol").GetString() ?? "N/A";

var priceText = ticker.GetProperty("lastPrice").GetString() ?? "0";
var changeText = ticker.GetProperty("price24hPcnt").GetString() ?? "0";

var price = decimal.Parse(
    priceText,
    CultureInfo.InvariantCulture
);

var change = decimal.Parse(
    changeText,
    CultureInfo.InvariantCulture
);

Console.WriteLine("=================================");
Console.WriteLine("      SENTINELA TRADE v0.1");
Console.WriteLine("=================================");

Console.WriteLine($"Ativo: {symbol}");
Console.WriteLine($"Preço: ${price:N2}");
Console.WriteLine($"Variação 24h: {change * 100:F2}%");