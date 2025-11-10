using FluentAssertions;
using System;
using System.Linq;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Tests.Domain.Aggregates;

public class UniversityTests
{
    [Theory]
    [InlineData(null, "Country", "Voivodeship", "City", "Address")]
    [InlineData("", "Country", "Voivodeship", "City", "Address")]
    [InlineData("Name", null, "Voivodeship", "City", "Address")]
    [InlineData("Name", "", "Voivodeship", "City", "Address")]
    [InlineData("Name", "Country", null, "City", "Address")]
    [InlineData("Name", "Country", "", "City", "Address")]
    [InlineData("Name", "Country", "Voivodeship", null, "Address")]
    [InlineData("Name", "Country", "Voivodeship", "", "Address")]
    [InlineData("Name", "Country", "Voivodeship", "City", null)]
    [InlineData("Name", "Country", "Voivodeship", "City", "")]
    public void Ctor_InvalidArguments_ThrowsArgumentException(string name, string country, string voivodeship, string city, string address)
    {
        Action act = () => new University(name!, country!, voivodeship!, city!, address!);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Ctor_ValidArguments_SetsProperties()
    {
        var uni = new University("Uni", "Poland", "Mazowieckie", "Warsaw", "Main St 1");

        uni.Name.Should().Be("Uni");
        uni.Country.Should().Be("Poland");
        uni.Voivodeship.Should().Be("Mazowieckie");
        uni.City.Should().Be("Warsaw");
        uni.Address.Should().Be("Main St 1");
        uni.Departments.Should().BeEmpty();
        uni.AllowedEmailDomains.Should().BeEmpty();
    }

    [Fact]
    public void Rename_UpdatesName()
    {
        var uni = new University("Old", "C", "V", "City", "Addr");

        uni.Rename("New");

        uni.Name.Should().Be("New");
    }

    [Fact]
    public void ChangeAddressCityVoivodeshipCountry_UpdateValues()
    {
        var uni = new University("U", "C1", "V1", "City1", "Addr1");

        uni.ChangeAddress("Addr2");
        uni.ChangeCity("City2");
        uni.ChangeVoivodeship("V2");
        uni.ChangeCountry("C2");

        uni.Address.Should().Be("Addr2");
        uni.City.Should().Be("City2");
        uni.Voivodeship.Should().Be("V2");
        uni.Country.Should().Be("C2");
    }

    [Fact]
    public void AddDepartment_AddsDepartmentAndPreventsDuplicates()
    {
        var uni = new University("U", "C", "V", "City", "Addr");

        uni.AddDepartment("Computer Science");

        uni.Departments.Should().ContainSingle(d => d.Name == "Computer Science");

        Action act = () => uni.AddDepartment("Computer Science");
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void RemoveDepartment_RemovesExistingAndThrowsWhenNotFound()
    {
        var uni = new University("U", "C", "V", "City", "Addr");
        uni.AddDepartment("Math");
        var department = uni.Departments.First();
        var id = department.Id;

        uni.RemoveDepartment(id);
        uni.Departments.Should().BeEmpty();

        Action act = () => uni.RemoveDepartment(999);
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void RenameDepartment_ChangesNameAndPreventsDuplicateNames()
    {
        var uni = new University("U", "C", "V", "City", "Addr");
        uni.AddDepartment("A");
        uni.AddDepartment("B");

        var depA = uni.Departments.First(d => d.Name == "A");
        uni.RenameDepartment(depA.Id, "A1");
        depA.Name.Should().Be("A1");

        var depB = uni.Departments.First(d => d.Name == "B");
        Action act = () => uni.RenameDepartment(depB.Id, "A1");
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void AddFieldOfStudy_AddsFieldAndPreventsDuplicates()
    {
        var uni = new University("U", "C", "V", "City", "Addr");
        uni.AddDepartment("Physics");
        var dep = uni.Departments.First();

        uni.AddFieldOfStudy(dep.Id, "Quantum Mechanics");
        dep.FieldsOfStudy.Should().ContainSingle(f => f.Name == "Quantum Mechanics");

        Action act = () => uni.AddFieldOfStudy(dep.Id, "Quantum Mechanics");
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void RemoveFieldOfStudy_RemovesExistingAndThrowsWhenNotFound()
    {
        var uni = new University("U", "C", "V", "City", "Addr");
        uni.AddDepartment("Chemistry");
        var dep = uni.Departments.First();
        uni.AddFieldOfStudy(dep.Id, "Organic");
        var field = dep.FieldsOfStudy.First();

        uni.RemoveFieldOfStudy(field.Id);
        dep.FieldsOfStudy.Should().BeEmpty();

        Action act = () => uni.RemoveFieldOfStudy(999);
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void RenameFieldOfStudy_ChangesNameAndPreventsDuplicates()
    {
        var uni = new University("U", "C", "V", "City", "Addr");
        uni.AddDepartment("Bio");
        var dep = uni.Departments.First();
        uni.AddFieldOfStudy(dep.Id, "Genetics");
        uni.AddFieldOfStudy(dep.Id, "Ecology");

        var gen = dep.FieldsOfStudy.First(f => f.Name == "Genetics");
        uni.RenameFieldOfStudy(gen.Id, "Genetics II");
        gen.Name.Should().Be("Genetics II");

        var eco = dep.FieldsOfStudy.First(f => f.Name == "Ecology");
        Action act = () => uni.RenameFieldOfStudy(eco.Id, "Genetics II");
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetFieldOfStudyById_ReturnsCorrectFieldOrNull()
    {
        var uni = new University("U", "C", "V", "City", "Addr");
        uni.AddDepartment("Law");
        var dep = uni.Departments.First();
        uni.AddFieldOfStudy(dep.Id, "Criminal Law");
        var field = dep.FieldsOfStudy.First();

        var found = uni.GetFieldOfStudyById(field.Id);
        found.Should().NotBeNull().And.BeSameAs(field);

        uni.GetFieldOfStudyById(999).Should().BeNull();
    }

    [Fact]
    public void AllowedEmailDomains_AddChangeRemoveAndPreventDuplicates()
    {
        var uni = new University("U", "C", "V", "City", "Addr");

        uni.AddAllowedEmailDomain("uni.edu");
        uni.AllowedEmailDomains.Should().ContainSingle(d => d.Domain == "uni.edu");

        Action actDuplicate = () => uni.AddAllowedEmailDomain("uni.edu");
        actDuplicate.Should().Throw<InvalidOperationException>();

        var domain = uni.AllowedEmailDomains.First();
        uni.ChangeAllowedEmailDomain(domain.Id, "university.edu");
        domain.Domain.Should().Be("university.edu");

        uni.AddAllowedEmailDomain("other.edu");
        Action actChangeToExisting = () => uni.ChangeAllowedEmailDomain(domain.Id, "other.edu");
        actChangeToExisting.Should().Throw<InvalidOperationException>();

        uni.RemoveAllowedEmailDomain(domain.Id);
        uni.AllowedEmailDomains.Should().NotContain(d => d.Id == domain.Id);

        Action actRemoveNonExisting = () => uni.RemoveAllowedEmailDomain(999);
        actRemoveNonExisting.Should().Throw<InvalidOperationException>();
    }
}