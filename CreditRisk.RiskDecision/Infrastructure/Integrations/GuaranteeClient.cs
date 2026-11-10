using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace CreditRisk.RiskDecision.Infrastructure.Integrations;

public class GuaranteeClient
{
    private readonly HttpClient _httpClient;

    public GuaranteeClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<JsonElement> GetGuaranteesByOperationAsync(
        Guid creditOperationId,
        CancellationToken cancellationToken = default)
    {
        if (creditOperationId == Guid.Empty)
            throw new ArgumentException(
                "A operação de crédito é obrigatória.");

        using var response = await _httpClient.GetAsync(
            $"api/garantias/operacao/{creditOperationId}",
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new KeyNotFoundException(
                "Nenhuma garantia foi encontrada para a operação.");

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<JsonElement>(
            cancellationToken: cancellationToken);
    }
}