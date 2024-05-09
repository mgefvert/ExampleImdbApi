﻿namespace ImdbApi.Logging;

public static class ApiLoggingMiddlewareExtension
{
    public static void AddLoggingMiddleware(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ApiLogRequest>();
    }

    public static void UseLoggingMiddleware(this WebApplication hostBuilder)
    {
        hostBuilder.UseMiddleware<ApiLoggingMiddleware>();
    }
}
