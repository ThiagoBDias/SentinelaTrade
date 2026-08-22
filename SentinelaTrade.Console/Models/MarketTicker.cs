namespace SentinelaTrade.Console.Models;

public class MarketTicker
{
    public string Symbol { get; set; } = "";
    public decimal LastPrice { get; set; }
    public decimal Change24h { get; set; }
}