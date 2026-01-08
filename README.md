<div align="right">

[EN](./README.md) | [UA](./README.uk.md)

</div>

# GeneratorSQL - Enhanced Test Data Generator

A powerful WPF application for generating realistic test data and exporting it as SQL INSERT statements.

---

![Preview](./assets/preview.gif)

---

## ⬇️ Download (Windows)
Go to Releases and download the latest installer:

GeneratorSQL_Setup.exe
After installation, GeneratorSQL will appear in the Start menu and may create a shortcut on your desktop.

---

## 🚀 Features

### Data Generation
- **Extensive Field Library**: 50+ field types across multiple categories:
  - **Personal**: Names (with localization support), Age, Gender, Birth Date.
  - **Products**: Electronics (Phones, Laptops), Clothing (Items, Sizes), Food (Dishes, Drinks).
  - **Contact**: Email, Phone, IP Address.
  - **Location**: Country (Safe list only), City, Address, Coordinates.
  - **Professional**: Job Title, Company, Department.
  - **Financial**: Balance, IBAN, Credit Card, Currency.
  - **Technology**: IPv4/IPv6, MAC Address, URL.

### SQL Generation
- **Multi-Dialect Support**:
  - ✅ MySQL
  - ✅ PostgreSQL  
  - ✅ SQL Server
  - ✅ Oracle
  - ✅ SQLite

### Localization
- **Bilingual UI**: Instant switching between **English (EN)** and **Ukrainian (UA)**.
- **Smart Data Localization**: Generated data adapts to the selected language (e.g., Ukrainian names and addresses).


### Technical Capabilities
- **Reactive MVVM Architecture**: Built with modern WPF practices for reliability and testability.
- **Bogus Library Integration**: High-performance generation of millions of rows.
- **Dynamic UI**: Category headers and field names translate instantly without restarting the app.
- **Theming**: Built-in Dark and Light themes.
- **Async Processing**: responsive UI even during large dataset generation.
