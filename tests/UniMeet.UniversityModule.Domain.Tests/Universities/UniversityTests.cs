using FluentAssertions;
using UniMeet.UniversityModule.Domain.Universities;
using UniMeet.UniversityModule.Domain.Universities.Exceptions;

namespace UniMeet.UniversityModule.Domain.Tests.Universities;

public class UniversityTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateUniversity()
    {
        // Arrange
        var name = "Example University";
        var country = "Poland";
        var voivodeship = "Mazowieckie";
        var city = "Warsaw";
        var address = "Main Street 123";

        // Act
        var university = new University(name, country, voivodeship, city, address);

        // Assert
        university.Name.Should().Be(name);
        university.Country.Should().Be(country);
        university.Voivodeship.Should().Be(voivodeship);
        university.City.Should().Be(city);
        university.Address.Should().Be(address);
        university.Departments.Should().NotBeNull().And.BeEmpty();
        university.AllowedEmailDomains.Should().NotBeNull().And.BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidName_ShouldThrowInvalidUniversityNameException(string invalidName)
    {
        // Act
        var act = () => new University(invalidName, "Poland", "Mazowieckie", "Warsaw", "Street 123");

        // Assert
        act.Should().Throw<InvalidUniversityNameException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidCountry_ShouldThrowInvalidCountryNameException(string invalidCountry)
    {
        // Act
        var act = () => new University("University", invalidCountry, "Mazowieckie", "Warsaw", "Street 123");

        // Assert
        act.Should().Throw<InvalidCountryNameException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidVoivodeship_ShouldThrowInvalidVoivodeshipNameException(string invalidVoivodeship)
    {
        // Act
        var act = () => new University("University", "Poland", invalidVoivodeship, "Warsaw", "Street 123");

        // Assert
        act.Should().Throw<InvalidVoivodeshipNameException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidCity_ShouldThrowInvalidCityNameException(string invalidCity)
    {
        // Act
        var act = () => new University("University", "Poland", "Mazowieckie", invalidCity, "Street 123");

        // Assert
        act.Should().Throw<InvalidCityNameException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidAddress_ShouldThrowInvalidAddressException(string invalidAddress)
    {
        // Act
        var act = () => new University("University", "Poland", "Mazowieckie", "Warsaw", invalidAddress);

        // Assert
        act.Should().Throw<InvalidAddressException>();
    }

    [Fact]
    public void Rename_WithValidName_ShouldUpdateName()
    {
        // Arrange
        var university = new University("Old Name", "Poland", "Mazowieckie", "Warsaw", "Street 123");
        var newName = "New Name";

        // Act
        university.Rename(newName);

        // Assert
        university.Name.Should().Be(newName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Rename_WithInvalidName_ShouldThrowInvalidUniversityNameException(string invalidName)
    {
        // Arrange
        var university = new University("University", "Poland", "Mazowieckie", "Warsaw", "Street 123");

        // Act
        var act = () => university.Rename(invalidName);

        // Assert
        act.Should().Throw<InvalidUniversityNameException>();
    }

    [Fact]
    public void ChangeAddress_WithValidAddress_ShouldUpdateAddress()
    {
        // Arrange
        var university = new University("University", "Poland", "Mazowieckie", "Warsaw", "Old Street 1");
        var newAddress = "New Street 2";

        // Act
        university.ChangeAddress(newAddress);

        // Assert
        university.Address.Should().Be(newAddress);
    }

    [Fact]
    public void ChangeCity_WithValidCity_ShouldUpdateCity()
    {
        // Arrange
        var university = new University("University", "Poland", "Mazowieckie", "Warsaw", "Street 123");
        var newCity = "Krakow";

        // Act
        university.ChangeCity(newCity);

        // Assert
        university.City.Should().Be(newCity);
    }

    [Fact]
    public void ChangeVoivodeship_WithValidVoivodeship_ShouldUpdateVoivodeship()
    {
        // Arrange
        var university = new University("University", "Poland", "Mazowieckie", "Warsaw", "Street 123");
        var newVoivodeship = "Malopolskie";

        // Act
        university.ChangeVoivodeship(newVoivodeship);

        // Assert
        university.Voivodeship.Should().Be(newVoivodeship);
    }

    [Fact]
    public void ChangeCountry_WithValidCountry_ShouldUpdateCountry()
    {
        // Arrange
        var university = new University("University", "Poland", "Mazowieckie", "Warsaw", "Street 123");
        var newCountry = "Germany";

        // Act
        university.ChangeCountry(newCountry);

        // Assert
        university.Country.Should().Be(newCountry);
    }

    [Fact]
    public void AddDepartment_WithValidName_ShouldAddDepartment()
    {
        // Arrange
        var university = new University("University", "Poland", "Mazowieckie", "Warsaw", "Street 123");
        var departmentName = "Computer Science";

        // Act
        university.AddDepartment(departmentName);

        // Assert
        university.Departments.Should().HaveCount(1);
        university.Departments.First().Name.Should().Be(departmentName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void AddDepartment_WithInvalidName_ShouldThrowInvalidDepartmentNameException(string invalidName)
    {
        // Arrange
        var university = new University("University", "Poland", "Mazowieckie", "Warsaw", "Street 123");

        // Act
        var act = () => university.AddDepartment(invalidName);

        // Assert
        act.Should().Throw<InvalidDepartmentNameException>();
    }

    [Fact]
    public void AddDepartment_WithDuplicateName_ShouldThrowDepartmentAlreadyExistsException()
    {
        // Arrange
        var university = new University("University", "Poland", "Mazowieckie", "Warsaw", "Street 123");
        var departmentName = "Computer Science";
        university.AddDepartment(departmentName);

        // Act
        var act = () => university.AddDepartment(departmentName);

        // Assert
        act.Should().Throw<DepartmentAlreadyExistsException>();
    }

    [Fact]
    public void AddAllowedEmailDomain_WithValidDomain_ShouldAddDomain()
    {
        // Arrange
        var university = new University("University", "Poland", "Mazowieckie", "Warsaw", "Street 123");
        var domain = "example.com";

        // Act
        university.AddAllowedEmailDomain(domain);

        // Assert
        university.AllowedEmailDomains.Should().HaveCount(1);
        university.AllowedEmailDomains.First().Domain.Should().Be(domain);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void AddAllowedEmailDomain_WithInvalidDomain_ShouldThrowInvalidAllowedEmailDomainNameException(string invalidDomain)
    {
        // Arrange
        var university = new University("University", "Poland", "Mazowieckie", "Warsaw", "Street 123");

        // Act
        var act = () => university.AddAllowedEmailDomain(invalidDomain);

        // Assert
        act.Should().Throw<InvalidAllowedEmailDomainNameException>();
    }

    [Fact]
    public void AddAllowedEmailDomain_WithDuplicateDomain_ShouldThrowAllowedDomainAlreadyExistsException()
    {
        // Arrange
        var university = new University("University", "Poland", "Mazowieckie", "Warsaw", "Street 123");
        var domain = "example.com";
        university.AddAllowedEmailDomain(domain);

        // Act
        var act = () => university.AddAllowedEmailDomain(domain);

        // Assert
        act.Should().Throw<AllowedDomainAlreadyExistsException>();
    }

    [Fact]
    public void RemoveAllowedEmailDomain_WithValidId_ShouldRemoveDomain()
    {
        // Arrange
        var university = new University("University", "Poland", "Mazowieckie", "Warsaw", "Street 123");
        university.AddAllowedEmailDomain("example.com");
        var domainId = university.AllowedEmailDomains.First().Id;

        // Act
        university.RemoveAllowedEmailDomain(domainId);

        // Assert
        university.AllowedEmailDomains.Should().BeEmpty();
    }

    [Fact]
    public void RemoveAllowedEmailDomain_WithInvalidId_ShouldThrowAllowedDomainNotFoundException()
    {
        // Arrange
        var university = new University("University", "Poland", "Mazowieckie", "Warsaw", "Street 123");
        var invalidId = 999;

        // Act
        var act = () => university.RemoveAllowedEmailDomain(invalidId);

        // Assert
        act.Should().Throw<AllowedDomainNotFoundException>();
    }

    [Fact]
    public void RemoveDepartment_WithValidId_ShouldRemoveDepartment()
    {
        // Arrange
        var university = new University("University", "Poland", "Mazowieckie", "Warsaw", "Street 123");
        university.AddDepartment("Computer Science");
        var departmentId = university.Departments.First().Id;

        // Act
        university.RemoveDepartment(departmentId);

        // Assert
        university.Departments.Should().BeEmpty();
    }

    [Fact]
    public void RemoveDepartment_WithInvalidId_ShouldThrowDepartmentNotFoundException()
    {
        // Arrange
        var university = new University("University", "Poland", "Mazowieckie", "Warsaw", "Street 123");
        var invalidId = 999;

        // Act
        var act = () => university.RemoveDepartment(invalidId);

        // Assert
        act.Should().Throw<DepartmentNotFoundException>();
    }

    [Fact]
    public void RenameDepartment_WithValidParameters_ShouldRenameDepartment()
    {
        // Arrange
        var university = new University("University", "Poland", "Mazowieckie", "Warsaw", "Street 123");
        university.AddDepartment("Old Name");
        var departmentId = university.Departments.First().Id;
        var newName = "New Name";

        // Act
        university.RenameDepartment(departmentId, newName);

        // Assert
        university.Departments.First().Name.Should().Be(newName);
    }

    [Fact]
    public void RenameDepartment_WithInvalidId_ShouldThrowDepartmentNotFoundException()
    {
        // Arrange
        var university = new University("University", "Poland", "Mazowieckie", "Warsaw", "Street 123");
        var invalidId = 999;

        // Act
        var act = () => university.RenameDepartment(invalidId, "New Name");

        // Assert
        act.Should().Throw<DepartmentNotFoundException>();
    }

    [Fact]
    public void AddFieldOfStudy_WithValidParameters_ShouldAddFieldOfStudy()
    {
        // Arrange
        var university = new University("University", "Poland", "Mazowieckie", "Warsaw", "Street 123");
        university.AddDepartment("Computer Science");
        var departmentId = university.Departments.First().Id;
        var fieldOfStudyName = "Software Engineering";

        // Act
        university.AddFieldOfStudy(departmentId, fieldOfStudyName);

        // Assert
        university.Departments.First().FieldsOfStudy.Should().HaveCount(1);
        university.Departments.First().FieldsOfStudy.First().Name.Should().Be(fieldOfStudyName);
    }

    [Fact]
    public void AddFieldOfStudy_WithInvalidDepartmentId_ShouldThrowDepartmentNotFoundException()
    {
        // Arrange
        var university = new University("University", "Poland", "Mazowieckie", "Warsaw", "Street 123");
        var invalidId = 999;

        // Act
        var act = () => university.AddFieldOfStudy(invalidId, "Software Engineering");

        // Assert
        act.Should().Throw<DepartmentNotFoundException>();
    }

    [Fact]
    public void GetFieldOfStudyById_WithValidId_ShouldReturnFieldOfStudy()
    {
        // Arrange
        var university = new University("University", "Poland", "Mazowieckie", "Warsaw", "Street 123");
        university.AddDepartment("Computer Science");
        var departmentId = university.Departments.First().Id;
        university.AddFieldOfStudy(departmentId, "Software Engineering");
        var fieldOfStudyId = university.Departments.First().FieldsOfStudy.First().Id;

        // Act
        var result = university.GetFieldOfStudyById(fieldOfStudyId);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Software Engineering");
    }

    [Fact]
    public void GetFieldOfStudyById_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var university = new University("University", "Poland", "Mazowieckie", "Warsaw", "Street 123");
        var invalidId = 999;

        // Act
        var result = university.GetFieldOfStudyById(invalidId);

        // Assert
        result.Should().BeNull();
    }
}
