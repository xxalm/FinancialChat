using FinancialChat.Application.Interfaces;
using System.Globalization;

namespace FinancialChat.Infrastructure.Clients;

public class StooqStockQuoteClient : IStockQuoteClient {
    private readonly HttpClient _httpClient;

    public StooqStockQuoteClient(HttpClient httpClient) {
        _httpClient = httpClient;
    }

    public async Task<decimal?> GetQuoteAsync(
        string stockCode,
        CancellationToken cancellationToken = default) {
        var url =
            $"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv";

        var response = await _httpClient.GetAsync(url, cancellationToken);

        if (!response.IsSuccessStatusCode)
            return null;

        var csv = await response.Content.ReadAsStringAsync(cancellationToken);

        var lines = csv.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length < 2)
            return null;

        var values = lines[1].Split(',');

        // Close price é a coluna 6 (ohlcv → c)
        if (decimal.TryParse(
            values[6],
            NumberStyles.Any,
            CultureInfo.InvariantCulture,
            out var price)) {
            return price;
        }

        return null;
    }
}
