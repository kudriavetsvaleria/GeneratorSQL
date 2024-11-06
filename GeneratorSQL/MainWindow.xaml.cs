using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace GeneratorSQL
{
    public partial class MainWindow : Window
    {
        private readonly Random _random = new Random(); // Single instance of Random for consistency
        private List<CheckBox> _selectedCheckboxesOrder = new List<CheckBox>();


        public MainWindow()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (!_selectedCheckboxesOrder.Contains(checkBox))
            {
                _selectedCheckboxesOrder.Add(checkBox);
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            _selectedCheckboxesOrder.Remove(checkBox);
        }


        private void OnGenerateSqlClick(object sender, RoutedEventArgs e)
        {
            string tableName = TableNameTextBox.Text;
            string columns = ColumnsTextBox.Text;
            int rowCount = int.TryParse(RowCountTextBox.Text, out var count) ? count : 0;

            if (rowCount <= 0)
            {
                MessageBox.Show("Please enter a valid row count.");
                return;
            }

            StringBuilder outputBuilder = new StringBuilder();

            bool generateSqlFormat = !string.IsNullOrWhiteSpace(tableName) && !string.IsNullOrWhiteSpace(columns);
            bool includeCommas = IncludeCommasCheckBox.IsChecked == true;
            bool useDoubleQuotes = UseDoubleQuotesCheckBox.IsChecked == true;
            string quote = useDoubleQuotes ? "\"" : "'";

            if (generateSqlFormat)
            {
                outputBuilder.AppendLine($"INSERT INTO {tableName} ({columns}) VALUES");
            }

            for (int i = 0; i < rowCount; i++)
            {
                if (generateSqlFormat)
                {
                    outputBuilder.Append("(");
                }

                bool isFirstValue = true;

                foreach (var checkBox in _selectedCheckboxesOrder)
                {
                    string valueToAdd = string.Empty;

                    //ID 1
                    if (checkBox == GenerateIDCheckBox && GenerateIDCheckBox.IsChecked == true)
                        valueToAdd = $"{i + 1}";
                    //NAME 1
                    if (checkBox == GenerateNameCheckBox && GenerateNameCheckBox.IsChecked == true)
                        valueToAdd = $"{quote}{GenerateRandomName()}{quote}";
                    //LAST NAME 1
                    if (checkBox == GenerateSurnameCheckBox && GenerateSurnameCheckBox.IsChecked == true)
                        valueToAdd = $"{quote}{GenerateRandomSurname()}{quote}";
                    //GENDER 1
                    if (checkBox == GenerateGenderCheckBox && GenerateGenderCheckBox.IsChecked == true)
                        valueToAdd = $"{quote}{GenerateRandomGender()}{quote}";
                    //CITY 1
                    if (checkBox == GenerateCityCheckBox && GenerateCityCheckBox.IsChecked == true)
                        valueToAdd = $"{quote}{GenerateRandomCity()}{quote}";
                    //MIDDLE NAME 1
                    if (checkBox == GenerateMiddleNameCheckBox && GenerateMiddleNameCheckBox.IsChecked == true)
                        valueToAdd = $"{quote}{GenerateRandomMiddleName()}{quote}";
                    //AGE 1
                    if (checkBox == GenerateAgeCheckBox && GenerateAgeCheckBox.IsChecked == true)
                        valueToAdd = $"{GenerateRandomAge()}";
                    //BD 1
                    if (checkBox == GenerateBirthDateCheckBox && GenerateBirthDateCheckBox.IsChecked == true)
                        valueToAdd = $"{quote}{GenerateBirthDateFromAge(GenerateRandomAge()):yyyy-MM-dd}{quote}";
                    //ADRESS 2
                    if (checkBox == GenerateAddressCheckBox && GenerateAddressCheckBox.IsChecked == true)
                        valueToAdd = $"{quote}{GenerateRandomAddress()}{quote}";
                    //PHONE NUMBER 2
                    if (checkBox == GeneratePhoneNumberCheckBox && GeneratePhoneNumberCheckBox.IsChecked == true)
                        valueToAdd = $"{quote}{GenerateRandomPhoneNumber()}{quote}";
                    //PROFESSION 2
                    if (checkBox == GenerateProfessionCheckBox && GenerateProfessionCheckBox.IsChecked == true)
                        valueToAdd = $"{quote}{GenerateRandomProfession()}{quote}";
                    //COMPANY 2
                    if (checkBox == GenerateCompanyCheckBox && GenerateCompanyCheckBox.IsChecked == true)
                        valueToAdd = $"{quote}{GenerateRandomCompany()}{quote}";
                    //STATUS 2
                    if (checkBox == GenerateStatusCheckBox && GenerateStatusCheckBox.IsChecked == true)
                        valueToAdd = $"{quote}{GenerateRandomStatus()}{quote}";
                    //EDUCATION LEVEL 2
                    if (checkBox == GenerateEducationLevelCheckBox && GenerateEducationLevelCheckBox.IsChecked == true)
                        valueToAdd = $"{quote}{GenerateRandomEducationLevel()}{quote}";
                    //HOBBY 2
                    if (checkBox == GenerateHobbyCheckBox && GenerateHobbyCheckBox.IsChecked == true)
                        valueToAdd = $"{quote}{GenerateRandomHobby()}{quote}";
                    //RELATIONSHIP STATUS 2
                    if (checkBox == GenerateRelationshipStatusCheckBox && GenerateRelationshipStatusCheckBox.IsChecked == true)
                        valueToAdd = $"{quote}{GenerateRandomRelationshipStatus()}{quote}";
                    //EMAIL 3
                    if (checkBox == GenerateEmailCheckBox && GenerateEmailCheckBox.IsChecked == true)
                        valueToAdd = $"{quote}{GenerateRandomEmail()}{quote}";
                    //BALANCE 3
                    if (checkBox == GenerateBalanceCheckBox && GenerateBalanceCheckBox.IsChecked == true)
                        valueToAdd = $"{GenerateRandomBalance():0.00}";
                    //RATING 3
                    if (checkBox == GenerateRatingCheckBox && GenerateRatingCheckBox.IsChecked == true)
                        valueToAdd = $"{quote}{GenerateRandomRating()}{quote}";
                    //USER CATEGORY 3
                    if (checkBox == GenerateUserCategoryCheckBox && GenerateUserCategoryCheckBox.IsChecked == true)
                        valueToAdd = $"{quote}{GenerateRandomUserCategory()}{quote}";
                    //COUNTRY
                    if (checkBox == GenerateCountryCheckBox && GenerateCountryCheckBox.IsChecked == true)
                        valueToAdd = $"{quote}{GenerateRandomCountry()}{quote}";
                    //IP ADDRESS
                    if (checkBox == GenerateIPAddressCheckBox && GenerateIPAddressCheckBox.IsChecked == true)
                        valueToAdd = $"{quote}{GenerateRandomIPAddress()}{quote}";



                    if (!string.IsNullOrEmpty(valueToAdd))
                    {
                        if (!isFirstValue)
                        {
                            outputBuilder.Append(includeCommas ? ", " : " ");
                        }
                        outputBuilder.Append(valueToAdd);
                        isFirstValue = false;
                    }
                }

                if (generateSqlFormat)
                {
                    outputBuilder.Append(i == rowCount - 1 ? ");" : "),\n");
                }
                else
                {
                    outputBuilder.AppendLine();
                }
            }

            SqlOutputTextBox.Text = outputBuilder.ToString();
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
            string[] countries = { "United States", "Canada", "Germany", "France", "United Kingdom", "Australia", "Italy", "Spain", "Netherlands", "Sweden", "Ukraine" };
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

        private string GenerateRandomMiddleName()
        {
            string[] middleNames = { "Ivanovich", "Petrovich", "Sergeevich", "Alekseevna", "Nikolaevna", "Dmitrievna" };
            return middleNames[_random.Next(middleNames.Length)];
        }

        private string GenerateRandomGender()
        {
            string[] genders = { "Male", "Female", "Other" };
            return genders[_random.Next(genders.Length)];
        }

        private int GenerateRandomAge(int minAge = 18, int maxAge = 70)
        {
            return _random.Next(minAge, maxAge + 1);
        }

        private DateTime GenerateBirthDateFromAge(int age)
        {
            DateTime today = DateTime.Today;
            DateTime birthDate = today.AddYears(-age).AddDays(-_random.Next(365));
            return birthDate;
        }

        private string GenerateRandomCity()
        {
            string[] cities = { "New York", "Berlin", "Paris", "Tokyo", "Sydney", "Madrid", "Rome", "Dnipro", "London", "Odesa", "Kyiv" };
            return cities[_random.Next(cities.Length)];
        }

        private string GenerateRandomAddress()
        {
            string[] streets = { "Main St", "High St", "Broadway", "Maple Ave", "Elm St", "Oak St", "Pine Ave" };
            int houseNumber = _random.Next(1, 200);
            int apartmentNumber = _random.Next(1, 50);
            return $"{houseNumber} {streets[_random.Next(streets.Length)]}, Apt {apartmentNumber}";
        }


        private string GenerateRandomPhoneNumber()
        {
            return $"+{_random.Next(1, 999)}-{_random.Next(100, 999)}-{_random.Next(1000, 9999)}";
        }

        private string GenerateRandomProfession()
        {
            string[] professions = { "Engineer", "Teacher", "Doctor", "Lawyer", "Artist", "Scientist", "Writer", "Chef" };
            return professions[_random.Next(professions.Length)];
        }

        private string GenerateRandomCompany()
        {
            string[] companies = { "TechCorp", "HealthPlus", "EduSolve", "Financo", "BuildIt", "Creativa" };
            return companies[_random.Next(companies.Length)];
        }

        private string GenerateRandomStatus()
        {
            string[] statuses = { "Active", "On Probation", "Dismissed" };
            return statuses[_random.Next(statuses.Length)];
        }

        private string GenerateRandomEducationLevel()
        {
            string[] educationLevels = { "High School", "Bachelor", "Master", "PhD" };
            return educationLevels[_random.Next(educationLevels.Length)];
        }

        private string GenerateRandomHobby()
        {
            string[] hobbies = { "Sports", "Reading", "Traveling", "Cooking", "Gaming", "Photography" };
            return hobbies[_random.Next(hobbies.Length)];
        }

        private string GenerateRandomRelationshipStatus()
        {
            string[] relationshipStatuses = { "Single", "Married", "Divorced" };
            return relationshipStatuses[_random.Next(relationshipStatuses.Length)];
        }

        private string GenerateRandomIPAddress()
        {
            return $"{_random.Next(1, 255)}.{_random.Next(1, 255)}.{_random.Next(1, 255)}.{_random.Next(1, 255)}";
        }

        private string GenerateRandomUserCategory()
        {
            string[] categories = { "VIP", "New Customer", "Returning Customer" };
            return categories[_random.Next(categories.Length)];
        }

        private int GenerateRandomRating()
        {
            return _random.Next(1, 6); // Rating from 1 to 5
        }

    }
}
