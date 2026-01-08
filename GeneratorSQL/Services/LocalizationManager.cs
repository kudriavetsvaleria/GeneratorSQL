using System.ComponentModel;
using System.Globalization;
using System.Resources;
using GeneratorSQL.Core;

namespace GeneratorSQL.Services
{
    /// <summary>
    /// Manages application localization and provides binding sources for UI
    /// </summary>
    public class LocalizationManager : ViewModelBase
    {
        private static readonly ResourceManager _resourceManager = 
            new ResourceManager("GeneratorSQL.Resources.Strings", typeof(LocalizationManager).Assembly);
            
        private CultureInfo _currentCulture = new CultureInfo("en");

        public static LocalizationManager Instance { get; } = new LocalizationManager();

        private LocalizationManager() { }

        public void SetCulture(string cultureCode)
        {
            _currentCulture = new CultureInfo(cultureCode);
            // Notify that all properties have changed
            OnPropertyChanged(string.Empty);
            OnPropertyChanged(nameof(CurrentCultureCode));
        }

        public string CurrentCultureCode => _currentCulture.Name;

        public string this[string key]
        {
            get
            {
                try
                {
                    return _resourceManager.GetString(key, _currentCulture) ?? $"[{key}]";
                }
                catch
                {
                    return $"[{key}]";
                }
            }
        }
    }
}
