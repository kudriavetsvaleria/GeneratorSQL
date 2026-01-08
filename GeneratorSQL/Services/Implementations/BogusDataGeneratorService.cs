using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using GeneratorSQL.Models;
using GeneratorSQL.Models.Enums;
using GeneratorSQL.Services.Interfaces;

namespace GeneratorSQL.Services.Implementations
{
    /// <summary>
    /// Data generator service using Bogus library - OPTIMIZED for large datasets
    /// </summary>
    public class BogusDataGeneratorService : IDataGeneratorService
    {
        private readonly Dictionary<string, Faker> _fakerCache = new Dictionary<string, Faker>();
        private int? _seed;

        // Localization data
        private static readonly Dictionary<string, Dictionary<string, string[]>> _localizedData = new Dictionary<string, Dictionary<string, string[]>>
        {
            ["en"] = new Dictionary<string, string[]>
            {
                ["EmployeeStatus"] = new[] { "Active", "On Probation", "On Leave", "Dismissed" },
                ["EducationLevel"] = new[] { "High School", "Bachelor", "Master", "PhD" },
                ["Country"] = new[] { "USA", "Canada", "UK", "Germany", "France", "Italy", "Spain", "Poland", "Ukraine", "Japan", "China", "Australia", "Brazil", "India", "Netherlands", "Sweden", "Norway", "Switzerland" },
                ["Hobby"] = new[] { "Sports", "Reading", "Traveling", "Cooking", "Gaming", "Photography", "Music", "Art" },
                ["RelationshipStatus"] = new[] { "Single", "Married", "Divorced", "Widowed" },
                ["UserCategory"] = new[] { "VIP", "Premium", "Standard", "New Customer", "Returning Customer" },
                ["Status"] = new[] { "Active", "Inactive", "Pending", "Suspended" },
                ["Department"] = new[] { "Engineering", "Sales", "Marketing", "HR", "Finance", "Legal", "Support", "IT", "Operations" },
                ["VehicleFuel"] = new[] { "Gasoline", "Diesel", "Electric", "Hybrid", "Propane", "Hydrogen" },
                ["Genre"] = new[] { "Rock", "Pop", "Jazz", "Classical", "Hip Hop", "Electronic", "Blues", "Country", "Reggae", "Folk", "Metal" },
                ["ElectronicsProduct"] = new[] { "Smartphone", "Laptop", "Tablet", "Smart Watch", "Headphones", "Camera", "TV", "Console", "Speaker", "Printer" },
                ["PhoneModel"] = new[] { "iPhone 15", "Samsung Galaxy S24", "Google Pixel 8", "Xiaomi 14", "OnePlus 12", "Sony Xperia 1", "iPhone 14 Pro", "Samsung Galaxy Z Fold", "Asus ROG Phone" },
                ["ClothingItem"] = new[] { "T-Shirt", "Jeans", "Jacket", "Sweater", "Dress", "Skirt", "Shorts", "Coat", "Suit", "Hoodie" },
                ["ClothingSize"] = new[] { "XS", "S", "M", "L", "XL", "XXL" },
                ["DishName"] = new[] { "Pizza", "Burger", "Pasta", "Sushi", "Salad", "Steak", "Soup", "Sandwich", "Tacos", "Curry" },
                ["Drink"] = new[] { "Coffee", "Tea", "Water", "Juice", "Soda", "Beer", "Wine", "Cocktail", "Milkshake", "Lemonade" }
            },
            ["uk"] = new Dictionary<string, string[]>
            {
                ["EmployeeStatus"] = new[] { "Активний", "Випробувальний термін", "У відпустці", "Звільнений" },
                ["EducationLevel"] = new[] { "Середня школа", "Бакалавр", "Магістр", "Доктор наук" },
                ["Country"] = new[] { "США", "Канада", "Великобританія", "Німеччина", "Франція", "Італія", "Іспанія", "Польща", "Україна", "Японія", "Китай", "Австралія", "Бразилія", "Індія", "Нідерланди", "Швеція", "Норвегія", "Швейцарія" },
                ["Hobby"] = new[] { "Спорт", "Читання", "Подорожі", "Кулінарія", "Ігри", "Фотографія", "Музика", "Мистецтво" },
                ["RelationshipStatus"] = new[] { "Неодружений/на", "Одружений/на", "Розлучений/на", "Вдівець/ва" },
                ["UserCategory"] = new[] { "VIP", "Преміум", "Стандарт", "Новий клієнт", "Постійний клієнт" },
                ["Status"] = new[] { "Активний", "Неактивний", "В очікуванні", "Призупинено" },
                ["Department"] = new[] { "Розробка", "Продажі", "Маркетинг", "HR", "Фінанси", "Юридичний", "Підтримка", "IT", "Операційний" },
                ["VehicleFuel"] = new[] { "Бензин", "Дизель", "Електро", "Гібрид", "Газ", "Водень" },
                ["Genre"] = new[] { "Рок", "Поп", "Джаз", "Класика", "Хіп-хоп", "Електронна", "Блюз", "Кантрі", "Реггі", "Фолк", "Метал" },
                ["ElectronicsProduct"] = new[] { "Смартфон", "Ноутбук", "Планшет", "Смарт-годинник", "Навушники", "Камера", "Телевізор", "Ігрова консоль", "Колонка", "Принтер" },
                ["PhoneModel"] = new[] { "iPhone 15", "Samsung Galaxy S24", "Google Pixel 8", "Xiaomi 14", "OnePlus 12", "Sony Xperia 1", "iPhone 14 Pro", "Samsung Galaxy Z Fold", "Asus ROG Phone" },
                ["ClothingItem"] = new[] { "Футболка", "Джинси", "Куртка", "Светр", "Сукня", "Спідниця", "Шорти", "Пальто", "Костюм", "Худі" },
                ["ClothingSize"] = new[] { "XS", "S", "M", "L", "XL", "XXL" },
                ["DishName"] = new[] { "Борщ", "Вареники", "Піца", "Суші", "Бургер", "Паста", "Сало", "Голубці", "Котлета по-київськи", "Деруни" },
                ["Drink"] = new[] { "Кава", "Чай", "Вода", "Сік", "Лимонад", "Квас", "Узвар", "Пиво", "Вино", "Коктейль" },
                ["Description"] = new[] 
                { 
                    "Цей продукт має високу якість.", 
                    "Чудовий вибір для повсякденного використання.", 
                    "Ергономічний дизайн та зручність.", 
                    "Виготовлено з екологічно чистих матеріалів.", 
                    "Найкраще співвідношення ціни та якості.", 
                    "Надійність та довговічність гарантовано.", 
                    "Інноваційні технології виробництва.", 
                    "Простий у використанні та обслуговуванні.", 
                    "Компактний розмір та легка вага.", 
                    "Стильний аксесуар для вашого дому.",
                    "Відмінний подарунок для близьких.",
                    "Гарантія якості від виробника."
                }
            }
        };

