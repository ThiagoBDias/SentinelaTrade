using SentinelaTrade.Console.Models;

namespace SentinelaTrade.Console.Analysis;

public class TradeFilter
{
    public string Analyze(
        string trend,
        string structure,
        TradeSignal signal)
    {
        if (signal.Status == "SEM SETUP")
            return "SEM SETUP";

        if (signal.Direction == "LONG")
        {
            if (trend == "ALTA" && structure == "ALTA")
                return "ENTRADA VALIDA";

            if (trend == "ALTA" && structure == "LATERAL")
                return "AGUARDAR CONFIRMACAO";

            return "SETUP CONTRARIO";
        }

        if (signal.Direction == "SHORT")
        {
            if (trend == "BAIXA" && structure == "BAIXA")
                return "ENTRADA VALIDA";

            if (trend == "BAIXA" && structure == "LATERAL")
                return "AGUARDAR CONFIRMACAO";

            return "SETUP CONTRARIO";
        }

        return "SEM SETUP";
    }
}