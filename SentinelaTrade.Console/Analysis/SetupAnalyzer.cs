using SentinelaTrade.Console.Models;

namespace SentinelaTrade.Console.Analysis;

public static class SetupAnalyzer
{
    public static TradeSignal Analyze(decimal currentPrice, decimal ema9, decimal ema21, decimal ema50, decimal rsi)
    {
        var longScore = 0;
        var shortScore = 0;

        if (ema9 > ema21 && ema21 > ema50)
        {
            longScore += 2;
        }

        if (currentPrice > ema9)
        {
            longScore += 1;
        }

        if (rsi >= 50m && rsi <= 70m)
        {
            longScore += 1;
        }

        if (ema9 < ema21 && ema21 < ema50)
        {
            shortScore += 2;
        }

        if (currentPrice < ema9)
        {
            shortScore += 1;
        }

        if (rsi >= 30m && rsi <= 50m)
        {
            shortScore += 1;
        }

        var direction = "NEUTRO";
        if (longScore > shortScore)
        {
            direction = "LONG";
        }
        else if (shortScore > longScore)
        {
            direction = "SHORT";
        }

        var bestScore = Math.Max(longScore, shortScore);
        var status = bestScore switch
        {
            4 => "FORTE",
            3 => "MODERADO",
            _ => "SEM SETUP"
        };

        if (direction == "NEUTRO" || bestScore <= 2)
        {
            direction = "NEUTRO";
        }

        return new TradeSignal
        {
            LongScore = longScore,
            ShortScore = shortScore,
            Direction = direction,
            Status = status
        };
    }
}
