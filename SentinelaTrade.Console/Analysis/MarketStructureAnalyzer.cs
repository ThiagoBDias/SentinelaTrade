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

    public string GetStructure(List<Candle> candles)
    {
        if (candles.Count < 10)
            return "DADOS INSUFICIENTES";

        var swingHighs = new List<decimal>();
        var swingLows = new List<decimal>();

        for (int i = 2; i < candles.Count - 2; i++)
        {
            var current = candles[i];

            bool isSwingHigh =
                current.High > candles[i - 1].High &&
                current.High > candles[i - 2].High &&
                current.High > candles[i + 1].High &&
                current.High > candles[i + 2].High;

            bool isSwingLow =
                current.Low < candles[i - 1].Low &&
                current.Low < candles[i - 2].Low &&
                current.Low < candles[i + 1].Low &&
                current.Low < candles[i + 2].Low;

            if (isSwingHigh)
                swingHighs.Add(current.High);

            if (isSwingLow)
                swingLows.Add(current.Low);
        }

        if (swingHighs.Count < 2 || swingLows.Count < 2)
            return "LATERAL";

        var previousHigh = swingHighs[^2];
        var lastHigh = swingHighs[^1];

        var previousLow = swingLows[^2];
        var lastLow = swingLows[^1];

        bool higherHigh = lastHigh > previousHigh;
        bool higherLow = lastLow > previousLow;

        bool lowerHigh = lastHigh < previousHigh;
        bool lowerLow = lastLow < previousLow;

        if (higherHigh && higherLow)
            return "ALTA";

        if (lowerHigh && lowerLow)
            return "BAIXA";

        return "LATERAL";
    }
}