using System.Text;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace mapper_refactor.Services;

public interface ICodeGenerationService
{
    Task<string> GenerateSchemaAsync(string api, string targetSchema = "migration");
    Task<string> GenerateAllSchemasAsync();
    Task<string> BuildMappingAsync(string mapping);
    Task<string> GenerateTableScriptAsync(string mapName);
    Task<string> GenerateViewScriptAsync(string mapName);
    Task<string> GenerateLoaderScriptAsync(string mapName);
}

public class CodeGenerationService : ICodeGenerationService
{
    private readonly ILogger<CodeGenerationService> _logger;
    private readonly IDatabaseService _databaseService;
    private string _currentSchema = string.Empty;
    private const string DefaultSchema = "migration";

    public CodeGenerationService(
        ILogger<CodeGenerationService> logger,
        IDatabaseService databaseService)
    {
        _logger = logger;
        _databaseService = databaseService;
    }

    public async Task<string> GenerateSchemaAsync(string api, string targetSchema = "migration")
    {
        try
        {
            _currentSchema = targetSchema;
            var result = new StringBuilder();
            var schemaBuilder = new StringBuilder();
            var enumBuilder = new StringBuilder();
            var foreignKeyBuilder = new StringBuilder();

            // Create schema
            result.AppendLine($"CREATE SCHEMA IF NOT EXISTS {_currentSchema};");
            _currentSchema = _currentSchema + ".";

            // Get all tables for the API
            var tables = await _databaseService.ExecuteQueryAsync(
                $"SELECT DISTINCT table_name FROM src_data WHERE api = '{api}' ORDER BY table_name");

            foreach (DataRow table in tables.Rows)
            {
                await GenerateTableDefinitionAsync(table["table_name"].ToString()!, api, schemaBuilder);
            }

            // Combine all parts
            result.AppendLine(enumBuilder.ToString());
            result.AppendLine(schemaBuilder.ToString());
            result.AppendLine(foreignKeyBuilder.ToString());

            return result.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating schema for API {Api}", api);
            throw;
        }
    }

