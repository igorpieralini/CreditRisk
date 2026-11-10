
using CreditRisk.Monitoring.Application.DTOs;
using CreditRisk.Monitoring.Application.Services;
using CreditRisk.Monitoring.Application.Validators;
using CreditRisk.Monitoring.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CreditRisk.Monitoring.Controllers;

[ApiController]
[Route("api/monitorings")]
public class MonitoringController : ControllerBase
{
    private readonly IMonitoringService _monitoringService;
    private readonly IMonitoringEventRepository _eventRepository;
    private readonly MonitoringValidator _validator;

    public MonitoringController(
        IMonitoringService monitoringService,
        IMonitoringEventRepository eventRepository,
        MonitoringValidator validator)
    {
        _monitoringService = monitoringService;
        _eventRepository = eventRepository;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var monitorings = await _monitoringService.GetAllAsync(
            cancellationToken);

        return Ok(monitorings);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var monitoring = await _monitoringService.GetByIdAsync(
            id, cancellationToken);

        return monitoring is null ? NotFound() : Ok(monitoring);
    }

    [HttpGet("operation/{creditOperationId:guid}")]
    public async Task<IActionResult> GetByOperation(
        Guid creditOperationId,
        CancellationToken cancellationToken)
    {
        var monitoring = await _monitoringService.GetByCreditOperationIdAsync(
            creditOperationId, cancellationToken);

        return monitoring is null ? NotFound() : Ok(monitoring);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] MonitoringRequest request,
        CancellationToken cancellationToken)
    {
        var errors = _validator.Validate(request);

        if (errors.Count > 0)
            return BadRequest(errors);

        var monitoring = await _monitoringService.CreateAsync(
            request, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = monitoring.Id },
            monitoring);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] MonitoringRequest request,
        CancellationToken cancellationToken)
    {
        var errors = _validator.Validate(request);

        if (errors.Count > 0)
            return BadRequest(errors);

        var monitoring = await _monitoringService.UpdateAsync(
            id, request, cancellationToken);

        return Ok(monitoring);
    }

    [HttpGet("{id:guid}/events")]
    public async Task<IActionResult> GetEvents(
        Guid id,
        CancellationToken cancellationToken)
    {
        var monitoring = await _monitoringService.GetByIdAsync(
            id, cancellationToken);

        if (monitoring is null)
            return NotFound();

        var events = await _eventRepository.GetByCreditMonitoringIdAsync(
            id, cancellationToken);

        return Ok(events.Select(e => new
        {
            e.Id,
            e.CreditMonitoringId,
            e.Type,
            e.Description,
            e.CreatedAt
        }));
    }

    [HttpPost("events")]
    public async Task<IActionResult> RegisterEvent(
        [FromBody] MonitoringEventRequest request,
        CancellationToken cancellationToken)
    {
        var errors = _validator.ValidateEvent(request);

        if (errors.Count > 0)
            return BadRequest(errors);

        await _monitoringService.RegisterEventAsync(
            request, cancellationToken);

        return NoContent();
    }
}
