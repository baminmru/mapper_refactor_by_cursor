using System.Text;
using System.Data;
using Microsoft.Extensions.Logging;

namespace mapper_refactor.Services;

public class ViewGenerationService : IViewGenerationService
{
    private readonly IDatabaseService _databaseService;
    private readonly ILogger<ViewGenerationService> _logger;

    public ViewGenerationService(
        IDatabaseService databaseService,
        ILogger<ViewGenerationService> logger)
    {
        _databaseService = databaseService;
        _logger = logger;
    }

    public async Task<(string Views, string LoaderScript)> GenerateViewsAsync(string apiName)
    {
        try
        {
            var tables = await _databaseService.ExecuteQueryAsync(
                $@"SELECT DISTINCT table_name 
                   FROM src_data 
                   WHERE api_name = '{apiName}'
                   ORDER BY table_name");

            var views = new StringBuilder();
            var loaderScript = new StringBuilder();

            foreach (DataRow row in tables.Rows)
            {
                var tableName = row["table_name"].ToString();
                var (viewSql, loaderSql) = await GenerateViewForTableAsync(apiName, tableName!);
                views.AppendLine(viewSql);
                views.AppendLine("GO");
                views.AppendLine();
                loaderScript.AppendLine(loaderSql);
                loaderScript.AppendLine("GO");
                loaderScript.AppendLine();
            }

            return (views.ToString(), loaderScript.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating views for API {ApiName}", apiName);
            throw;
        }
    }

    public async Task<(string Views, string LoaderScript)> GenerateAllViewsAsync()
    {
        try
        {
            var apis = await _databaseService.ExecuteQueryAsync(
                "SELECT DISTINCT api_name FROM src_data ORDER BY api_name");

            var views = new StringBuilder();
            var loaderScript = new StringBuilder();

            foreach (DataRow row in apis.Rows)
            {
                var apiName = row["api_name"].ToString();
                var (viewSql, loaderSql) = await GenerateViewsAsync(apiName!);
                views.Append(viewSql);
                loaderScript.Append(loaderSql);
            }

            return (views.ToString(), loaderScript.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating all views");
            throw;
        }
    }

    private async Task<(string ViewSql, string LoaderSql)> GenerateViewForTableAsync(string apiName, string tableName)
    {
        var fields = await _databaseService.ExecuteQueryAsync(
            $@"SELECT field_name, field_type, max_length, precision, scale
               FROM src_data
               WHERE api_name = '{apiName}'
               AND table_name = '{tableName}'
               ORDER BY field_name");

        var viewSql = new StringBuilder();
        viewSql.AppendLine($"CREATE OR ALTER VIEW v_{apiName}_{tableName} AS");
        viewSql.AppendLine("SELECT");

        var fieldList = new List<string>();
        foreach (DataRow row in fields.Rows)
        {
            var fieldName = row["field_name"].ToString();
            var fieldType = row["field_type"].ToString();
            var maxLength = row["max_length"].ToString();
            var precision = row["precision"].ToString();
            var scale = row["scale"].ToString();

            fieldList.Add($"    {fieldName} {MapFieldType(fieldType!, maxLength, precision, scale)}");
        }

        viewSql.AppendLine(string.Join(",\n", fieldList));
        viewSql.AppendLine($"FROM {tableName}");

        var loaderSql = new StringBuilder();
        loaderSql.AppendLine($"INSERT INTO {tableName} (");
        loaderSql.AppendLine(string.Join(",\n", fields.Rows.Cast<DataRow>().Select(r => $"    {r["field_name"]}")));
        loaderSql.AppendLine(")");
        loaderSql.AppendLine($"SELECT * FROM v_{apiName}_{tableName}");

        return (viewSql.ToString(), loaderSql.ToString());
    }

    private static string MapFieldType(string sourceType, string? maxLength, string? precision, string? scale)
    {
        maxLength = string.IsNullOrEmpty(maxLength) ? "0" : maxLength;
        precision = string.IsNullOrEmpty(precision) ? "0" : precision;
        scale = string.IsNullOrEmpty(scale) ? "0" : scale;

        return sourceType.ToUpper() switch
        {
            "VARCHAR" => $"varchar({maxLength})",
            "CHAR" => $"char({maxLength})",
            "NVARCHAR" => $"varchar({maxLength})",
            "NCHAR" => $"char({maxLength})",
            "TEXT" => "text",
            "NTEXT" => "text",
            "INT" => "integer",
            "BIGINT" => "bigint",
            "SMALLINT" => "smallint",
            "TINYINT" => "smallint",
            "BIT" => "boolean",
            "DECIMAL" => $"decimal({precision},{scale})",
            "NUMERIC" => $"numeric({precision},{scale})",
            "MONEY" => "money",
            "SMALLMONEY" => "money",
            "FLOAT" => "double precision",
            "REAL" => "real",
            "DATETIME" => "timestamp",
            "DATETIME2" => "timestamp",
            "SMALLDATETIME" => "timestamp",
            "DATE" => "date",
            "TIME" => "time",
            "UNIQUEIDENTIFIER" => "uuid",
            "BINARY" => "bytea",
            "VARBINARY" => "bytea",
            "IMAGE" => "bytea",
            _ => "text" // Default to text for unknown types
        };
    }

    public async Task<string> GenerateViewsForMappingAsync(string mappingName)
    {
        try
        {
            var mappings = await _databaseService.ExecuteQueryAsync(
                $@"SELECT 
                    source_table,
                    source_field,
                    dest_table,
                    dest_field,
                    transformation
                FROM map_data
                WHERE map_name = '{mappingName}'
                ORDER BY source_table, source_field");

            var views = new StringBuilder();
            var currentSourceTable = string.Empty;
            var currentFields = new List<string>();

            foreach (DataRow row in mappings.Rows)
            {
                var sourceTable = row["source_table"].ToString()!;
                var sourceField = row["source_field"].ToString()!;
                var destTable = row["dest_table"].ToString()!;
                var destField = row["dest_field"].ToString()!;
                var transform = row["transformation"].ToString();

                if (sourceTable != currentSourceTable)
                {
                    if (currentFields.Count > 0)
                    {
                        GenerateView(views, currentSourceTable, currentFields);
                        currentFields.Clear();
                    }
                    currentSourceTable = sourceTable;
                }

                var fieldMapping = string.IsNullOrEmpty(transform)
                    ? $"    {sourceField} AS {destField}"
                    : $"    {transform} AS {destField}";

                currentFields.Add(fieldMapping);
            }

            if (currentFields.Count > 0)
            {
                GenerateView(views, currentSourceTable, currentFields);
            }

            return views.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating views for mapping {MappingName}", mappingName);
            throw;
        }
    }

    private static void GenerateView(StringBuilder views, string sourceTable, List<string> fields)
    {
        views.AppendLine($"CREATE OR ALTER VIEW v_{sourceTable} AS");
        views.AppendLine("SELECT");
        views.AppendLine(string.Join(",\n", fields));
        views.AppendLine($"FROM {sourceTable};");
        views.AppendLine("GO");
        views.AppendLine();
    }
} 