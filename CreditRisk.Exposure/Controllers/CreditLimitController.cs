
using Exposure.Application.DTOs;
using Exposure.Application.Services;
using Exposure.Application.Validators;
using Microsoft.AspNetCore.Mvc;

namespace Exposure.Controllers;

[ApiController]
[Route("api/credit-limits")]
public class CreditLimitController : ControllerBase
{
    private readonly ICreditLimitService _creditLimitService;
    private readonly CreditLimitValidator _validator;

    public CreditLimitController(
        ICreditLimitService creditLimitService,
        CreditLimitValidator validator)
    {
        _creditLimitService = creditLimitService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var limits = await _creditLimitService.GetAllAsync(
            cancellationToken);

        return Ok(limits);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var limit = await _creditLimitService.GetByIdAsync(
            id, cancellationToken);

        return limit is null ? NotFound() : Ok(limit);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreditLimitRequest request,
        CancellationToken cancellationToken)
    {
        var errors = _validator.Validate(request);

        if (errors.Count > 0)
            return BadRequest(errors);

        var limit = await _creditLimitService.CreateAsync(
            request, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = limit.Id },
            limit);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] CreditLimitRequest request,
        CancellationToken cancellationToken)
    {
        var errors = _validator.Validate(request);

        if (errors.Count > 0)
            return BadRequest(errors);

        var limit = await _creditLimitService.UpdateAsync(
            id, request, cancellationToken);

        return Ok(limit);
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _creditLimitService.DeactivateAsync(
            id, cancellationToken);

        return NoContent();
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> Activate(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _creditLimitService.ActivateAsync(
            id, cancellationToken);

        return NoContent();
    }
}