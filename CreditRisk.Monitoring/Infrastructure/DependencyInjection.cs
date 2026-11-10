
using CreditRisk.Monitoring.Application.Services;
using CreditRisk.Monitoring.Application.Validators;
using CreditRisk.Monitoring.Domain.Repositories;
using CreditRisk.Monitoring.Domain.Rules;
using CreditRisk.Monitoring.Infrastructure.Messaging;
using CreditRisk.Monitoring.Infrastructure.Persistence;
using CreditRisk.Monitoring.Infrastructure.Persistence.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace CreditRisk.Monitoring.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException(
                "A conexão com o banco de dados não foi configurada.");

        services.AddDbContext<MonitoringDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<
            ICreditMonitoringRepository,
            CreditMonitoringRepository>();

        services.AddScoped<
            IRiskAlertRepository,
            RiskAlertRepository>();

        services.AddScoped<
            IMonitoringEventRepository,
            MonitoringEventRepository>();

        services.AddScoped<IMonitoringService, MonitoringService>();
        services.AddScoped<IAlertService, AlertService>();

        services.AddScoped<MonitoringValidator>();

        services.AddScoped<RiskScoreAlertRule>();
        services.AddScoped<ExposureAlertRule>();
        services.AddScoped<GuaranteeAlertRule>();

        services.AddScoped<RiskAlertPublisher>();

        services.AddMassTransit(bus =>
        {
            bus.AddConsumer<MonitoringEventConsumer>();

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
