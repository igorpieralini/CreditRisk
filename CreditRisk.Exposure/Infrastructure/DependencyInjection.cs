
using Exposure.Application.Services;
using Exposure.Application.Validators;
using Exposure.Domain.Calculations;
using Exposure.Domain.Repositories;
using Exposure.Domain.Rules;
using Exposure.Infrastructure.Messaging;
using Exposure.Infrastructure.Persistence;
using Exposure.Infrastructure.Persistence.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Exposure.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "A conexão com o banco de dados não foi configurada.");

        services.AddDbContext<CreditRiskDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ICreditExposureRepository, CreditExposureRepository>();
        services.AddScoped<ICreditLimitRepository, CreditLimitRepository>();
        services.AddScoped<IExposureMovementRepository, ExposureMovementRepository>();

        services.AddScoped<IExposureService, ExposureService>();
        services.AddScoped<IExposureAnalysisService, ExposureAnalysisService>();
        services.AddScoped<ICreditLimitService, CreditLimitService>();

        services.AddScoped<ExposureValidator>();
        services.AddScoped<CreditLimitValidator>();

        services.AddScoped<ExposureCalculator>();
        services.AddScoped<LimitUtilizationCalculator>();
        services.AddScoped<ConcentrationCalculator>();

        services.AddScoped<CreditLimitRule>();
        services.AddScoped<ExposureConcentrationRule>();

        services.AddScoped<ExposureAnalysisPublisher>();

        services.AddMassTransit(bus =>
        {
            bus.AddConsumer<ExposureAnalysisConsumer>();

            bus.UsingRabbitMq((context, cfg) =>
            {
                var host = configuration["RabbitMQ:Host"]
                    ?? throw new InvalidOperationException(
                        "O host do RabbitMQ não foi configurado.");

                var username = configuration["RabbitMQ:Username"]
                    ?? throw new InvalidOperationException(
                        "O usuário do RabbitMQ não foi configurado.");

                var password = configuration["RabbitMQ:Password"]
                    ?? throw new InvalidOperationException(
                        "A senha do RabbitMQ não foi configurada.");

                cfg.Host(host, "/", h =>
                {
                    h.Username(username);
                    h.Password(password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}