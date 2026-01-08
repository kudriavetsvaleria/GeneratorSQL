namespace GeneratorSQL.Models.Enums
{
    /// <summary>
    /// Represents the type of data field that can be generated
    /// </summary>
    public enum FieldType
    {
        // Identifiers
        ID,
        GUID,
        
        // Personal Information
        FirstName,
        LastName,
        MiddleName,
        FullName,
        Gender,
        Age,
        BirthDate,
        
        // Contact Information
        Email,
        PhoneNumber,
        Username,
        
        // Location
        Country,
        City,
        State,
        Address,
        StreetAddress,
        ZipCode,
        Latitude,
        Longitude,
        
        // Professional
        JobTitle,
        Company,
        Department,
        EmployeeStatus,
        EducationLevel,
        
        // Financial
        Balance,
        Salary,
        CreditCardNumber,
        IBAN,
        BIC,
        Currency,
        
        // Internet/Technology
        IPv4,
        IPv6,
        MACAddress,
        URL,
        UserAgent,
        DomainName,
        FileName,
        MimeType,
        AppVersion,
        
        // Vehicle
        VehicleManufacturer,
        VehicleModel,
        VehicleFuel,
        VehicleVin,
        
        // Media
        Genre,
        
        // Electronics
        ElectronicsProduct,
        PhoneModel,

        // Clothing
        ClothingItem,
        ClothingSize,

        // Food
        DishName,
        Drink,
        
        // Miscellaneous
        Hobby,
        RelationshipStatus,
        UserCategory,
        Rating,
        Status,
        Description,
        LoremIpsum,
        Color,
        ProductName,
        ISBN,
        
        // Custom
        Custom
    }
}
