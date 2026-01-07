using System.Collections.Generic;
using GeneratorSQL.Models;
using GeneratorSQL.Models.Enums;

namespace GeneratorSQL.Services.Interfaces
{
    /// <summary>
    /// Service for generating random test data
    /// </summary>
    public interface IDataGeneratorService
    {
        /// <summary>
        /// Generates a random value for the specified field type
        /// </summary>
        /// <param name="fieldType">Type of field to generate</param>
        /// <param name="locale">Locale for data generation (e.g., "en", "ru", "uk")</param>
        /// <returns>Generated value as string</returns>
        string GenerateValue(FieldType fieldType, string locale = "en");

        /// <summary>
        /// Sets the random seed for reproducible results
        /// </summary>
        /// <param name="seed">Random seed value</param>
        void SetSeed(int seed);

        /// <summary>
        /// Generates multiple rows of data
        /// </summary>
        /// <param name="fields">Fields to generate</param>
        /// <param name="count">Number of rows</param>
        /// <param name="locale">Locale for data generation</param>
        /// <returns>List of row data</returns>
        List<Dictionary<string, object>> GenerateRows(List<GeneratorField> fields, int count, string locale = "en");
    }
}
