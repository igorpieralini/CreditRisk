
using CreditRisk.RiskDecision.Application.DTOs;
using CreditRisk.RiskDecision.Application.Services;
using CreditRisk.RiskDecision.Application.Validators;
using Microsoft.AspNetCore.Mvc;

namespace CreditRisk.RiskDecision.Controllers;

[ApiController]
[Route("api/risk-decisions")]
public class RiskDecisionController : ControllerBase
{
    private readonly IRiskDecisionService _decisionService;
    private readonly RiskDecisionValidator _validator;

    public RiskDecisionController(
        IRiskDecisionService decisionService,
        RiskDecisionValidator validator)
    {
        _decisionService = decisionService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var decisions = await _decisionService.GetAllAsync(
            cancellationToken);

        return Ok(decisions);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var decision = await _decisionService.GetByIdAsync(
            id, cancellationToken);

        return decision is null ? NotFound() : Ok(decision);
    }

    [HttpGet("operation/{creditOperationId:guid}")]
    public async Task<IActionResult> GetByOperation(
        Guid creditOperationId,
        CancellationToken cancellationToken)
    {
        var decisions = await _decisionService.GetByCreditOperationIdAsync(
            creditOperationId, cancellationToken);

        return Ok(decisions);
    }

    [HttpGet("{id:guid}/history")]
    public async Task<IActionResult> GetHistory(
        Guid id,
        CancellationToken cancellationToken)
    {
        var history = await _decisionService.GetHistoryAsync(
            id, cancellationToken);

        return Ok(history);
    }

    [HttpPost("evaluate")]
    public async Task<IActionResult> Evaluate(
        [FromBody] RiskDecisionRequest request,
        CancellationToken cancellationToken)
    {
        var errors = _validator.Validate(request);

        if (errors.Count > 0)
            return BadRequest(errors);

        var decision = await _decisionService.EvaluateAsync(
            request, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = decision.Id },
            decision);
    }
}