        public BogusDataGeneratorService()
        {
            // Pre-create common locales
            _fakerCache["en"] = new Faker("en");
            _fakerCache["uk"] = new Faker("uk");
        }

        public void SetSeed(int seed)
        {
            _seed = seed;
            Randomizer.Seed = new Random(seed);
            
            // Recreate all cached fakers with new seed
            _fakerCache.Clear();
            _fakerCache["en"] = new Faker("en");
            _fakerCache["uk"] = new Faker("uk");
        }

        private Faker GetFaker(string locale)
        {
            if (!_fakerCache.ContainsKey(locale))
            {
                _fakerCache[locale] = new Faker(locale);
            }
            return _fakerCache[locale];
        }

        public string GenerateValue(FieldType fieldType, string locale = "en")
        {
            var faker = GetFaker(locale);

            return fieldType switch
            {
                // Identifiers
                FieldType.ID => faker.IndexFaker.ToString(),
                FieldType.GUID => faker.Random.Guid().ToString(),

                // Personal Information
                FieldType.FirstName => faker.Name.FirstName(),
                FieldType.LastName => faker.Name.LastName(),
                FieldType.MiddleName => (locale == "uk") 
                    ? GenerateUkrainianMiddleName(faker) 
                    : faker.Name.FirstName(),
                FieldType.FullName => faker.Name.FullName(),
                FieldType.Gender => GenerateLocalizedGender(faker, locale),
                FieldType.Age => faker.Random.Int(18, 70).ToString(),
                FieldType.BirthDate => faker.Date.Past(50, DateTime.Now.AddYears(-18)).ToString("yyyy-MM-dd"),

                // Contact Information
                FieldType.Email => faker.Internet.Email(),
                FieldType.PhoneNumber => faker.Phone.PhoneNumber(),
                FieldType.Username => faker.Internet.UserName(),

                // Location
                FieldType.Country => GetLocalizedValue(faker, "Country", locale),
                FieldType.City => faker.Address.City(),
                FieldType.State => faker.Address.State(),
                FieldType.Address => faker.Address.FullAddress(),
                FieldType.StreetAddress => faker.Address.StreetAddress(),
                FieldType.ZipCode => faker.Address.ZipCode(),
                FieldType.Latitude => faker.Address.Latitude().ToString(),
                FieldType.Longitude => faker.Address.Longitude().ToString(),

                // Professional
                FieldType.JobTitle => faker.Name.JobTitle(),
                FieldType.Company => faker.Company.CompanyName(),
                FieldType.Department => GetLocalizedValue(faker, "Department", locale) ?? faker.Commerce.Department(),
                FieldType.EmployeeStatus => GetLocalizedValue(faker, "EmployeeStatus", locale),
                FieldType.EducationLevel => GetLocalizedValue(faker, "EducationLevel", locale),

                // Financial
                FieldType.Balance => faker.Finance.Amount(0, 100000, 2).ToString(),
                FieldType.Salary => faker.Finance.Amount(30000, 200000, 0).ToString(),
                FieldType.CreditCardNumber => faker.Finance.CreditCardNumber(),
                FieldType.IBAN => faker.Finance.Iban(),
                FieldType.BIC => faker.Finance.Bic(),
                FieldType.Currency => faker.Finance.Currency().Code,

                // Internet/Technology
                FieldType.IPv4 => faker.Internet.Ip(),
                FieldType.IPv6 => faker.Internet.Ipv6(),
                FieldType.MACAddress => faker.Internet.Mac(),
                FieldType.URL => faker.Internet.Url(),
                FieldType.UserAgent => faker.Internet.UserAgent(),
                FieldType.DomainName => faker.Internet.DomainName(),
                FieldType.FileName => faker.System.FileName(),
                FieldType.MimeType => faker.System.MimeType(),
                FieldType.AppVersion => faker.System.Semver(),

                // Vehicle
                FieldType.VehicleManufacturer => faker.Vehicle.Manufacturer(),
                FieldType.VehicleModel => faker.Vehicle.Model(),
                FieldType.VehicleFuel => GetLocalizedValue(faker, "VehicleFuel", locale),
                FieldType.VehicleVin => faker.Vehicle.Vin(),

                // Media
                FieldType.Genre => GetLocalizedValue(faker, "Genre", locale),

                // Electronics
                FieldType.ElectronicsProduct => GetLocalizedValue(faker, "ElectronicsProduct", locale),
                FieldType.PhoneModel => GetLocalizedValue(faker, "PhoneModel", locale),

                // Clothing
                FieldType.ClothingItem => GetLocalizedValue(faker, "ClothingItem", locale),
                FieldType.ClothingSize => GetLocalizedValue(faker, "ClothingSize", locale),

                // Food
                FieldType.DishName => GetLocalizedValue(faker, "DishName", locale),
                FieldType.Drink => GetLocalizedValue(faker, "Drink", locale),

                // Miscellaneous
                FieldType.Hobby => GetLocalizedValue(faker, "Hobby", locale),
                FieldType.RelationshipStatus => GetLocalizedValue(faker, "RelationshipStatus", locale),
                FieldType.UserCategory => GetLocalizedValue(faker, "UserCategory", locale),
                FieldType.Rating => faker.Random.Int(1, 5).ToString(),
                FieldType.Status => GetLocalizedValue(faker, "Status", locale),
                FieldType.Description => !string.IsNullOrEmpty(GetLocalizedValue(faker, "Description", locale)) 
                    ? GetLocalizedValue(faker, "Description", locale) 
                    : faker.Lorem.Sentence(),
                FieldType.LoremIpsum => faker.Lorem.Paragraph(),
                FieldType.Color => faker.Commerce.Color(),
                FieldType.ProductName => faker.Commerce.ProductName(),
                FieldType.ISBN => faker.Commerce.Ean13(),

                _ => string.Empty
            };
        }

