namespace GeneratorSQL.Services.Interfaces
{
    /// <summary>
    /// Service for clipboard operations
    /// </summary>
    public interface IClipboardService
    {
        /// <summary>
        /// Copies text to clipboard
        /// </summary>
        /// <param name="text">Text to copy</param>
        void SetText(string text);

        /// <summary>
        /// Gets text from clipboard
        /// </summary>
        /// <returns>Clipboard text</returns>
        string GetText();
    }
}
