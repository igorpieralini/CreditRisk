using CreditRisk.Guarantee.Application.Services;
using CreditRisk.Guarantee.Application.Validators;
using CreditRisk.Guarantee.Domain.Calculations;
using CreditRisk.Guarantee.Domain.Repositories;
using CreditRisk.Guarantee.Domain.Rules;
using CreditRisk.Guarantee.Infrastructure.Messaging;
using CreditRisk.Guarantee.Infrastructure.Persistence;
using CreditRisk.Guarantee.Infrastructure.Persistence.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace CreditRisk.Guarantee.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IGuaranteeService, GuaranteeService>();
        services.AddScoped<IGuaranteeAnalysisService, GuaranteeAnalysisService>();
        services.AddScoped<IGuaranteeValuationService, GuaranteeValuationService>();
        services.AddScoped<IGuaranteeOperationService, GuaranteeOperationService>();

        services.AddScoped<IGuaranteeRepository, GuaranteeRepository>();
        services.AddScoped<IGuaranteeValuationRepository, GuaranteeValuationRepository>();
        services.AddScoped<IGuaranteeOperationRepository, GuaranteeOperationRepository>();

        services.AddScoped<LtvCalculator>();
        services.AddScoped<CoverageCalculator>();
        services.AddScoped<EligibleValueCalculator>();

        services.AddScoped<GuaranteeSufficiencyRule>();
        services.AddScoped<MaximumLtvRule>();

        services.AddScoped<GuaranteeValidator>();
        services.AddScoped<GuaranteeAnalysisPublisher>();

        var connectionString = configuration.GetConnectionString("CreditRisk");

        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            services.AddDbContext<CreditRiskDbContext>(options =>
                options.UseNpgsql(connectionString));
        }

        services.AddMassTransit(x =>
        {
            x.AddConsumer<GuaranteeAnalysisConsumer>();

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

                cfg.ReceiveEndpoint("guarantee-analysis", e =>
                {
                    e.ConfigureConsumer<GuaranteeAnalysisConsumer>(context);
                });
            });
        });

        return services;
    }
}