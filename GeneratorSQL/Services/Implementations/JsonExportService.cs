using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneratorSQL.Models;
using GeneratorSQL.Models.Enums;
using GeneratorSQL.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GeneratorSQL.Services.Implementations
{
    /// <summary>
    /// Service for exporting data in JSON format
    /// </summary>
    public class JsonExportService : IExportService
    {
        public ExportFormat Format => ExportFormat.JSON;

        public string Export(GenerationConfig config, List<Dictionary<string, object>> data)
        {
            if (data == null || data.Count == 0)
                return "[]";

            // Convert to JSON with nice formatting
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            return json;
        }

        /// <summary>
        /// Export with custom options
        /// </summary>
        public string ExportWithOptions(List<Dictionary<string, object>> data, bool indented = true, bool includeNulls = false)
        {
            if (data == null || data.Count == 0)
                return "[]";

            var settings = new JsonSerializerSettings
            {
                Formatting = indented ? Formatting.Indented : Formatting.None,
                NullValueHandling = includeNulls ? NullValueHandling.Include : NullValueHandling.Ignore
            };

            return JsonConvert.SerializeObject(data, settings);
        }
    }
}
