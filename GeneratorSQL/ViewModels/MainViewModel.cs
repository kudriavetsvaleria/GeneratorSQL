using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using GeneratorSQL.Core;
using GeneratorSQL.Models;
using GeneratorSQL.Models.Enums;
using GeneratorSQL.Services;
using GeneratorSQL.Services.Interfaces;

namespace GeneratorSQL.ViewModels
{
    /// <summary>
    /// Main ViewModel for the application
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataGeneratorService _dataGenerator;
        private readonly ISqlExportService _sqlExporter;
        private readonly IClipboardService _clipboard;
        private readonly IThemeService _themeService;

        private string _tableName;
        private string _columns;
        private int _rowCount;
        private string _generatedOutput;
        private bool _isGenerating;
        private bool _useDoubleQuotes;
        private bool _includeCommas;
        private SqlDialect _selectedDialect;
        private string _statusMessage;
        private long _lastGenerationTimeMs;
        private string _currentLocale;
        private string _fieldSearchText;
        private bool _isDarkTheme;

        private ICollectionView _fieldsView;

        public MainViewModel(
            IDataGeneratorService dataGenerator,
            ISqlExportService sqlExporter,
            IClipboardService clipboard,
            IThemeService themeService)
        {
            _dataGenerator = dataGenerator ?? throw new ArgumentNullException(nameof(dataGenerator));
            _sqlExporter = sqlExporter ?? throw new ArgumentNullException(nameof(sqlExporter));
            _clipboard = clipboard ?? throw new ArgumentNullException(nameof(clipboard));
            _themeService = themeService ?? throw new ArgumentNullException(nameof(themeService));

            // Initialize properties
            _tableName = string.Empty;
            _columns = string.Empty;
            _rowCount = 10;
            _useDoubleQuotes = false;
            _includeCommas = true;
            _selectedDialect = SqlDialect.MySQL;
            _currentLocale = "en"; // Default to English
            _statusMessage = "Ready";
            _isDarkTheme = false;

            // Initialize fields collection
            InitializeFields();

            // Initialize commands
            GenerateCommand = new AsyncRelayCommand(GenerateAsync, CanGenerate);
            CopyToClipboardCommand = new RelayCommand(CopyToClipboard, CanCopyToClipboard);
            ClearCommand = new RelayCommand(Clear);
        }

        #region Properties

