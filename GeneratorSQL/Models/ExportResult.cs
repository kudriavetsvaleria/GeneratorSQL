using System;

namespace GeneratorSQL.Models
{
    /// <summary>
    /// Result of a data generation/export operation
    /// </summary>
    public class ExportResult
    {
        /// <summary>
        /// Generated output content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Whether the operation was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Error message if operation failed
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Number of rows generated
        /// </summary>
        public int RowCount { get; set; }

        /// <summary>
        /// Time taken to generate (in milliseconds)
        /// </summary>
        public long ElapsedMilliseconds { get; set; }

        /// <summary>
        /// Size of generated content in bytes
        /// </summary>
        public long SizeBytes { get; set; }

        public ExportResult()
        {
            Success = true;
        }

        public static ExportResult Failed(string errorMessage)
        {
            return new ExportResult
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }

        public static ExportResult Successful(string content, int rowCount, long elapsedMs)
        {
            return new ExportResult
            {
                Success = true,
                Content = content,
                RowCount = rowCount,
                ElapsedMilliseconds = elapsedMs,
                SizeBytes = System.Text.Encoding.UTF8.GetByteCount(content)
            };
        }
    }
}
