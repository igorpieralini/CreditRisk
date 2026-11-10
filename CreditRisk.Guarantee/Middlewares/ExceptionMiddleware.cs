
using Microsoft.AspNetCore.Mvc;

namespace CreditRisk.Guarantee.Middlewares;

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
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Recurso não encontrado.");

            await WriteErrorAsync(
                context,
                StatusCodes.Status404NotFound,
                "Não encontrado",
                ex.Message
            );
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Dados inválidos.");

            await WriteErrorAsync(
                context,
                StatusCodes.Status400BadRequest,
                "Dados inválidos",
                ex.Message
            );
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Operação inválida.");

            await WriteErrorAsync(
                context,
                StatusCodes.Status409Conflict,
                "Conflito na operação",
                ex.Message
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno no microsserviço de garantias.");

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