        public string TableName
        {
            get => _tableName;
            set
            {
                if (SetProperty(ref _tableName, value))
                {
                    GenerateCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Columns
        {
            get => _columns;
            set
            {
                if (SetProperty(ref _columns, value))
                {
                    GenerateCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public int RowCount
        {
            get => _rowCount;
            set
            {
                if (SetProperty(ref _rowCount, value))
                {
                    GenerateCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string GeneratedOutput
        {
            get => _generatedOutput;
            set
            {
                if (SetProperty(ref _generatedOutput, value))
                {
                    CopyToClipboardCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public bool IsGenerating
        {
            get => _isGenerating;
            set => SetProperty(ref _isGenerating, value);
        }

        public bool UseDoubleQuotes
        {
            get => _useDoubleQuotes;
            set => SetProperty(ref _useDoubleQuotes, value);
        }

        public bool IncludeCommas
        {
            get => _includeCommas;
            set => SetProperty(ref _includeCommas, value);
        }

        public SqlDialect SelectedDialect
        {
            get => _selectedDialect;
            set => SetProperty(ref _selectedDialect, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public long LastGenerationTimeMs
        {
            get => _lastGenerationTimeMs;
            set => SetProperty(ref _lastGenerationTimeMs, value);
        }

        public ObservableCollection<GeneratorField> AvailableFields { get; private set; }

        public ICollectionView FieldsView => _fieldsView;

        public Array SqlDialects => Enum.GetValues(typeof(SqlDialect));

        public string CurrentLocale
        {
            get => _currentLocale;
            set
            {
                if (SetProperty(ref _currentLocale, value))
                {
                    // Update UI language
                    LocalizationManager.Instance.SetCulture(value);
                    GenerateCommand.RaiseCanExecuteChanged();
                    
                    // Update columns text to reflect new language
                    UpdateColumnsText();
                }
            }
        }

        public LocalizationManager Strings => LocalizationManager.Instance;

        public string FieldSearchText
        {
            get => _fieldSearchText;
            set
            {
                if (SetProperty(ref _fieldSearchText, value))
                {
                    _fieldsView?.Refresh();
                }
            }
        }

        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set
            {
                if (SetProperty(ref _isDarkTheme, value))
                {
                    _themeService.SetTheme(value ? "Dark" : "Light");
                }
            }
        }

        #endregion

        #region Commands

        public AsyncRelayCommand GenerateCommand { get; }
        public RelayCommand CopyToClipboardCommand { get; }
        public RelayCommand ClearCommand { get; }

        #endregion

        #region Command Methods

        private bool CanGenerate()
        {
            return !IsGenerating &&
                   !string.IsNullOrWhiteSpace(TableName) &&
                   !string.IsNullOrWhiteSpace(Columns) &&
                   RowCount > 0 &&
                   AvailableFields.Any(f => f.IsSelected);
        }

        private async System.Threading.Tasks.Task GenerateAsync()
        {
            try
            {
                IsGenerating = true;
                StatusMessage = "Generating...";

                var sw = Stopwatch.StartNew();

                // Get selected fields in order
                var selectedFields = AvailableFields
                    .Where(f => f.IsSelected)
                    .OrderBy(f => f.Order)
                    .ToList();

                // Get column names directly from selected fields (since input is read-only/auto-filled)
                var columnNames = selectedFields.Select(f => f.DisplayName).ToList();

                // Generate data with selected locale
                var data = await System.Threading.Tasks.Task.Run(() =>
                    _dataGenerator.GenerateRows(selectedFields, RowCount, CurrentLocale));

                // Create config
                var config = new GenerationConfig
                {
                    TableName = TableName,
                    ColumnNames = columnNames,
                    RowCount = RowCount,
                    Dialect = SelectedDialect,
                    UseDoubleQuotes = UseDoubleQuotes,
                    IncludeCommas = IncludeCommas
                };

                // Generate SQL
                var sql = await System.Threading.Tasks.Task.Run(() =>
                    _sqlExporter.GenerateSqlInsert(config, data));

                GeneratedOutput = sql;

                sw.Stop();
                LastGenerationTimeMs = sw.ElapsedMilliseconds;
                StatusMessage = $"Generated {RowCount} rows in {LastGenerationTimeMs}ms";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
                MessageBox.Show($"Generation failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsGenerating = false;
            }
        }

        private bool CanCopyToClipboard()
        {
            return !string.IsNullOrWhiteSpace(GeneratedOutput);
        }

        private void CopyToClipboard()
        {
            try
            {
                _clipboard.SetText(GeneratedOutput);
                StatusMessage = "Copied to clipboard!";
                MessageBox.Show("SQL text copied to clipboard!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to copy: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Clear()
        {
            GeneratedOutput = string.Empty;
            StatusMessage = "Ready";
        }

        #endregion

        #region Helper Methods

        private void InitializeFields()
        {
            AvailableFields = new ObservableCollection<GeneratorField>
            {
                // Identifiers
                new GeneratorField("ID", FieldType.ID, FieldCategory.Identifiers) { ResourceKey = "Field_ID" },
                new GeneratorField("GUID", FieldType.GUID, FieldCategory.Identifiers) { ResourceKey = "Field_GUID" },

                // Personal
                new GeneratorField("First Name", FieldType.FirstName, FieldCategory.Personal) { ResourceKey = "Field_FirstName" },
                new GeneratorField("Last Name", FieldType.LastName, FieldCategory.Personal) { ResourceKey = "Field_LastName" },
                new GeneratorField("Middle Name", FieldType.MiddleName, FieldCategory.Personal) { ResourceKey = "Field_MiddleName" },
                new GeneratorField("Full Name", FieldType.FullName, FieldCategory.Personal) { ResourceKey = "Field_FullName" },
                new GeneratorField("Gender", FieldType.Gender, FieldCategory.Personal) { ResourceKey = "Field_Gender" },
                new GeneratorField("Age", FieldType.Age, FieldCategory.Personal) { ResourceKey = "Field_Age" },
                new GeneratorField("Birth Date", FieldType.BirthDate, FieldCategory.Personal) { ResourceKey = "Field_BirthDate" },

                // Contact
                new GeneratorField("Email", FieldType.Email, FieldCategory.Contact) { ResourceKey = "Field_Email" },
                new GeneratorField("Phone Number", FieldType.PhoneNumber, FieldCategory.Contact) { ResourceKey = "Field_PhoneNumber" },
                new GeneratorField("Username", FieldType.Username, FieldCategory.Contact) { ResourceKey = "Field_Username" },

                // Location
                new GeneratorField("Country", FieldType.Country, FieldCategory.Location) { ResourceKey = "Field_Country" },
                new GeneratorField("City", FieldType.City, FieldCategory.Location) { ResourceKey = "Field_City" },
                new GeneratorField("Address", FieldType.Address, FieldCategory.Location) { ResourceKey = "Field_Address" },
                new GeneratorField("Zip Code", FieldType.ZipCode, FieldCategory.Location) { ResourceKey = "Field_ZipCode" },

                // Professional
                new GeneratorField("Job Title", FieldType.JobTitle, FieldCategory.Professional) { ResourceKey = "Field_JobTitle" },
                new GeneratorField("Company", FieldType.Company, FieldCategory.Professional) { ResourceKey = "Field_Company" },
                new GeneratorField("Department", FieldType.Department, FieldCategory.Professional) { ResourceKey = "Field_Department" },
                new GeneratorField("Status", FieldType.EmployeeStatus, FieldCategory.Professional) { ResourceKey = "Field_Status" },
                new GeneratorField("Education Level", FieldType.EducationLevel, FieldCategory.Professional) { ResourceKey = "Field_EducationLevel" },

                // Financial
                new GeneratorField("Balance", FieldType.Balance, FieldCategory.Financial) { ResourceKey = "Field_Balance" },
                new GeneratorField("Credit Card", FieldType.CreditCardNumber, FieldCategory.Financial) { ResourceKey = "Field_CreditCard" },
                new GeneratorField("IBAN", FieldType.IBAN, FieldCategory.Financial) { ResourceKey = "Field_IBAN" },

                // Technology
                new GeneratorField("IP Address", FieldType.IPv4, FieldCategory.Technology) { ResourceKey = "Field_IPAddress" },
                new GeneratorField("MAC Address", FieldType.MACAddress, FieldCategory.Technology) { ResourceKey = "Field_MACAddress" },
                new GeneratorField("URL", FieldType.URL, FieldCategory.Technology) { ResourceKey = "Field_URL" },
                new GeneratorField("User Agent", FieldType.UserAgent, FieldCategory.Technology) { ResourceKey = "Field_UserAgent" },

                // Miscellaneous
                new GeneratorField("Hobby", FieldType.Hobby, FieldCategory.Miscellaneous) { ResourceKey = "Field_Hobby" },
                new GeneratorField("Relationship Status", FieldType.RelationshipStatus, FieldCategory.Miscellaneous) { ResourceKey = "Field_RelationshipStatus" },
                new GeneratorField("User Category", FieldType.UserCategory, FieldCategory.Miscellaneous) { ResourceKey = "Field_UserCategory" },
                new GeneratorField("Rating", FieldType.Rating, FieldCategory.Miscellaneous) { ResourceKey = "Field_Rating" },
                new GeneratorField("Description", FieldType.Description, FieldCategory.Miscellaneous) { ResourceKey = "Field_Description" },
                new GeneratorField("Product Name", FieldType.ProductName, FieldCategory.Miscellaneous) { ResourceKey = "Field_ProductName" },
            };

            _fieldsView = CollectionViewSource.GetDefaultView(AvailableFields);
            _fieldsView.Filter = FilterField;
            _fieldsView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(GeneratorField.Category)));

            // Subscribe to selection changes
            foreach (var field in AvailableFields)
            {
                field.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(GeneratorField.IsSelected))
                    {
                        UpdateFieldOrders();
                        GenerateCommand.RaiseCanExecuteChanged();
                        UpdateColumnsText();
                    }
                };
            }
        }

        private void UpdateFieldOrders()
        {
            var selectedFields = AvailableFields.Where(f => f.IsSelected).ToList();
            for (int i = 0; i < selectedFields.Count; i++)
            {
                selectedFields[i].Order = i;
            }
        }

        private void UpdateColumnsText()
        {
            var fieldNames = AvailableFields
                .Where(f => f.IsSelected)
                .OrderBy(f => f.Order)
                .Select(f => f.DisplayName);
            
            Columns = string.Join(", ", fieldNames);
        }

        private bool FilterField(object obj)
        {
            if (string.IsNullOrWhiteSpace(FieldSearchText))
            {
                return true;
            }

            if (obj is GeneratorField field)
            {
                return field.DisplayName.IndexOf(FieldSearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                       field.Type.ToString().IndexOf(FieldSearchText, StringComparison.OrdinalIgnoreCase) >= 0;
            }

            return true;
        }

        #endregion
    }
}
