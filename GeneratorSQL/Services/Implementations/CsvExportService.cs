using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using GeneratorSQL.Models;
using GeneratorSQL.Models.Enums;
using GeneratorSQL.Services.Interfaces;

namespace GeneratorSQL.Services.Implementations
{
    /// <summary>
    /// Service for exporting data in CSV format
    /// </summary>
    public class CsvExportService : IExportService
    {
        public ExportFormat Format => ExportFormat.CSV;

        public string Export(GenerationConfig config, List<Dictionary<string, object>> data)
        {
            if (data == null || data.Count == 0)
                return string.Empty;

            using (var stringWriter = new StringWriter())
            using (var csv = new CsvWriter(stringWriter, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            }))
            {
                // Get column names from first row
                var columnNames = data[0].Keys.ToList();

                // Write header
                foreach (var column in columnNames)
                {
                    csv.WriteField(column);
                }
                csv.NextRecord();

                // Write data rows
                foreach (var row in data)
                {
                    foreach (var column in columnNames)
                    {
                        csv.WriteField(row[column]);
                    }
                    csv.NextRecord();
                }

                return stringWriter.ToString();
            }
        }

        /// <summary>
        /// Export with custom delimiter
        /// </summary>
        public string ExportWithDelimiter(List<Dictionary<string, object>> data, string delimiter = ",")
        {
            if (data == null || data.Count == 0)
                return string.Empty;

            using (var stringWriter = new StringWriter())
            using (var csv = new CsvWriter(stringWriter, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = delimiter,
                HasHeaderRecord = true,
            }))
            {
                var columnNames = data[0].Keys.ToList();

                foreach (var column in columnNames)
                {
                    csv.WriteField(column);
                }
                csv.NextRecord();

                foreach (var row in data)
                {
                    foreach (var column in columnNames)
                    {
                        csv.WriteField(row[column]);
                    }
                    csv.NextRecord();
                }

                return stringWriter.ToString();
            }
        }
    }
}
