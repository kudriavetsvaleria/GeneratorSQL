"# GeneratorSQL - Enhanced Test Data Generator

A powerful WPF application for generating realistic test data and exporting it in multiple formats (SQL, JSON, CSV, XML).

## 🚀 Features

### Data Generation
- **40+ Field Types** including:
  - Personal: Names, Age, Birth Date, Gender
  - Contact: Email, Phone, IP Address
  - Location: Country, City, Address, Coordinates
  - Professional: Job Title, Company, Department, Education
  - Financial: Balance, IBAN, Credit Card, Currency
  - Technology: IPv4/IPv6, MAC Address, URL, User Agent
  - And many more...

### Multi-Dialect SQL Support
- ✅ MySQL
- ✅ PostgreSQL  
- ✅ SQL Server
- ✅ Oracle
- ✅ SQLite

### Export Formats
- 📄 **SQL** - INSERT statements with dialect-specific formatting
- 📋 **JSON** - Pretty-printed or compact
- 📊 **CSV** - With custom delimiters
- 📁 **XML** - Well-formatted with proper structure

### Advanced Features
- **Bogus Library Integration** - Professional, realistic test data
- **Locale Support** - Including Russian and Ukrainian names/addresses
- **MVVM Architecture** - Clean, testable, maintainable code
- **Async Generation** - Responsive UI for large datasets
- **Reproducible Results** - Seed-based generation

## 🏗️ Architecture

### MVVM Pattern
```
├── Core/              # Base classes (ViewModelBase, Commands)
├── Models/            # Data models and enums
├── ViewModels/        # Presentation logic
├── Views/             # XAML UI
├── Services/          # Business logic (Data generation, Export)
└── Helpers/           # Utilities (Converters, Extensions)
```

### Key Technologies
- **.NET Framework 4.7.2** - Target framework
- **WPF** - User interface
- **Bogus** - Realistic data generation
- **Newtonsoft.Json** - JSON serialization
- **CsvHelper** - CSV export
- **MVVM** - Clean architecture pattern

## 📖 Usage

### Basic Workflow
1. **Configure** - Enter table name, column names, and row count
2. **Select SQL Dialect** - Choose your target database
3. **Select Fields** - Check the data types you want to generate
4. **Generate** - Click "Generate SQL" button
5. **Export** - Copy to clipboard or save to file

### Example Output

#### SQL (MySQL)
```sql
INSERT INTO `users` (`id`, `name`, `email`) VALUES
(1, 'John Doe', 'john.doe@example.com'),
(2, 'Jane Smith', 'jane.smith@gmail.com');
```

#### JSON
```json
[
  {
    "id": 1,
    "name": "John Doe",
    "email": "john.doe@example.com"
  },
  {
    "id": 2,
    "name": "Jane Smith",
    "email": "jane.smith@gmail.com"
  }
]
```

#### CSV
```csv
id,name,email
1,John Doe,john.doe@example.com
2,Jane Smith,jane.smith@gmail.com
```

## 🛠️ Development

### Building the Project
```powershell
# Restore NuGet packages
nuget restore GeneratorSQL.sln

# Build
msbuild GeneratorSQL.sln /p:Configuration=Debug
```

### Project Structure
- `Core/` - MVVM base classes
- `Models/` - Data models (GeneratorField, GenerationConfig)
- `ViewModels/` - MainViewModel with commands
- `Views/` - MainWindow with data bindings
- `Services/` - Data generation and export services

## 📦 Dependencies
- **Bogus** (v35.6.1) - Fake data generation
- **Newtonsoft.Json** (v13.0.3) - JSON serialization
- **CsvHelper** (v30.0.1) - CSV export

## 🎯 Roadmap

### Completed ✅
- [x] MVVM Architecture
- [x] Bogus Integration (40+ field types)
- [x] Multi-dialect SQL Export
- [x] JSON/CSV/XML Export
- [x] Async Generation
- [x] Locale Support

### Planned 📋
- [ ] Material Design UI Theme
- [ ] Dark Mode
- [ ] Field Drag-and-Drop Reordering
- [ ] Preview Mode
- [ ] Template System
- [ ] Custom Data Sets
- [ ] Unit Tests

## 📄 License
This project is provided as-is for educational and commercial use.

## 👤 Author
Developed with ❤️ using modern WPF and MVVM best practices.
" 
