using SentinelaTrade.Console.Models;

namespace SentinelaTrade.Console.Indicators;

public static class EmaCalculator
{
    public static decimal CalculateEma9(IReadOnlyList<Candle> candles)
    {
        return CalculateEma(candles, 9);
    }

    public static decimal CalculateEma21(IReadOnlyList<Candle> candles)
    {
        return CalculateEma(candles, 21);
    }

    public static decimal CalculateEma50(IReadOnlyList<Candle> candles)
    {
        return CalculateEma(candles, 50);
    }

    private static decimal CalculateEma(IReadOnlyList<Candle> candles, int period)
    {
        if (candles.Count < period)
        {
            throw new InvalidOperationException($"Candles insuficientes para EMA {period}.");
        }

        var multiplier = 2m / (period + 1m);
        decimal ema = candles.Take(period).Average(c => c.Close);

        for (var i = period; i < candles.Count; i++)
        {
            ema = ((candles[i].Close - ema) * multiplier) + ema;
        }

        return ema;
    }
}
