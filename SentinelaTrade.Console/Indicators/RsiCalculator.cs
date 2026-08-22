using SentinelaTrade.Console.Models;

namespace SentinelaTrade.Console.Indicators;

public static class RsiCalculator
{
    public static decimal CalculateRsi14(IReadOnlyList<Candle> candles)
    {
        const int period = 14;

        if (candles.Count <= period)
        {
            throw new InvalidOperationException("Candles insuficientes para RSI 14.");
        }

        decimal gains = 0m;
        decimal losses = 0m;

        for (var i = 1; i <= period; i++)
        {
            var change = candles[i].Close - candles[i - 1].Close;
            if (change > 0)
            {
                gains += change;
            }
            else
            {
                losses += -change;
            }
        }

        var averageGain = gains / period;
        var averageLoss = losses / period;

        for (var i = period + 1; i < candles.Count; i++)
        {
            var change = candles[i].Close - candles[i - 1].Close;
            var gain = change > 0 ? change : 0m;
            var loss = change < 0 ? -change : 0m;

            averageGain = ((averageGain * (period - 1)) + gain) / period;
            averageLoss = ((averageLoss * (period - 1)) + loss) / period;
        }

        if (averageLoss == 0m)
        {
            return 100m;
        }

        var rs = averageGain / averageLoss;
        return 100m - (100m / (1m + rs));
    }
}
