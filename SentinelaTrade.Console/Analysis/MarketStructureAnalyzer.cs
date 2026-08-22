using SentinelaTrade.Console.Models;

namespace SentinelaTrade.Console.Analysis;

public class MarketStructureAnalyzer
{
    public decimal GetRecentHigh(List<Candle> candles, int period = 20)
    {
        return candles
            .TakeLast(period)
            .Max(candle => candle.High);
    }

    public decimal GetRecentLow(List<Candle> candles, int period = 20)
    {
        return candles
            .TakeLast(period)
            .Min(candle => candle.Low);
    }

    public string GetStructure(List<Candle> candles, int period = 20)
    {
        if (candles.Count < period)
            return "DADOS INSUFICIENTES";

        var recentCandles = candles.TakeLast(period).ToList();

        var firstHalf = recentCandles.Take(period / 2).ToList();
        var secondHalf = recentCandles.Skip(period / 2).ToList();

        decimal firstHigh = firstHalf.Max(c => c.High);
        decimal secondHigh = secondHalf.Max(c => c.High);

        decimal firstLow = firstHalf.Min(c => c.Low);
        decimal secondLow = secondHalf.Min(c => c.Low);

        if (secondHigh > firstHigh && secondLow > firstLow)
            return "ALTA";

        if (secondHigh < firstHigh && secondLow < firstLow)
            return "BAIXA";

        return "LATERAL";
    }
}