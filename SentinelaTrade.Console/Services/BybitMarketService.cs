using System.Globalization;
using System.Text.Json;
using SentinelaTrade.Console.Models;

namespace SentinelaTrade.Console.Services;

public class BybitMarketService
{
    private readonly HttpClient _client = new();

    public async Task<MarketTicker> GetTickerAsync(string symbol)
    {
        var url =
            $"https://api.bybit.com/v5/market/tickers?category=linear&symbol={symbol}";

        var response = await _client.GetStringAsync(url);

        using var document = JsonDocument.Parse(response);

        var ticker = document.RootElement
            .GetProperty("result")
            .GetProperty("list")[0];

        return new MarketTicker
        {
            Symbol = ticker.GetProperty("symbol").GetString() ?? "",
            LastPrice = decimal.Parse(
                ticker.GetProperty("lastPrice").GetString() ?? "0",
                CultureInfo.InvariantCulture
            ),
            Change24h = decimal.Parse(
                ticker.GetProperty("price24hPcnt").GetString() ?? "0",
                CultureInfo.InvariantCulture
            )
        };
    }
}