        public List<Dictionary<string, object>> GenerateRows(List<GeneratorField> fields, int count, string locale = "en")
        {
            var rows = new List<Dictionary<string, object>>(count); // Pre-allocate capacity
            var faker = GetFaker(locale); // Get cached faker once

            // Sort fields by order once
            var sortedFields = fields.Where(f => f.IsSelected).OrderBy(f => f.Order).ToList();

            for (int i = 0; i < count; i++)
            {
                var row = new Dictionary<string, object>(sortedFields.Count); // Pre-allocate

                foreach (var field in sortedFields)
                {
                    string value;
                    
                    // Special handling for ID field - use sequential numbering
                    if (field.Type == FieldType.ID)
                    {
                        value = (i + 1).ToString();
                    }
                    else
                    {
                        value = GenerateValue(field.Type, locale);
                    }

                    row[field.DisplayName] = value;
                }

                rows.Add(row);
            }

            return rows;
        }

        private string GenerateUkrainianMiddleName(Faker faker)
        {
            var middleNames = new[]
            {
                "Олександрович", "Олексійович", "Андрійович", "Борисович", "Володимирович",
                "Дмитрович", "Євгенович", "Іванович", "Михайлович", "Миколайович",
                "Олегович", "Петрович", "Сергійович", "Юрійович",
                "Олександрівна", "Олексіївна", "Андріївна", "Борисівна", "Володимирівна",
                "Дмитрівна", "Євгенівна", "Іванівна", "Михайлівна", "Миколаївна",
                "Олегівна", "Петрівна", "Сергіївна", "Юріївна"
            };
            
            return faker.PickRandom(middleNames);
        }

        private string GenerateLocalizedGender(Faker faker, string locale)
        {
            if (locale == "uk")
            {
                return faker.PickRandom(new[] { "Чоловіча", "Жіноча" });
            }
            
            return faker.PickRandom(new[] { "Male", "Female", "Other" });
        }

        private string GetLocalizedValue(Faker faker, string fieldName, string locale)
        {
            if (_localizedData.TryGetValue(locale, out var fieldData) && 
                fieldData.TryGetValue(fieldName, out var values))
            {
                return faker.PickRandom(values);
            }

            // Fallback to English if locale not found
            if (_localizedData["en"].TryGetValue(fieldName, out var enValues))
            {
                return faker.PickRandom(enValues);
            }

            return string.Empty;
        }
    }
}
