using System.Data;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace mapper_refactor.Services;

public interface IDatabaseService
{
    Task SetConnectionStringAsync(string connectionString);
    Task TestConnectionAsync();
    Task<DataTable> ExecuteQueryAsync(string query);
    Task<int> ExecuteNonQueryAsync(string query);
}

public class DatabaseService : IDatabaseService
{
    private readonly ILogger<DatabaseService> _logger;
    private string _connectionString;

    public DatabaseService(string connectionString, ILogger<DatabaseService> logger)
    {
        _connectionString = connectionString;
        _logger = logger;
    }

    public Task SetConnectionStringAsync(string connectionString)
    {
        _connectionString = connectionString;
        return Task.CompletedTask;
    }

    public async Task TestConnectionAsync()
    {
        try
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            _logger.LogInformation("Successfully connected to database");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to connect to database");
            throw;
        }
    }

    public async Task<DataTable> ExecuteQueryAsync(string query)
    {
        try
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            var dataTable = new DataTable();
            dataTable.Load(reader);

            return dataTable;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing query: {Query}", query);
            throw;
        }
    }

    public async Task<int> ExecuteNonQueryAsync(string query)
    {
        try
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new NpgsqlCommand(query, conn);
            return await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing non-query: {Query}", query);
            throw;
        }
    }
} 