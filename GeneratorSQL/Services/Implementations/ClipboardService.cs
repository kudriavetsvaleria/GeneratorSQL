using System.Windows;
using GeneratorSQL.Services.Interfaces;

namespace GeneratorSQL.Services.Implementations
{
    /// <summary>
    /// Service for clipboard operations
    /// </summary>
    public class ClipboardService : IClipboardService
    {
        public void SetText(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                Clipboard.SetText(text);
            }
        }

        public string GetText()
        {
            return Clipboard.ContainsText() ? Clipboard.GetText() : string.Empty;
        }
    }
}
