using System;

namespace GeneratorSQL.Services.Interfaces
{
    /// <summary>
    /// Controls application theme switching.
    /// </summary>
    public interface IThemeService
    {
        /// <summary>
        /// Currently applied theme key (e.g., Light, Dark).
        /// </summary>
        string CurrentTheme { get; }

        /// <summary>
        /// Apply the specified theme.
        /// </summary>
        /// <param name="themeKey">Theme identifier.</param>
        void SetTheme(string themeKey);
    }
}

