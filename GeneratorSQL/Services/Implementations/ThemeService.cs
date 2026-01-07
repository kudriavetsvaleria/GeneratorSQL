using System;
using System.Linq;
using System.Windows;
using GeneratorSQL.Services.Interfaces;

namespace GeneratorSQL.Services.Implementations
{
    /// <summary>
    /// Simple theme service that swaps ResourceDictionaries.
    /// </summary>
    public class ThemeService : IThemeService
    {
        private const string LightResourcePath = "Themes/Light.xaml";
        private const string DarkResourcePath = "Themes/Dark.xaml";

        public string CurrentTheme { get; private set; } = "Light";

        public void SetTheme(string themeKey)
        {
            var app = Application.Current;
            if (app == null) return;

            var normalized = string.IsNullOrWhiteSpace(themeKey) ? "Light" : themeKey;
            var targetPath = normalized.Equals("Dark", StringComparison.OrdinalIgnoreCase)
                ? DarkResourcePath
                : LightResourcePath;

            // Remove existing theme dictionary (light or dark)
            var existing = app.Resources.MergedDictionaries.FirstOrDefault(md =>
                md.Source != null &&
                (md.Source.OriginalString.EndsWith("Light.xaml", StringComparison.OrdinalIgnoreCase) ||
                 md.Source.OriginalString.EndsWith("Dark.xaml", StringComparison.OrdinalIgnoreCase)));

            if (existing != null)
            {
                app.Resources.MergedDictionaries.Remove(existing);
            }

            // Add the new theme dictionary at the beginning so it overrides
            var newDict = new ResourceDictionary { Source = new Uri(targetPath, UriKind.Relative) };
            app.Resources.MergedDictionaries.Insert(0, newDict);

            CurrentTheme = normalized.Equals("Dark", StringComparison.OrdinalIgnoreCase) ? "Dark" : "Light";
        }
    }
}

