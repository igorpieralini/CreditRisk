
using CreditRisk.RiskDecision.Application.Services;
using CreditRisk.RiskDecision.Application.Validators;
using CreditRisk.RiskDecision.Domain.Calculations;
using CreditRisk.RiskDecision.Domain.Repositories;
using CreditRisk.RiskDecision.Domain.Rules;
using CreditRisk.RiskDecision.Infrastructure.Integrations;
using CreditRisk.RiskDecision.Infrastructure.Messaging;
using CreditRisk.RiskDecision.Infrastructure.Persistence;
using CreditRisk.RiskDecision.Infrastructure.Persistence.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace CreditRisk.RiskDecision.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "A conexão com o banco de dados não foi configurada.");

        services.AddDbContext<RiskDecisionDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ICreditDecisionRepository, CreditDecisionRepository>();
        services.AddScoped<IDecisionHistoryRepository, DecisionHistoryRepository>();
        services.AddScoped<IDecisionPolicyRepository, DecisionPolicyRepository>();

        services.AddScoped<IRiskDecisionService, RiskDecisionService>();
        services.AddScoped<IRiskEvaluationService, RiskEvaluationService>();

        services.AddScoped<RiskDecisionValidator>();

        services.AddScoped<RiskScoreCalculator>();
        services.AddScoped<CreditScoreRule>();
        services.AddScoped<GuaranteeCoverageRule>();
        services.AddScoped<ExposureLimitRule>();

        services.AddScoped<RiskDecisionPublisher>();

        services.AddHttpClient<CreditAnalysisClient>(client =>
        {
            client.BaseAddress = new Uri(
                configuration["Services:CreditAnalysis"]
                ?? "http://localhost:5207/");
        });

        services.AddHttpClient<GuaranteeClient>(client =>
        {
            client.BaseAddress = new Uri(
                configuration["Services:Guarantee"]
                ?? "http://localhost:5100/");
        });

        services.AddHttpClient<ExposureClient>(client =>
        {
            client.BaseAddress = new Uri(
                configuration["Services:Exposure"]
                ?? "http://localhost:5242/");
        });

        services.AddMassTransit(bus =>
        {
            bus.AddConsumer<RiskDecisionConsumer>();

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