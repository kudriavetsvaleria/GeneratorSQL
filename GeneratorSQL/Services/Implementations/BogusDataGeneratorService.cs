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

        public BogusDataGeneratorService()
        {
            // Pre-create common locales
            _fakerCache["en"] = new Faker("en");
            _fakerCache["ru"] = new Faker("ru");
            _fakerCache["uk"] = new Faker("uk");
        }

        public void SetSeed(int seed)
        {
            _seed = seed;
            Randomizer.Seed = new Random(seed);
            
            // Recreate all cached fakers with new seed
            _fakerCache.Clear();
            _fakerCache["en"] = new Faker("en");
            _fakerCache["ru"] = new Faker("ru");
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
                FieldType.MiddleName => (locale == "ru" || locale == "uk") 
                    ? GenerateRussianMiddleName(faker) 
                    : faker.Name.FirstName(),
                FieldType.FullName => faker.Name.FullName(),
                FieldType.Gender => faker.PickRandom(new[] { "Male", "Female", "Other" }),
                FieldType.Age => faker.Random.Int(18, 70).ToString(),
                FieldType.BirthDate => faker.Date.Past(50, DateTime.Now.AddYears(-18)).ToString("yyyy-MM-dd"),

                // Contact Information
                FieldType.Email => faker.Internet.Email(),
                FieldType.PhoneNumber => faker.Phone.PhoneNumber(),
                FieldType.Username => faker.Internet.UserName(),

                // Location
                FieldType.Country => faker.Address.Country(),
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
                FieldType.Department => faker.Commerce.Department(),
                FieldType.EmployeeStatus => faker.PickRandom(new[] { "Active", "On Probation", "On Leave", "Dismissed" }),
                FieldType.EducationLevel => faker.PickRandom(new[] { "High School", "Bachelor", "Master", "PhD" }),

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

                // Miscellaneous
                FieldType.Hobby => faker.PickRandom(new[] { "Sports", "Reading", "Traveling", "Cooking", "Gaming", "Photography", "Music", "Art" }),
                FieldType.RelationshipStatus => faker.PickRandom(new[] { "Single", "Married", "Divorced", "Widowed" }),
                FieldType.UserCategory => faker.PickRandom(new[] { "VIP", "Premium", "Standard", "New Customer", "Returning Customer" }),
                FieldType.Rating => faker.Random.Int(1, 5).ToString(),
                FieldType.Status => faker.PickRandom(new[] { "Active", "Inactive", "Pending", "Suspended" }),
                FieldType.Description => faker.Lorem.Sentence(),
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

        private string GenerateRussianMiddleName(Faker faker)
        {
            var middleNames = new[]
            {
                "Александрович", "Алексеевич", "Андреевич", "Борисович", "Владимирович",
                "Дмитриевич", "Евгеньевич", "Иванович", "Михайлович", "Николаевич",
                "Олегович", "Петрович", "Сергеевич", "Юрьевич",
                "Александровна", "Алексеевна", "Андреевна", "Борисовна", "Владимировна",
                "Дмитриевна", "Евгеньевна", "Ивановна", "Михайловна", "Николаевна",
                "Олеговна", "Петровна", "Сергеевна", "Юрьевна"
            };
            
            return faker.PickRandom(middleNames);
        }
    }
}
