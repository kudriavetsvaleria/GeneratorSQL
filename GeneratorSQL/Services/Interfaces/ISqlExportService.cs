using GeneratorSQL.Models;
using GeneratorSQL.Models.Enums;
using System.Collections.Generic;

namespace GeneratorSQL.Services.Interfaces
{
    /// <summary>
    /// Service for exporting data in SQL format
    /// </summary>
    public interface ISqlExportService
    {
        /// <summary>
        /// Generates SQL INSERT statements from data
        /// </summary>
        /// <param name="config">Generation configuration</param>
        /// <param name="data">Row data to export</param>
        /// <returns>Generated SQL script</returns>
        string GenerateSqlInsert(GenerationConfig config, List<Dictionary<string, object>> data);

        /// <summary>
        /// Escapes a string value for SQL
        /// </summary>
        /// <param name="value">Value to escape</param>
        /// <param name="dialect">SQL dialect</param>
        /// <returns>Escaped string</returns>
        string EscapeString(string value, SqlDialect dialect);

        /// <summary>
        /// Formats a column name according to dialect conventions
        /// </summary>
        /// <param name="name">Column name</param>
        /// <param name="dialect">SQL dialect</param>
        /// <returns>Formatted column name</returns>
        string FormatColumnName(string name, SqlDialect dialect);
    }
}
