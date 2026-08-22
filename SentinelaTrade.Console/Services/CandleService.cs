using System.Globalization;
using System.Text.Json;
using SentinelaTrade.Console.Models;

namespace SentinelaTrade.Console.Services;

public class CandleService
{
    private readonly HttpClient _client = new();

    public async Task<List<Candle>> GetCandlesAsync(string symbol, string interval = "15", int limit = 200)
    {
        var url =
            $"https://api.bybit.com/v5/market/kline?category=linear&symbol={symbol}&interval={interval}&limit={limit}";

        var response = await _client.GetStringAsync(url);

        using var document = JsonDocument.Parse(response);

        var list = document.RootElement
            .GetProperty("result")
            .GetProperty("list");

        var candles = new List<Candle>(list.GetArrayLength());

        foreach (var item in list.EnumerateArray())
        {
            var openTimeMs = long.Parse(item[0].GetString() ?? "0", CultureInfo.InvariantCulture);

            candles.Add(new Candle
            {
                OpenTime = DateTimeOffset.FromUnixTimeMilliseconds(openTimeMs).UtcDateTime,
                Open = decimal.Parse(item[1].GetString() ?? "0", CultureInfo.InvariantCulture),
                High = decimal.Parse(item[2].GetString() ?? "0", CultureInfo.InvariantCulture),
                Low = decimal.Parse(item[3].GetString() ?? "0", CultureInfo.InvariantCulture),
                Close = decimal.Parse(item[4].GetString() ?? "0", CultureInfo.InvariantCulture),
                Volume = decimal.Parse(item[5].GetString() ?? "0", CultureInfo.InvariantCulture)
            });
        }

        candles.Sort((a, b) => a.OpenTime.CompareTo(b.OpenTime));

        return candles;
    }
}
