using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace GeneratorSQL
{
    public partial class MainWindow : Window
    {
        private readonly Random _random = new Random(); // Single instance of Random for consistency

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnGenerateSqlClick(object sender, RoutedEventArgs e)
        {
            string tableName = TableNameTextBox.Text;
            string columns = ColumnsTextBox.Text;
            int rowCount = int.TryParse(RowCountTextBox.Text, out var count) ? count : 0;

            if (rowCount <= 0 || string.IsNullOrWhiteSpace(tableName) || string.IsNullOrWhiteSpace(columns))
            {
                MessageBox.Show("Please enter a valid table name, columns, and row count.");
                return;
            }

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine($"INSERT INTO {tableName} ({columns}) VALUES");

            for (int i = 0; i < rowCount; i++)
            {
                sqlBuilder.Append("(");

                if (GenerateIDCheckBox.IsChecked == true)
                    sqlBuilder.Append($"{i + 1}, ");

                if (GenerateNameCheckBox.IsChecked == true)
                    sqlBuilder.Append($"'{GenerateRandomName()}', ");

                if (GenerateSurnameCheckBox.IsChecked == true)
                    sqlBuilder.Append($"'{GenerateRandomSurname()}', ");

                if (GenerateDateCheckBox.IsChecked == true)
                    sqlBuilder.Append($"'{GenerateRandomDate(new DateTime(1980, 1, 1), DateTime.Now):yyyy-MM-dd}', ");

                if (GenerateEmailCheckBox.IsChecked == true)
                    sqlBuilder.Append($"'{GenerateRandomEmail()}', ");

                if (GenerateCountryCheckBox.IsChecked == true)
                    sqlBuilder.Append($"'{GenerateRandomCountry()}', ");

                if (GenerateBalanceCheckBox.IsChecked == true)
                    sqlBuilder.Append($"{GenerateRandomBalance()}, ");

                sqlBuilder.Length -= 2; // Remove last comma and space
                sqlBuilder.Append(i == rowCount - 1 ? ");" : "),\n");
            }

            SqlOutputTextBox.Text = sqlBuilder.ToString();
        }

        private string GenerateRandomName()
        {
            string[] names = { "Michael", "Alexander", "Sarah", "Mary", "James", "Sophia", "Emma", "John", "David", "Olivia" };
            return names[_random.Next(names.Length)];
        }

        private string GenerateRandomSurname()
        {
            string[] surnames = { "Smith", "Johnson", "Brown", "Williams", "Jones", "Miller", "Davis", "Garcia", "Rodriguez", "Martinez" };
            return surnames[_random.Next(surnames.Length)];
        }

        private DateTime GenerateRandomDate(DateTime start, DateTime end)
        {
            int range = (end - start).Days;
            return start.AddDays(_random.Next(range));
        }

        private string GenerateRandomEmail()
        {
            string[] domains = { "@gmail.com", "@yahoo.com", "@hotmail.com", "@outlook.com" };
            string[] emailPrefixes = { "user", "contact", "admin", "support", "info" };
            return $"{emailPrefixes[_random.Next(emailPrefixes.Length)]}{_random.Next(100, 999)}{domains[_random.Next(domains.Length)]}";
        }

        private string GenerateRandomCountry()
        {
            string[] countries = { "United States", "Canada", "Germany", "France", "United Kingdom", "Australia", "Italy", "Spain", "Netherlands", "Sweden" };
            return countries[_random.Next(countries.Length)];
        }

        private decimal GenerateRandomBalance()
        {
            return Math.Round((decimal)(_random.Next(1000, 10000) + _random.NextDouble()), 2);
        }

        private void TableNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TableNamePlaceholder.Visibility = string.IsNullOrWhiteSpace(TableNameTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ColumnsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ColumnsPlaceholder.Visibility = string.IsNullOrWhiteSpace(ColumnsTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void RowCountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RowCountPlaceholder.Visibility = string.IsNullOrWhiteSpace(RowCountTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void OnCopyToClipboardClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SqlOutputTextBox.Text))
            {
                Clipboard.SetText(SqlOutputTextBox.Text);
                MessageBox.Show("SQL text copied to clipboard!", "Copied", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("There is no SQL text to copy.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
