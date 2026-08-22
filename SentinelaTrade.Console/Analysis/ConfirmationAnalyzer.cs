using SentinelaTrade.Console.Models;

namespace SentinelaTrade.Console.Analysis;

public class ConfirmationAnalyzer
{
    public bool IsLongConfirmed(List<Candle> candles)
    {
        if (candles.Count < 2)
            return false;

        var current = candles[^1];
        var previous = candles[^2];

        bool bullishCandle =
            current.Close > current.Open;

        bool higherClose =
            current.Close > previous.Close;

        return bullishCandle && higherClose;
    }

    public bool IsShortConfirmed(List<Candle> candles)
    {
        if (candles.Count < 2)
            return false;

        var current = candles[^1];
        var previous = candles[^2];

        bool bearishCandle =
            current.Close < current.Open;

        bool lowerClose =
            current.Close < previous.Close;

        return bearishCandle && lowerClose;
    }

    public string Analyze(
        List<Candle> candles,
        string direction
    )
    {
        if (direction == "LONG")
        {
            if (IsLongConfirmed(candles))
                return "CONFIRMADO";

            return "AGUARDANDO";
        }

        if (direction == "SHORT")
        {
            if (IsShortConfirmed(candles))
                return "CONFIRMADO";

            return "AGUARDANDO";
        }

        return "SEM SETUP";
    }
}