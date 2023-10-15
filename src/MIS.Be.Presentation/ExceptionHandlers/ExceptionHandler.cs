using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MIS.Be.Application.Models;

namespace MIS.Be.Presentation.ExceptionHandlers;

internal sealed class ExceptionHandler
{
    private readonly RequestDelegate _next;

    public ExceptionHandler(RequestDelegate next)
        => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(e, context);
        }
    }

    private static Task HandleExceptionAsync(Exception exception, HttpContext context)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = GetStatusCode(exception);

        var response = new Error(
            exception.GetType().Name,
            exception.Message
        );
        return context.Response.WriteAsJsonAsync(response);
    }

    private static int GetStatusCode(Exception exception)
    {
        var code = exception switch
        {
            ApplicationException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };

        return (int)code;
    }
}
