using Exposure.Application.DTOs;
using Exposure.Application.Services;
using Exposure.Domain.Entities;
using Exposure.Domain.Repositories;
using Exposure.Domain.Rules;
using Moq;

namespace CreditRisk.Tests.Exposure.Services;

public class ExposureServiceTests
{
    private readonly Mock<ICreditExposureRepository> _exposureRepository;
    private readonly Mock<ICreditLimitRepository> _limitRepository;
    private readonly Mock<IExposureMovementRepository> _movementRepository;
    private readonly ExposureService _service;

    public ExposureServiceTests()
    {
        _exposureRepository = new Mock<ICreditExposureRepository>();
        _limitRepository = new Mock<ICreditLimitRepository>();
        _movementRepository = new Mock<IExposureMovementRepository>();

        _service = new ExposureService(
            _exposureRepository.Object,
            _limitRepository.Object,
            _movementRepository.Object,
            new CreditLimitRule()
        );
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateExposure_WhenLimitIsAvailable()
    {
        var limit = new CreditLimit("Limite Corporativo", 100000m);

        var request = new ExposureRequest
        {
            CreditOperationId = Guid.NewGuid(),
            CreditLimitId = limit.Id,
            Amount = 25000m
        };

        _limitRepository
            .Setup(r => r.GetByIdAsync(limit.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(limit);

        _exposureRepository
            .Setup(r => r.GetByCreditLimitIdAsync(
                limit.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CreditExposure>());

        var result = await _service.CreateAsync(request);

        Assert.Equal(25000m, result.OriginalAmount);
        Assert.Equal(25000m, result.OutstandingAmount);
        Assert.Equal(limit.Id, result.CreditLimitId);

        _exposureRepository.Verify(
            r => r.AddAsync(
                It.IsAny<CreditExposure>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _movementRepository.Verify(
            r => r.AddAsync(
                It.IsAny<ExposureMovement>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenLimitIsExceeded()
    {
        var limit = new CreditLimit("Limite Corporativo", 100000m);

        var existingExposure = new CreditExposure(
            Guid.NewGuid(),
            limit.Id,
            90000m
        );

        var request = new ExposureRequest
        {
            CreditOperationId = Guid.NewGuid(),
            CreditLimitId = limit.Id,
            Amount = 20000m
        };

        _limitRepository
            .Setup(r => r.GetByIdAsync(limit.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(limit);

        _exposureRepository
            .Setup(r => r.GetByCreditLimitIdAsync(
                limit.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CreditExposure> { existingExposure });

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateAsync(request));

        _exposureRepository.Verify(
            r => r.AddAsync(
                It.IsAny<CreditExposure>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenOperationAlreadyExists()
    {
        var operationId = Guid.NewGuid();
        var limit = new CreditLimit("Limite Corporativo", 100000m);

        var existingExposure = new CreditExposure(
            operationId,
            limit.Id,
            25000m
        );

        var request = new ExposureRequest
        {
            CreditOperationId = operationId,
            CreditLimitId = limit.Id,
            Amount = 10000m
        };

        _exposureRepository
            .Setup(r => r.GetByCreditOperationIdAsync(
                operationId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingExposure);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateAsync(request));
    }

    [Fact]
    public async Task IncreaseAsync_ShouldIncreaseOutstandingAmount()
    {
        var limit = new CreditLimit("Limite Corporativo", 100000m);

        var exposure = new CreditExposure(
            Guid.NewGuid(),
            limit.Id,
            25000m
        );

        _exposureRepository
            .Setup(r => r.GetByIdAsync(
                exposure.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(exposure);

        _limitRepository
            .Setup(r => r.GetByIdAsync(
                limit.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(limit);

        _exposureRepository
            .Setup(r => r.GetByCreditLimitIdAsync(
                limit.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CreditExposure> { exposure });

        var result = await _service.IncreaseAsync(exposure.Id, 10000m);

        Assert.Equal(35000m, result.OutstandingAmount);

        _exposureRepository.Verify(
            r => r.UpdateAsync(
                It.IsAny<CreditExposure>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _movementRepository.Verify(
            r => r.AddAsync(
                It.IsAny<ExposureMovement>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task ReduceAsync_ShouldReduceOutstandingAmount()
    {
        var exposure = new CreditExposure(
            Guid.NewGuid(),
            Guid.NewGuid(),
            40000m
        );

        _exposureRepository
            .Setup(r => r.GetByIdAsync(
                exposure.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(exposure);

        var result = await _service.ReduceAsync(exposure.Id, 5000m);

        Assert.Equal(35000m, result.OutstandingAmount);

        _exposureRepository.Verify(
            r => r.UpdateAsync(
                It.IsAny<CreditExposure>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task ReduceAsync_ShouldThrowException_WhenAmountExceedsBalance()
    {
        var exposure = new CreditExposure(
            Guid.NewGuid(),
            Guid.NewGuid(),
            25000m
        );

        _exposureRepository
            .Setup(r => r.GetByIdAsync(
                exposure.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(exposure);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.ReduceAsync(exposure.Id, 30000m));
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenExposureDoesNotExist()
    {
        var result = await _service.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }
}
