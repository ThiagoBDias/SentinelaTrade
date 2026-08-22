using SentinelaTrade.Console.Analysis;
using SentinelaTrade.Console.Indicators;
using SentinelaTrade.Console.Models;
using SentinelaTrade.Console.Services;

var marketService = new BybitMarketService();
var candleService = new CandleService();
var structureAnalyzer = new MarketStructureAnalyzer();
var tradeFilter = new TradeFilter();

string[] symbols =
[
    "BTCUSDT",
    "ETHUSDT",
    "SOLUSDT"
];

var dashboardRows = new List<DashboardRow>();

foreach (var symbol in symbols)
{
    var ticker = await marketService.GetTickerAsync(symbol);

    var candles = await candleService.GetCandlesAsync(
        symbol,
        interval: "15",
        limit: 200
    );

    var ema9 = EmaCalculator.CalculateEma9(candles);
    var ema21 = EmaCalculator.CalculateEma21(candles);
    var ema50 = EmaCalculator.CalculateEma50(candles);
    var rsi14 = RsiCalculator.CalculateRsi14(candles);

    var trend = TrendAnalyzer.Analyze(
        ticker.LastPrice,
        ema9,
        ema21,
        ema50
    );

    var signal = SetupAnalyzer.Analyze(
        ticker.LastPrice,
        ema9,
        ema21,
        ema50,
        rsi14
    );

    var structure = structureAnalyzer.GetStructure(candles);

    var recentHigh = structureAnalyzer.GetRecentHigh(candles);

    var recentLow = structureAnalyzer.GetRecentLow(candles);

    // NOVO: filtra o sinal usando tendência + estrutura
    var filterResult = tradeFilter.Analyze(
        trend,
        structure,
        signal
    );

    dashboardRows.Add(
        new DashboardRow
        {
            Symbol = ticker.Symbol,
            Price = ticker.LastPrice,
            Change24h = ticker.Change24h,

            Trend = trend,
            Structure = structure,

            RecentHigh = recentHigh,
            RecentLow = recentLow,

            Rsi14 = rsi14,

            Ema9 = ema9,
            Ema21 = ema21,
            Ema50 = ema50,

            Signal = signal,

            // NOVO
            FilterResult = filterResult
        }
    );
}

Console.WriteLine("============================================================");
Console.WriteLine("                    SENTINELA TRADE v0.6");
Console.WriteLine("                     MARKET DASHBOARD");
Console.WriteLine("============================================================");

Console.WriteLine();

Console.WriteLine(
    $"{"ATIVO",-10} " +
    $"{"PRECO",-18} " +
    $"{"24H",-10} " +
    $"{"TENDENCIA",-11} " +
    $"{"ESTRUTURA",-11} " +
    $"{"RSI",-6} " +
    $"{"FILTRO",-22}"
);

Console.WriteLine();

foreach (var row in dashboardRows)
{
    Console.WriteLine(
        $"{row.Symbol,-10} " +
        $"${row.Price,16:N2} " +
        $"{row.Change24h,9:+0.00%;-0.00%;0.00%} " +
        $"{row.Trend,-11} " +
        $"{row.Structure,-11} " +
        $"{row.Rsi14,6:N2} " +
        $"{row.FilterResult,-22}"
    );
}

Console.WriteLine();

Console.WriteLine("============================================================");
Console.WriteLine("                     ANALISE DETALHADA");
Console.WriteLine("============================================================");

foreach (var row in dashboardRows)
{
    Console.WriteLine();

    Console.WriteLine(row.Symbol);

    Console.WriteLine();

    Console.WriteLine($"Preco:       ${row.Price:N2}");

    Console.WriteLine();

    Console.WriteLine($"EMA 9:       ${row.Ema9:N2}");
    Console.WriteLine($"EMA 21:      ${row.Ema21:N2}");
    Console.WriteLine($"EMA 50:      ${row.Ema50:N2}");

    Console.WriteLine();

    Console.WriteLine($"RSI 14:      {row.Rsi14:N2}");

    Console.WriteLine();

    Console.WriteLine($"Tendencia:   {row.Trend}");
    Console.WriteLine($"Estrutura:   {row.Structure}");

    Console.WriteLine();

    Console.WriteLine($"Resistencia: ${row.RecentHigh:N2}");
    Console.WriteLine($"Suporte:     ${row.RecentLow:N2}");

    Console.WriteLine();

    Console.WriteLine($"LONG SCORE:  {row.Signal.LongScore}/4");
    Console.WriteLine($"SHORT SCORE: {row.Signal.ShortScore}/4");

    Console.WriteLine();

    Console.WriteLine($"DECISAO:     {row.Signal.Direction}");
    Console.WriteLine($"STATUS:      {row.Signal.Status}");

    Console.WriteLine();

    // NOVO
    Console.WriteLine($"FILTRO:      {row.FilterResult}");

    Console.WriteLine();

    Console.WriteLine("============================================================");
}

public class DashboardRow
{
    public string Symbol { get; set; } = "";

    public decimal Price { get; set; }

    public decimal Change24h { get; set; }

    public string Trend { get; set; } = "NEUTRO";

    public string Structure { get; set; } = "LATERAL";

    public decimal RecentHigh { get; set; }

    public decimal RecentLow { get; set; }

    public decimal Rsi14 { get; set; }

    public decimal Ema9 { get; set; }

    public decimal Ema21 { get; set; }

    public decimal Ema50 { get; set; }

    public TradeSignal Signal { get; set; } = new();

    // NOVO
    public string FilterResult { get; set; } = "";
}
