using Dapper;
using MySqlConnector;

namespace ImdbDataLoader;

public class ImdbDatabase
{
    private readonly MySqlConnection _connection;

    private static string Quote(string name) => "`" + name.Replace("`", "") + "`";

    public ImdbDatabase(MySqlConnection connection)
    {
        _connection = connection;
    }
    
    public async Task DropAllTables()
    {
        Console.WriteLine("Dropping all tables");
        var tables = (await _connection.QueryAsync<string>("show tables")).ToList();
        foreach (var table in tables)
            await _connection.ExecuteAsync($"drop table {Quote(table)}");
    }

    public async Task PostProcess()
    {
        Console.WriteLine("Post-processing");
        var sqls = (await File.ReadAllTextAsync("postprocess.sql"))
            .Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        foreach (var sql in sqls)
        {
            Console.WriteLine(sql);
            await _connection.ExecuteAsync(sql, commandTimeout: 900); // Five minutes
        }
    }

    public async Task RecreateTables()
    {
        Console.WriteLine("Recreating tables");
        var schema = (await File.ReadAllTextAsync("schema.sql"))
            .Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        foreach (var sql in schema)
            await _connection.ExecuteAsync(sql);
    }
}