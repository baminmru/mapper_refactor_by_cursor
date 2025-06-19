using System.Text;
using System.Data;
using Microsoft.Extensions.Logging;

namespace mapper_refactor.Services;

public interface IMigrationMappingService
{
    Task<(string SourceViews, string DestinationTables, string MappingScript)> GenerateMappingAsync(string api);
    Task<(string SourceViews, string DestinationTables, string MappingScript)> GenerateAllMappingsAsync();
}

public class MigrationMappingService : IMigrationMappingService
{
    private readonly ILogger<MigrationMappingService> _logger;
    private readonly IDatabaseService _databaseService;
    private string _currentSchema = "migration";

    public MigrationMappingService(
        ILogger<MigrationMappingService> logger,
        IDatabaseService databaseService)
    {
        _logger = logger;
        _databaseService = databaseService;
    }

    public async Task<(string SourceViews, string DestinationTables, string MappingScript)> GenerateMappingAsync(string api)
    {
        try
        {
            var sourceViews = new StringBuilder();
            var destinationTables = new StringBuilder();
            var mappingScript = new StringBuilder();

            // Get all tables for the API
            var tables = await _databaseService.ExecuteQueryAsync(
                $@"SELECT DISTINCT s.table_name as source_table, d.table_name as dest_table 
                   FROM src_data s 
                   JOIN dest_data d ON s.api = d.api AND s.table_name = d.table_name 
                   WHERE s.api = '{api}' 
                   ORDER BY s.table_name");

            foreach (DataRow table in tables.Rows)
            {
                var sourceTable = table["source_table"].ToString()!;
                var destTable = table["dest_table"].ToString()!;

                await GenerateSourceViewAsync(sourceTable, api, sourceViews);
                await GenerateDestinationTableAsync(destTable, api, destinationTables);
                await GenerateMappingScriptAsync(sourceTable, destTable, api, mappingScript);
            }

            return (sourceViews.ToString(), destinationTables.ToString(), mappingScript.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating mapping for API {Api}", api);
            throw;
        }
    }

    private async Task GenerateSourceViewAsync(string tableName, string api, StringBuilder builder)
    {
        var fields = await _databaseService.ExecuteQueryAsync(
            $@"SELECT DISTINCT 
                field_name, 
                field_type,
                comment,
                field_order 
               FROM src_data 
               WHERE api = '{api}' 
               AND table_name = '{tableName}' 
               ORDER BY field_order");

        var tableNameLower = tableName.ToLower();
        builder.AppendLine($"-- Source view for {tableNameLower}");
        builder.AppendLine($"CREATE OR ALTER VIEW v_{tableNameLower}_source AS");
        builder.AppendLine("SELECT");
        builder.AppendLine("\tspid");

        foreach (DataRow field in fields.Rows)
        {
            var fieldName = field["field_name"].ToString()!;
            var fieldType = field["field_type"].ToString()!;
            var comment = field["comment"].ToString()!;

            builder.AppendLine($"\t,{fieldName} -- {comment}");
        }

        builder.AppendLine($"FROM {tableNameLower};");
        builder.AppendLine("GO");
        builder.AppendLine();
    }

    private async Task GenerateDestinationTableAsync(string tableName, string api, StringBuilder builder)
    {
        var fields = await _databaseService.ExecuteQueryAsync(
            $@"SELECT DISTINCT 
                field_name, 
                field_type,
                comment,
                field_order,
                is_nullable,
                max_length,
                precision,
                scale 
               FROM dest_data 
               WHERE api = '{api}' 
               AND table_name = '{tableName}' 
               ORDER BY field_order");

        var tableNameLower = tableName.ToLower();
        builder.AppendLine($"-- Destination table for {tableNameLower}");
        builder.AppendLine($"CREATE TABLE {_currentSchema}.{tableNameLower} (");
        builder.AppendLine("\tspid int4 NOT NULL");

        foreach (DataRow field in fields.Rows)
        {
            var fieldName = field["field_name"].ToString()!;
            var fieldType = field["field_type"].ToString()!;
            var comment = field["comment"].ToString()!;
            var isNullable = Convert.ToBoolean(field["is_nullable"]);
            var maxLength = field["max_length"].ToString();
            var precision = field["precision"].ToString();
            var scale = field["scale"].ToString();

            var pgType = MapDestinationType(fieldType, maxLength, precision, scale);
            var nullableStr = isNullable ? "NULL" : "NOT NULL";

            builder.AppendLine($"\t,{fieldName} {pgType} {nullableStr} -- {comment}");
        }

        builder.AppendLine(");");
        builder.AppendLine();

        // Add comments
        builder.AppendLine($"COMMENT ON TABLE {_currentSchema}.{tableNameLower} IS 'Migration table for {tableName}';");
        foreach (DataRow field in fields.Rows)
        {
            var fieldName = field["field_name"].ToString()!;
            var comment = field["comment"].ToString()!.Replace("'", "''");
            builder.AppendLine($"COMMENT ON COLUMN {_currentSchema}.{tableNameLower}.{fieldName} IS '{comment}';");
        }
        builder.AppendLine();
    }

    private async Task GenerateMappingScriptAsync(string sourceTable, string destTable, string api, StringBuilder builder)
    {
        var mappings = await _databaseService.ExecuteQueryAsync(
            $@"SELECT 
                s.field_name as source_field,
                d.field_name as dest_field,
                d.field_type as dest_type,
                s.field_type as source_type
               FROM src_data s
               JOIN dest_data d ON s.api = d.api 
                AND s.table_name = d.table_name 
                AND s.field_name = d.field_name
               WHERE s.api = '{api}'
               AND s.table_name = '{sourceTable}'
               ORDER BY s.field_order");

        var tableNameLower = sourceTable.ToLower();
        builder.AppendLine($"-- Data migration script for {tableNameLower}");
        builder.AppendLine($"INSERT INTO {_currentSchema}.{tableNameLower}");
        builder.AppendLine("(");
        builder.AppendLine("\tspid");

        // Destination fields
        foreach (DataRow mapping in mappings.Rows)
        {
            var destField = mapping["dest_field"].ToString()!;
            builder.AppendLine($"\t,{destField}");
        }

        builder.AppendLine(")");
        builder.AppendLine("SELECT");
        builder.AppendLine("\tspid");

        // Source fields with type casting where needed
        foreach (DataRow mapping in mappings.Rows)
        {
            var sourceField = mapping["source_field"].ToString()!;
            var destType = mapping["dest_type"].ToString()!;
            var sourceType = mapping["source_type"].ToString()!;

            var castExpression = GenerateTypeCastExpression(sourceField, sourceType, destType);
            builder.AppendLine($"\t,{castExpression}");
        }

        builder.AppendLine($"FROM v_{tableNameLower}_source;");
        builder.AppendLine("GO");
        builder.AppendLine();
    }

    private static string MapDestinationType(string sourceType, string? maxLength, string? precision, string? scale)
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

    private static string GenerateTypeCastExpression(string field, string sourceType, string destType)
    {
        // If source and destination types are the same, no cast needed
        if (sourceType.Equals(destType, StringComparison.OrdinalIgnoreCase))
        {
            return field;
        }

        // Handle specific type conversions
        return (sourceType.ToUpper(), destType.ToUpper()) switch
        {
            ("VARCHAR", "INT") => $"NULLIF({field}, '')::integer",
            ("VARCHAR", "DECIMAL") => $"NULLIF({field}, '')::decimal",
            ("VARCHAR", "DATE") => $"NULLIF({field}, '')::date",
            ("VARCHAR", "TIMESTAMP") => $"NULLIF({field}, '')::timestamp",
            ("INT", "VARCHAR") => $"{field}::varchar",
            ("DECIMAL", "VARCHAR") => $"{field}::varchar",
            ("DATE", "VARCHAR") => $"{field}::varchar",
            ("TIMESTAMP", "VARCHAR") => $"{field}::varchar",
            _ => $"{field}::{destType.ToLower()}" // Default cast
        };
    }

    public async Task<(string SourceViews, string DestinationTables, string MappingScript)> GenerateAllMappingsAsync()
    {
        try
        {
            var sourceViews = new StringBuilder();
            var destinationTables = new StringBuilder();
            var mappingScript = new StringBuilder();

            // Get all distinct APIs
            var apis = await _databaseService.ExecuteQueryAsync(
                "SELECT DISTINCT api FROM src_data ORDER BY api");

            foreach (DataRow apiRow in apis.Rows)
            {
                var api = apiRow["api"].ToString()!;
                var (apiSourceViews, apiDestTables, apiMappingScript) = await GenerateMappingAsync(api);

                sourceViews.AppendLine(apiSourceViews);
                destinationTables.AppendLine(apiDestTables);
                mappingScript.AppendLine(apiMappingScript);
            }

            return (sourceViews.ToString(), destinationTables.ToString(), mappingScript.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating all mappings");
            throw;
        }
    }
} 