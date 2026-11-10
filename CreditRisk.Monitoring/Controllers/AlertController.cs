using CreditRisk.Monitoring.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CreditRisk.Monitoring.Controllers;

[ApiController]
[Route("api/alerts")]
public class AlertController : ControllerBase
{
    private readonly IAlertService _alertService;

    public AlertController(IAlertService alertService)
    {
        _alertService = alertService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var alerts = await _alertService.GetAllAsync(
            cancellationToken);

        return Ok(alerts);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var alert = await _alertService.GetByIdAsync(
            id, cancellationToken);

        return alert is null ? NotFound() : Ok(alert);
    }

    [HttpGet("monitoring/{creditMonitoringId:guid}")]
    public async Task<IActionResult> GetByMonitoring(
        Guid creditMonitoringId,
        CancellationToken cancellationToken)
    {
        var alerts = await _alertService.GetByCreditMonitoringIdAsync(
            creditMonitoringId, cancellationToken);

        return Ok(alerts);
    }

    [HttpPatch("{id:guid}/acknowledge")]
    public async Task<IActionResult> Acknowledge(
        Guid id,
        CancellationToken cancellationToken)
    {
        var alert = await _alertService.AcknowledgeAsync(
            id, cancellationToken);

        return Ok(alert);
    }

    [HttpPatch("{id:guid}/resolve")]
    public async Task<IActionResult> Resolve(
        Guid id,
        CancellationToken cancellationToken)
    {
        var alert = await _alertService.ResolveAsync(
            id, cancellationToken);

        return Ok(alert);
    }
}