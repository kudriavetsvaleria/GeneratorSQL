using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using GeneratorSQL.Models;
using GeneratorSQL.Models.Enums;
using GeneratorSQL.Services.Interfaces;

namespace GeneratorSQL.Services.Implementations
{
    /// <summary>
    /// Service for exporting data in XML format
    /// </summary>
    public class XmlExportService : IExportService
    {
        public ExportFormat Format => ExportFormat.XML;

        public string Export(GenerationConfig config, List<Dictionary<string, object>> data)
        {
            if (data == null || data.Count == 0)
                return "<Records></Records>";

            var rootName = config?.TableName ?? "Records";
            var root = new XElement(SanitizeXmlName(rootName));

            foreach (var row in data)
            {
                var recordElement = new XElement("Record");

                foreach (var kvp in row)
                {
                    var elementName = SanitizeXmlName(kvp.Key);
                    var element = new XElement(elementName, kvp.Value?.ToString() ?? string.Empty);
                    recordElement.Add(element);
                }

                root.Add(recordElement);
            }

            var document = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                root
            );

            using (var stringWriter = new System.IO.StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    OmitXmlDeclaration = false
                }))
                {
                    document.Save(xmlWriter);
                }
                return stringWriter.ToString();
            }
        }

        private string SanitizeXmlName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "Field";

            // Remove invalid XML name characters
            var sanitized = new string(name.Where(c => char.IsLetterOrDigit(c) || c == '_').ToArray());

            // XML names cannot start with a number
            if (sanitized.Length > 0 && char.IsDigit(sanitized[0]))
            {
                sanitized = "_" + sanitized;
            }

            return string.IsNullOrEmpty(sanitized) ? "Field" : sanitized;
        }
    }
}
