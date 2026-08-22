namespace SentinelaTrade.Console.Analysis;

public static class TrendAnalyzer
{
    public static string Analyze(decimal currentPrice, decimal ema9, decimal ema21, decimal ema50)
    {
        if (currentPrice > ema9 && ema9 > ema21 && ema21 > ema50)
        {
            return "ALTA";
        }

        if (currentPrice < ema9 && ema9 < ema21 && ema21 < ema50)
        {
            return "BAIXA";
        }

        return "NEUTRO";
    }
}
