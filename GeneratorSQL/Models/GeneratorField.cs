using GeneratorSQL.Core;
using GeneratorSQL.Models.Enums;

namespace GeneratorSQL.Models
{
    /// <summary>
    /// Represents a single data field that can be generated
    /// </summary>
    public class GeneratorField : ViewModelBase
    {
        private string _displayName;
        private bool _isSelected;
        private int _order;
        private string _resourceKey;

        /// <summary>
        /// Resource key for localization
        /// </summary>
        public string ResourceKey
        {
            get => _resourceKey;
            set
            {
                if (SetProperty(ref _resourceKey, value))
                {
                    OnPropertyChanged(nameof(DisplayName));
                }
            }
        }

        /// <summary>
        /// User-friendly display name for the field
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(ResourceKey))
                {
                    // Use the centralized LocalizationManager to look up the string
                    return GeneratorSQL.Services.LocalizationManager.Instance[ResourceKey];
                }
                return _displayName;
            }
            set => SetProperty(ref _displayName, value);
        }

        /// <summary>
        /// Type of data this field generates
        /// </summary>
        public FieldType Type { get; set; }

        /// <summary>
        /// Whether this field is selected for generation
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        /// <summary>
        /// Order in which this field appears in the output (0-based)
        /// </summary>
        public int Order
        {
            get => _order;
            set => SetProperty(ref _order, value);
        }

        /// <summary>
        /// Category this field belongs to for UI grouping
        /// </summary>
        public FieldCategory Category { get; set; }

        /// <summary>
        /// Optional custom configuration for this field (e.g., min/max values, format strings)
        /// </summary>
        public object CustomConfig { get; set; }

        public GeneratorField()
        {
            Order = -1; // Not ordered yet
            SubscribeToLocalization();
        }

        private void SubscribeToLocalization()
        {
            // Re-raise DisplayName change when culture changes
            GeneratorSQL.Services.LocalizationManager.Instance.PropertyChanged += (s, e) =>
            {
                if (string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == "Item[]")
                {
                    OnPropertyChanged(nameof(DisplayName));
                }
            };
        }

        public GeneratorField(string displayName, FieldType type, FieldCategory category)
        {
            DisplayName = displayName;
            Type = type;
            Category = category;
            IsSelected = false;
            Order = -1;
            SubscribeToLocalization();
        }
    }
}