    private async Task GenerateTableDefinitionAsync(string tableName, string api, StringBuilder builder)
    {
        if (string.IsNullOrEmpty(tableName)) return;

        var fields = await _databaseService.ExecuteQueryAsync(
            $@"SELECT DISTINCT 
                field_name, 
                comment, 
                field_type, 
                field_order, 
                table_comment 
               FROM src_data 
               WHERE api = '{api}' 
               AND table_name = '{tableName}' 
               ORDER BY field_order");

        if (fields.Rows.Count == 0) return;

        var tableNameLower = tableName.ToLower();
        var comments = new StringBuilder();

        builder.AppendLine($" -- start {tableNameLower}");

        // Add table comment
        var tableComment = fields.Rows[0]["table_comment"].ToString();
        comments.AppendLine($"COMMENT ON TABLE {_currentSchema}{tableNameLower} IS '{EscapeComment(tableComment)}';");

        // Create table
        builder.AppendLine($"CREATE TABLE IF NOT EXISTS {_currentSchema}{tableNameLower} (");
        builder.AppendLine("\t\tspid int4");

        // Add fields
        foreach (DataRow field in fields.Rows)
        {
            var fieldName = field["field_name"].ToString()!;
            var fieldNameLower = fieldName.ToLower();
            var fieldType = field["field_type"].ToString()!;
            var comment = field["comment"].ToString()!;
            var pgType = MapBaseType(fieldType);

            comments.AppendLine($"COMMENT ON COLUMN {_currentSchema}{tableNameLower}.{fieldNameLower} IS '{EscapeComment(comment)}';");

            var func = MapFunction(fieldName);
            if (string.IsNullOrEmpty(func) || func == "?")
            {
                var caseInfo = GenerateCase(fieldNameLower, comment);
                if (!string.IsNullOrEmpty(caseInfo))
                {
                    builder.AppendLine($"\t\t,{fieldNameLower} {pgType}");
                    builder.AppendLine($"\t\t,{fieldNameLower}_text text");
                }
                else
                {
                    builder.AppendLine($"\t\t,{fieldNameLower} {pgType}");
                }
            }
            else
            {
                builder.AppendLine($"\t\t,{fieldNameLower} {pgType}");
                builder.AppendLine($"\t\t,{fieldNameLower}_text text");
            }
        }

        builder.AppendLine(");");
        builder.AppendLine($" -- comments for {tableNameLower}");
        builder.AppendLine(comments.ToString());
        builder.AppendLine($" -- end {tableNameLower}");
        builder.AppendLine();
    }

    private static string MapBaseType(string sourceType) => sourceType.ToUpper() switch
    {
        "DSOPERDAY" => "text",
        "DSINT_KEY" => "text",
        "DSTINYINT" => "text",
        "DSDATETIME" => "text",
        "DSNUMBER20" => "text",
        "DSBRIEFNAME" => "text",
        "DSMONEY" => "text",
        "DSIDENTIFIER" => "text",
        "DSVARFULLNAME40" => "text",
        "DSFULLNAME" => "text",
        "DSCOMMENT" => "text",
        "DSSPID" => "text",
        _ => "text"
    };

    private static string MapFunction(string fieldName)
    {
        // TODO: Implement function mapping logic
        return string.Empty;
    }

    private static string GenerateCase(string fieldName, string comment)
    {
        // TODO: Implement case generation logic
        return string.Empty;
    }

    private static string EscapeComment(string comment)
    {
        return comment?.Replace("'", "''") ?? string.Empty;
    }

    public async Task<string> GenerateAllSchemasAsync()
    {
        try
        {
            var apis = await _databaseService.ExecuteQueryAsync(
                "SELECT DISTINCT api FROM src_data ORDER BY api");

            var result = new StringBuilder();
            foreach (DataRow api in apis.Rows)
            {
                var apiName = api["api"].ToString()!;
                result.AppendLine(await GenerateSchemaAsync(apiName));
            }

            return result.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating all schemas");
            throw;
        }
    }

    public async Task<string> BuildMappingAsync(string mapping)
    {
        try
        {
            // TODO: Implement mapping generation
            throw new NotImplementedException("Mapping generation not yet implemented");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error building mapping for {Mapping}", mapping);
            throw;
        }
    }

    public async Task<string> GenerateTableScriptAsync(string mapName)
    {
        try
        {
            var tableInfo = await _databaseService.ExecuteQueryAsync(
                $@"SELECT DISTINCT 
                    source_table,
                    dest_table,
                    dest_field,
                    field_type,
                    is_nullable,
                    max_length,
                    precision,
                    scale,
                    comment
                FROM map_data m
                JOIN dest_data d ON d.table_name = m.dest_table 
                    AND d.field_name = m.dest_field
                WHERE m.map_name = '{mapName}'
                ORDER BY d.field_order");

            if (tableInfo.Rows.Count == 0)
                return string.Empty;

            var sb = new StringBuilder();
            var currentTable = string.Empty;

            foreach (DataRow row in tableInfo.Rows)
            {
                var tableName = row["dest_table"].ToString()!.ToLower();
                
                if (tableName != currentTable)
                {
                    // Close previous table if exists
                    if (!string.IsNullOrEmpty(currentTable))
                    {
                        sb.AppendLine(");");
                        sb.AppendLine();
                    }

                    // Start new table
                    sb.AppendLine($"-- Table for {tableName}");
                    sb.AppendLine($"CREATE TABLE {DefaultSchema}.{tableName} (");
                    sb.AppendLine("    spid int4 NOT NULL,");
                    currentTable = tableName;
                }

                var fieldName = row["dest_field"].ToString()!;
                var fieldType = row["field_type"].ToString()!;
                var isNullable = Convert.ToBoolean(row["is_nullable"]);
                var maxLength = row["max_length"].ToString();
                var precision = row["precision"].ToString();
                var scale = row["scale"].ToString();
                var comment = row["comment"].ToString()!;

                var pgType = MapToPgType(fieldType, maxLength, precision, scale);
                var nullableStr = isNullable ? "NULL" : "NOT NULL";

                sb.AppendLine($"    {fieldName} {pgType} {nullableStr}, -- {comment}");
            }

            // Close last table
            sb.AppendLine(");");
            sb.AppendLine();

            // Add primary key
            sb.AppendLine($"ALTER TABLE {DefaultSchema}.{currentTable}");
            sb.AppendLine("    ADD CONSTRAINT pk_" + currentTable + " PRIMARY KEY (spid);");

            return sb.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating table script for mapping {MapName}", mapName);
            throw;
        }
    }

    public async Task<string> GenerateViewScriptAsync(string mapName)
    {
        try
        {
            var viewInfo = await _databaseService.ExecuteQueryAsync(
                $@"SELECT DISTINCT 
                    source_table,
                    source_field,
                    dest_field,
                    transformation
                FROM map_data
                WHERE map_name = '{mapName}'
                ORDER BY source_table, source_field");

            if (viewInfo.Rows.Count == 0)
                return string.Empty;

            var sb = new StringBuilder();
            var currentTable = string.Empty;

            foreach (DataRow row in viewInfo.Rows)
            {
                var tableName = row["source_table"].ToString()!.ToLower();
                
                if (tableName != currentTable)
                {
                    // Close previous view if exists
                    if (!string.IsNullOrEmpty(currentTable))
                    {
                        sb.AppendLine($"FROM {currentTable};");
                        sb.AppendLine();
                    }

                    // Start new view
                    sb.AppendLine($"-- View for {tableName}");
                    sb.AppendLine($"CREATE OR REPLACE VIEW v_{tableName}_source AS");
                    sb.AppendLine("SELECT");
                    sb.AppendLine("    spid,");
                    currentTable = tableName;
                }

                var sourceField = row["source_field"].ToString()!;
                var destField = row["dest_field"].ToString()!;
                var transformation = row["transformation"].ToString();

                var fieldExpression = string.IsNullOrEmpty(transformation)
                    ? sourceField
                    : $"({transformation}) as {destField}";

                sb.AppendLine($"    {fieldExpression},");
            }

            // Close last view
            if (!string.IsNullOrEmpty(currentTable))
            {
                // Remove last comma
                sb.Length -= 3;
                sb.AppendLine();
                sb.AppendLine($"FROM {currentTable};");
            }

            return sb.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating view script for mapping {MapName}", mapName);
            throw;
        }
    }

    public async Task<string> GenerateLoaderScriptAsync(string mapName)
    {
        try
        {
            var tables = await _databaseService.ExecuteQueryAsync(
                $@"SELECT DISTINCT source_table, dest_table
                   FROM map_data
                   WHERE map_name = '{mapName}'
                   ORDER BY source_table");

            if (tables.Rows.Count == 0)
                return string.Empty;

            var sb = new StringBuilder();

            foreach (DataRow row in tables.Rows)
            {
                var sourceTable = row["source_table"].ToString()!.ToLower();
                var destTable = row["dest_table"].ToString()!.ToLower();

                sb.AppendLine($"-- Loader for {sourceTable} -> {destTable}");
                sb.AppendLine($"INSERT INTO {DefaultSchema}.{destTable}");
                sb.AppendLine("(");
                sb.AppendLine("    spid,");

                // Get field mappings
                var fields = await _databaseService.ExecuteQueryAsync(
                    $@"SELECT source_field, dest_field
                       FROM map_data
                       WHERE map_name = '{mapName}'
                       AND source_table = '{sourceTable}'
                       ORDER BY dest_field");

                foreach (DataRow field in fields.Rows)
                {
                    var destField = field["dest_field"].ToString()!;
                    sb.AppendLine($"    {destField},");
                }

                // Remove last comma
                sb.Length -= 3;
                sb.AppendLine();
                sb.AppendLine(")");
                sb.AppendLine($"SELECT * FROM v_{sourceTable}_source;");
                sb.AppendLine();
            }

            return sb.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating loader script for mapping {MapName}", mapName);
            throw;
        }
    }

    private static string MapToPgType(string sourceType, string? maxLength, string? precision, string? scale)
    {
        maxLength = string.IsNullOrEmpty(maxLength) ? "0" : maxLength;
        precision = string.IsNullOrEmpty(precision) ? "0" : precision;
        scale = string.IsNullOrEmpty(scale) ? "0" : scale;

        return sourceType.ToUpper() switch
        {
            "VARCHAR" => $"varchar({maxLength})",
            "CHAR" => $"char({maxLength})",
            "DECIMAL" => $"numeric({precision},{scale})",
            "NUMERIC" => $"numeric({precision},{scale})",
            "INT" => "integer",
            "BIGINT" => "bigint",
            "SMALLINT" => "smallint",
            "DATETIME" => "timestamp",
            "DATE" => "date",
            "TIME" => "time",
            "BIT" => "boolean",
            "BINARY" => "bytea",
            "TEXT" => "text",
            _ => "text"
        };
    }
} 