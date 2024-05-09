namespace ImdbData.Services;

public static class Helper
{
    public const int DefaultMaxLimit = 100;

    public static IQueryable<T> AddLimiter<T>(IQueryable<T> query, int? start, int? limit, int maxLimit = DefaultMaxLimit)
    {
        var queryStart = start ?? 0;
        var queryLimit = limit ?? maxLimit;
        if (queryLimit > maxLimit)
            queryLimit = maxLimit;

        return query.Skip(queryStart).Take(queryLimit);
    }
}