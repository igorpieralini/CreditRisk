
using CreditRisk.Guarantee.Application.DTOs;
using CreditRisk.Guarantee.Application.Services;
using CreditRisk.Guarantee.Application.Validators;
using CreditRisk.Guarantee.Infrastructure.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace CreditRisk.Guarantee.Controllers;

[ApiController]
[Route("api/guarantees")]
public class GuaranteeController : ControllerBase
{
    private readonly IGuaranteeService _guaranteeService;
    private readonly IGuaranteeAnalysisService _analysisService;
    private readonly IGuaranteeValuationService _valuationService;
    private readonly IGuaranteeOperationService _operationService;
    private readonly GuaranteeValidator _validator;
    private readonly GuaranteeAnalysisPublisher _publisher;

    public GuaranteeController(
        IGuaranteeService guaranteeService,
        IGuaranteeAnalysisService analysisService,
        IGuaranteeValuationService valuationService,
        IGuaranteeOperationService operationService,
        GuaranteeValidator validator,
        GuaranteeAnalysisPublisher publisher)
    {
        _guaranteeService = guaranteeService;
        _analysisService = analysisService;
        _valuationService = valuationService;
        _operationService = operationService;
        _validator = validator;
        _publisher = publisher;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GuaranteeResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var guarantees = await _guaranteeService.GetAllAsync(cancellationToken);

        return Ok(guarantees);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GuaranteeResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var guarantee = await _guaranteeService.GetByIdAsync(id, cancellationToken);

        if (guarantee is null)
            return NotFound();

        return Ok(guarantee);
    }

    [HttpPost]
    public async Task<ActionResult<GuaranteeResponse>> Create(
        [FromBody] GuaranteeRequest request,
        CancellationToken cancellationToken)
    {
        var errors = _validator.Validate(request);

        if (errors.Count > 0)
            return BadRequest(errors);

        var guarantee = await _guaranteeService.CreateAsync(
            request,
            cancellationToken
        );

        return CreatedAtAction(
            nameof(GetById),
            new { id = guarantee.Id },
            guarantee
        );
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _guaranteeService.DeleteAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpPost("analyze")]
    public async Task<ActionResult<GuaranteeAnalysisResponse>> Analyze(
        [FromBody] GuaranteeAnalysisRequest request,
        CancellationToken cancellationToken)
    {
        var errors = _validator.ValidateAnalysis(request);

        if (errors.Count > 0)
            return BadRequest(errors);

        var result = await _analysisService.AnalyzeAsync(request, cancellationToken);

        await _publisher.PublishAsync(result, cancellationToken);

        return Ok(result);
    }

    [HttpPost("valuations")]
    public async Task<ActionResult<GuaranteeValuationResponse>> CreateValuation(
        [FromBody] GuaranteeValuationRequest request,
        CancellationToken cancellationToken)
    {
        var valuation = await _valuationService.CreateAsync(
            request,
            cancellationToken
        );

        return CreatedAtAction(
            nameof(GetValuationById),
            new { id = valuation.Id },
            valuation
        );
    }

    [HttpGet("valuations/{id:guid}")]
    public async Task<ActionResult<GuaranteeValuationResponse>> GetValuationById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var valuation = await _valuationService.GetByIdAsync(id, cancellationToken);

        if (valuation is null)
            return NotFound();

        return Ok(valuation);
    }

    [HttpGet("{guaranteeId:guid}/valuations")]
    public async Task<ActionResult<IReadOnlyList<GuaranteeValuationResponse>>> GetValuationHistory(
        Guid guaranteeId,
        CancellationToken cancellationToken)
    {
        var history = await _valuationService.GetHistoryAsync(
            guaranteeId,
            cancellationToken
        );

        return Ok(history);
    }

    [HttpGet("{guaranteeId:guid}/valuations/latest")]
    public async Task<ActionResult<GuaranteeValuationResponse>> GetLatestValuation(
        Guid guaranteeId,
        CancellationToken cancellationToken)
    {
        var valuation = await _valuationService.GetLatestAsync(
            guaranteeId,
            cancellationToken
        );

        if (valuation is null)
            return NotFound();

        return Ok(valuation);
    }

    [HttpPost("operations")]
    public async Task<ActionResult<GuaranteeOperationResponse>> CreateOperation(
        [FromBody] GuaranteeOperationRequest request,
        CancellationToken cancellationToken)
    {
        var operation = await _operationService.CreateAsync(
            request,
            cancellationToken
        );

        return CreatedAtAction(
            nameof(GetOperationById),
            new { id = operation.Id },
            operation
        );
    }

    [HttpGet("operations/{id:guid}")]
    public async Task<ActionResult<GuaranteeOperationResponse>> GetOperationById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var operation = await _operationService.GetByIdAsync(
            id,
            cancellationToken
        );

        if (operation is null)
            return NotFound();

        return Ok(operation);
    }

    [HttpGet("{guaranteeId:guid}/operations")]
    public async Task<ActionResult<IReadOnlyList<GuaranteeOperationResponse>>> GetGuaranteeOperations(
        Guid guaranteeId,
        CancellationToken cancellationToken)
    {
        var operations = await _operationService.GetByGuaranteeIdAsync(
            guaranteeId,
            cancellationToken
        );

        return Ok(operations);
    }

    [HttpGet("operations/credit/{creditOperationId:guid}")]
    public async Task<ActionResult<IReadOnlyList<GuaranteeOperationResponse>>> GetCreditOperations(
        Guid creditOperationId,
        CancellationToken cancellationToken)
    {
        var operations = await _operationService.GetByCreditOperationIdAsync(
            creditOperationId,
            cancellationToken
        );

        return Ok(operations);
    }

    [HttpPatch("operations/{id:guid}/release")]
    public async Task<IActionResult> ReleaseOperation(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _operationService.ReleaseAsync(id, cancellationToken);

        return NoContent();
    }
}
