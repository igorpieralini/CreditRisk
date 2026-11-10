
using Exposure.Application.DTOs;
using Exposure.Application.Services;
using Exposure.Application.Validators;
using Microsoft.AspNetCore.Mvc;

namespace Exposure.Controllers;

[ApiController]
[Route("api/exposures")]
public class ExposureController : ControllerBase
{
    private readonly IExposureService _exposureService;
    private readonly IExposureAnalysisService _analysisService;
    private readonly ExposureValidator _validator;

    public ExposureController(
        IExposureService exposureService,
        IExposureAnalysisService analysisService,
        ExposureValidator validator)
    {
        _exposureService = exposureService;
        _analysisService = analysisService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var exposures = await _exposureService.GetAllAsync(cancellationToken);

        return Ok(exposures);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var exposure = await _exposureService.GetByIdAsync(
            id, cancellationToken);

        return exposure is null ? NotFound() : Ok(exposure);
    }

    [HttpGet("operation/{creditOperationId:guid}")]
    public async Task<IActionResult> GetByOperation(
        Guid creditOperationId,
        CancellationToken cancellationToken)
    {
        var exposure = await _exposureService.GetByCreditOperationIdAsync(
            creditOperationId, cancellationToken);

        return exposure is null ? NotFound() : Ok(exposure);
    }

    [HttpGet("limit/{creditLimitId:guid}")]
    public async Task<IActionResult> GetByLimit(
        Guid creditLimitId,
        CancellationToken cancellationToken)
    {
        var exposures = await _exposureService.GetByCreditLimitIdAsync(
            creditLimitId, cancellationToken);

        return Ok(exposures);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] ExposureRequest request,
        CancellationToken cancellationToken)
    {
        var errors = _validator.Validate(request);

        if (errors.Count > 0)
            return BadRequest(errors);

        var exposure = await _exposureService.CreateAsync(
            request, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = exposure.Id },
            exposure);
    }

    [HttpPatch("{id:guid}/increase")]
    public async Task<IActionResult> Increase(
        Guid id,
        [FromBody] decimal amount,
        CancellationToken cancellationToken)
    {
        if (amount <= 0)
            return BadRequest("O valor deve ser maior que zero.");

        var exposure = await _exposureService.IncreaseAsync(
            id, amount, cancellationToken);

        return Ok(exposure);
    }

    [HttpPatch("{id:guid}/reduce")]
    public async Task<IActionResult> Reduce(
        Guid id,
        [FromBody] decimal amount,
        CancellationToken cancellationToken)
    {
        if (amount <= 0)
            return BadRequest("O valor deve ser maior que zero.");

        var exposure = await _exposureService.ReduceAsync(
            id, amount, cancellationToken);

        return Ok(exposure);
    }

    [HttpPost("analyze")]
    public async Task<IActionResult> Analyze(
        [FromBody] ExposureAnalysisRequest request,
        CancellationToken cancellationToken)
    {
        var errors = _validator.ValidateAnalysis(request);

        if (errors.Count > 0)
            return BadRequest(errors);

        var result = await _analysisService.AnalyzeAsync(
            request, cancellationToken);

        return Ok(result);
    }
}