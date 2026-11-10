using CreditRisk.CreditAnalysis.Application.DTOs;
using CreditRisk.CreditAnalysis.Application.Services;
using CreditRisk.CreditAnalysis.Application.Validators;
using CreditRisk.CreditAnalysis.Infrastructure.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace CreditRisk.CreditAnalysis.Controllers;

[ApiController]
[Route("api/credit")]
public class CreditAnalysisController : ControllerBase
{
    private readonly ICreditAnalysisService _creditAnalysisService;
    private readonly CreditAnalysisValidator _validator;
    private readonly CreditAnalysisPublisher _publisher;

    public CreditAnalysisController(
        ICreditAnalysisService creditAnalysisService,
        CreditAnalysisValidator validator,
        CreditAnalysisPublisher publisher)
    {
        _creditAnalysisService = creditAnalysisService;
        _validator = validator;
        _publisher = publisher;
    }

    [HttpPost("analyze")]
    public async Task<ActionResult<CreditAnalysisResponse>> Analyze(
        [FromBody] CreditAnalysisRequest request,
        CancellationToken cancellationToken)
    {
        var errors = _validator.Validate(request);

        if (errors.Count > 0)
            return BadRequest(new ValidationProblemDetails(
                errors.ToDictionary(
                    x => x.Key,
                    x => new[] { x.Value }
                )
            ));

        var result = _creditAnalysisService.Analyze(request);

        await _publisher.PublishAsync(result, cancellationToken);

        return Ok(result);
    }
}