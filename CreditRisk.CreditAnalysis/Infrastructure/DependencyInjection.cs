using CreditRisk.CreditAnalysis.Application.Services;
using CreditRisk.CreditAnalysis.Application.Validators;
using CreditRisk.CreditAnalysis.Domain.Calculations;
using CreditRisk.CreditAnalysis.Domain.Rules;
using CreditRisk.CreditAnalysis.Infrastructure.Messaging;
using MassTransit;

namespace CreditRisk.CreditAnalysis.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ICreditAnalysisService, CreditAnalysisService>();

        services.AddScoped<InterestCalculator>();
        services.AddScoped<InstallmentCalculator>();
        services.AddScoped<TotalCostCalculator>();

        services.AddScoped<CreditLimitRule>();
        services.AddScoped<PaymentTermRule>();

        services.AddScoped<CreditAnalysisValidator>();
        services.AddScoped<CreditAnalysisPublisher>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<CreditAnalysisConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(
                    configuration["RabbitMQ:Host"] ?? "localhost",
                    "/",
                    h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"] ?? "guest");
                        h.Password(configuration["RabbitMQ:Password"] ?? "guest");
                    });

                cfg.ReceiveEndpoint("credit-analysis", e =>
                {
                    e.ConfigureConsumer<CreditAnalysisConsumer>(context);
                });
            });
        });

        return services;
    }
}