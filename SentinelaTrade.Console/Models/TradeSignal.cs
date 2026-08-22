namespace SentinelaTrade.Console.Models;

public class TradeSignal
{
    public int LongScore { get; set; }
    public int ShortScore { get; set; }
    public string Direction { get; set; } = "NEUTRO";
    public string Status { get; set; } = "SEM SETUP";
}
