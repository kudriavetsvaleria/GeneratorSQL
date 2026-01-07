using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneratorSQL.Models;
using GeneratorSQL.Models.Enums;
using GeneratorSQL.Services.Interfaces;

namespace GeneratorSQL.Services.Implementations
{
    /// <summary>
    /// Service for generating SQL INSERT statements with dialect support
    /// </summary>
    public class SqlExportService : ISqlExportService
    {
        public string GenerateSqlInsert(GenerationConfig config, List<Dictionary<string, object>> data)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            
            if (data == null || data.Count == 0)
                return string.Empty;

            if (string.IsNullOrWhiteSpace(config.TableName))
                throw new ArgumentException("Table name cannot be empty.", nameof(config));

            var sb = new StringBuilder();
            var quote = config.UseDoubleQuotes ? "\"" : "'";
            var separator = config.IncludeCommas ? ", " : " ";

            // Get column names from config or first row
            var columnNames = config.ColumnNames != null && config.ColumnNames.Count > 0
                ? config.ColumnNames
                : data[0].Keys.ToList();
            
            if (columnNames.Count == 0)
                throw new InvalidOperationException("No columns specified.");

            var formattedColumns = columnNames.Select(c => FormatColumnName(c, config.Dialect));

            // Generate INSERT INTO statement
            sb.AppendLine($"INSERT INTO {FormatColumnName(config.TableName, config.Dialect)} ({string.Join(", ", formattedColumns)}) VALUES");

            // Generate VALUES for each row
            for (int i = 0; i < data.Count; i++)
            {
                var row = data[i];
                var values = new List<string>();

                foreach (var columnName in columnNames)
                {
                    // Try to get value by column name, fallback to display name if not found
                    object value = null;
                    if (row.ContainsKey(columnName))
                    {
                        value = row[columnName];
                    }
                    else
                    {
                        // Try to find by matching key (case-insensitive)
                        var matchingKey = row.Keys.FirstOrDefault(k => 
                            string.Equals(k, columnName, StringComparison.OrdinalIgnoreCase));
                        if (matchingKey != null)
                        {
                            value = row[matchingKey];
                        }
                    }
                    
                    var formattedValue = FormatValue(value, quote, config.Dialect);
                    values.Add(formattedValue);
                }

                var isLastRow = i == data.Count - 1;
                var valueString = string.Join(separator, values);
                
                if (isLastRow)
                {
                    sb.Append($"({valueString});");
                }
                else
                {
                    sb.AppendLine($"({valueString}),");
                }
            }

            return sb.ToString();
        }

        public string EscapeString(string value, SqlDialect dialect)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            // Escape single quotes by doubling them (standard SQL)
            var escaped = value.Replace("'", "''");

            // Escape double quotes if needed
            escaped = escaped.Replace("\"", "\"\"");

            // Escape backslashes for MySQL
            if (dialect == SqlDialect.MySQL)
            {
                escaped = escaped.Replace("\\", "\\\\");
            }

            return escaped;
        }

        public string FormatColumnName(string name, SqlDialect dialect)
        {
            if (string.IsNullOrWhiteSpace(name))
                return name;

            return dialect switch
            {
                SqlDialect.MySQL => $"`{name}`",
                SqlDialect.PostgreSQL => $"\"{name}\"",
                SqlDialect.SQLServer => $"[{name}]",
                SqlDialect.Oracle => $"\"{name}\"",
                SqlDialect.SQLite => $"\"{name}\"",
                _ => name
            };
        }

        private string FormatValue(object value, string quote, SqlDialect dialect)
        {
            if (value == null)
                return "NULL";

            var stringValue = value.ToString();

            // Check if this is a numeric value
            if (int.TryParse(stringValue, out _) || decimal.TryParse(stringValue, out _))
            {
                return stringValue;
            }

            // Check if this is a boolean
            if (bool.TryParse(stringValue, out bool boolValue))
            {
                return boolValue ? "1" : "0";
            }

            // It's a string - escape and quote it
            var escaped = EscapeString(stringValue, dialect);
            return $"{quote}{escaped}{quote}";
        }
    }
}
