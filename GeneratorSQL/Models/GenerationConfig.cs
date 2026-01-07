using System.Collections.Generic;
using GeneratorSQL.Models.Enums;

namespace GeneratorSQL.Models
{
    /// <summary>
    /// Configuration for data generation and export
    /// </summary>
    public class GenerationConfig
    {
        /// <summary>
        /// Name of the database table (for SQL export)
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Column names (for SQL export)
        /// </summary>
        public List<string> ColumnNames { get; set; }

        /// <summary>
        /// Number of rows to generate
        /// </summary>
        public int RowCount { get; set; }

        /// <summary>
        /// SQL dialect to use for generation
        /// </summary>
        public SqlDialect Dialect { get; set; }

        /// <summary>
        /// Whether to use double quotes (") instead of single quotes (')
        /// </summary>
        public bool UseDoubleQuotes { get; set; }

        /// <summary>
        /// Whether to include commas between values
        /// </summary>
        public bool IncludeCommas { get; set; }

        /// <summary>
        /// Export format
        /// </summary>
        public ExportFormat ExportFormat { get; set; }

        /// <summary>
        /// Locale for data generation (e.g., "en", "ru", "uk")
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// Seed for random number generator (for reproducible results)
        /// </summary>
        public int? Seed { get; set; }

        public GenerationConfig()
        {
            ColumnNames = new List<string>();
            RowCount = 10;
            Dialect = SqlDialect.MySQL;
            ExportFormat = ExportFormat.SQL;
            IncludeCommas = true;
            UseDoubleQuotes = false;
            Locale = "en";
        }
    }
}
