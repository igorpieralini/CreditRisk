using Microsoft.AspNetCore.Mvc;

namespace CreditRisk.CreditAnalysis.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação.");

            await WriteErrorAsync(
                context,
                StatusCodes.Status400BadRequest,
                "Dados inválidos",
                ex.Message
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno na análise de crédito.");

            await WriteErrorAsync(
                context,
                StatusCodes.Status500InternalServerError,
                "Erro interno",
                "Ocorreu um erro ao processar a solicitação."
            );
        }
    }

    private static async Task WriteErrorAsync(
        HttpContext context,
        int statusCode,
        string title,
        string detail)
    {
        context.Response.StatusCode = statusCode;

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        await context.Response.WriteAsJsonAsync(
            problem,
            options: null,
            contentType: "application/problem+json"
        );
    }
}