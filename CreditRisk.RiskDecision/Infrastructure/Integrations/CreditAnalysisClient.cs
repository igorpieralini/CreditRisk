using System.Net.Http.Json;
using System.Text.Json;
using CreditRisk.RiskDecision.Application.DTOs;

namespace CreditRisk.RiskDecision.Infrastructure.Integrations;

public class CreditAnalysisClient
{
    private readonly HttpClient _httpClient;

    public CreditAnalysisClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<JsonElement> GetAnalysisAsync(
        Guid creditOperationId,
        CancellationToken cancellationToken = default)
    {
        if (creditOperationId == Guid.Empty)
            throw new ArgumentException(
                "A operação de crédito é obrigatória.");

        using var response = await _httpClient.GetAsync(
            $"api/credit-analysis/operation/{creditOperationId}",
            cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(
                $"Erro ao consultar a análise de crédito. HTTP {(int)response.StatusCode}.");

        return await response.Content.ReadFromJsonAsync<JsonElement>(
            cancellationToken: cancellationToken);
    }
}