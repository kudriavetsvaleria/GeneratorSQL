using GeneratorSQL.Models;
using GeneratorSQL.Models.Enums;
using System.Collections.Generic;

namespace GeneratorSQL.Services.Interfaces
{
    /// <summary>
    /// Base interface for all export services
    /// </summary>
    public interface IExportService
    {
        /// <summary>
        /// Export format this service handles
        /// </summary>
        ExportFormat Format { get; }

        /// <summary>
        /// Exports data in the specific format
        /// </summary>
        /// <param name="config">Generation configuration</param>
        /// <param name="data">Row data to export</param>
        /// <returns>Exported string content</returns>
        string Export(GenerationConfig config, List<Dictionary<string, object>> data);
    }
}
