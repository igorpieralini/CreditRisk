
using CreditRisk.RiskDecision.Application.DTOs;
using CreditRisk.RiskDecision.Domain.Entities;
using CreditRisk.RiskDecision.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CreditRisk.RiskDecision.Controllers;

[ApiController]
[Route("api/decision-policies")]
public class DecisionPolicyController : ControllerBase
{
    private readonly IDecisionPolicyRepository _policyRepository;

    public DecisionPolicyController(
        IDecisionPolicyRepository policyRepository)
    {
        _policyRepository = policyRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var policies = await _policyRepository.GetAllAsync(
            cancellationToken);

        return Ok(policies);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var policy = await _policyRepository.GetByIdAsync(
            id, cancellationToken);

        return policy is null ? NotFound() : Ok(policy);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] DecisionPolicyRequest request,
        CancellationToken cancellationToken)
    {
        if (request is null)
            return BadRequest("Os dados da política são obrigatórios.");

        var existingPolicy = await _policyRepository.GetActiveAsync(
            cancellationToken);

        if (existingPolicy is not null)
            return Conflict(
                "Já existe uma política ativa. Desative-a antes de cadastrar outra.");

        var policy = new DecisionPolicy(
            request.Name,
            request.MinimumRiskScore,
            request.MinimumGuaranteeCoverage,
            request.MaximumLimitUtilization
        );

        await _policyRepository.AddAsync(policy, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = policy.Id },
            policy);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] DecisionPolicyRequest request,
        CancellationToken cancellationToken)
    {
        if (request is null)
            return BadRequest("Os dados da política são obrigatórios.");

        var policy = await _policyRepository.GetByIdAsync(
            id, cancellationToken);

        if (policy is null)
            return NotFound();

        policy.Update(
            request.Name,
            request.MinimumRiskScore,
            request.MinimumGuaranteeCoverage,
            request.MaximumLimitUtilization
        );

        await _policyRepository.UpdateAsync(
            policy, cancellationToken);

        return Ok(policy);
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> Activate(
        Guid id,
        CancellationToken cancellationToken)
    {
        var policy = await _policyRepository.GetByIdAsync(
            id, cancellationToken);

        if (policy is null)
            return NotFound();

        var activePolicy = await _policyRepository.GetActiveAsync(
            cancellationToken);

        if (activePolicy is not null)
            return Conflict(
                "Já existe uma política ativa.");

        policy.Activate();

        await _policyRepository.UpdateAsync(
            policy, cancellationToken);

        return NoContent();
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(
        Guid id,
        CancellationToken cancellationToken)
    {
        var policy = await _policyRepository.GetByIdAsync(
            id, cancellationToken);

        if (policy is null)
            return NotFound();

        policy.Deactivate();

        await _policyRepository.UpdateAsync(
            policy, cancellationToken);

        return NoContent();
    }
}