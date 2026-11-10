
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace CreditRisk.RiskDecision.Infrastructure.Integrations;

public class ExposureClient
{
    private readonly HttpClient _httpClient;

    public ExposureClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<JsonElement> GetExposureByOperationAsync(
        Guid creditOperationId,
        CancellationToken cancellationToken = default)
    {
        if (creditOperationId == Guid.Empty)
            throw new ArgumentException(
                "A operação de crédito é obrigatória.");

        using var response = await _httpClient.GetAsync(
            $"api/exposures/operation/{creditOperationId}",
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new KeyNotFoundException(
                "Nenhuma exposição foi encontrada para a operação.");

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<JsonElement>(
            cancellationToken: cancellationToken);
    }

    public async Task<JsonElement> AnalyzeExposureAsync(
        Guid creditLimitId,
        decimal additionalAmount,
        decimal categoryExposure,
        decimal totalPortfolioExposure,
        decimal maximumConcentrationPercentage,
        CancellationToken cancellationToken = default)
    {
        if (creditLimitId == Guid.Empty)
            throw new ArgumentException(
                "O limite de crédito é obrigatório.");

        if (additionalAmount < 0)
            throw new ArgumentException(
                "O valor adicional não pode ser negativo.");

        var request = new
        {
            CreditLimitId = creditLimitId,
            AdditionalAmount = additionalAmount,
            CategoryExposure = categoryExposure,
            TotalPortfolioExposure = totalPortfolioExposure,
            MaximumConcentrationPercentage = maximumConcentrationPercentage
        };

        using var response = await _httpClient.PostAsJsonAsync(
            "api/exposures/analyze",
            request,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<JsonElement>(
            cancellationToken: cancellationToken);
    }
}
