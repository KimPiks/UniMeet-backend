using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;

namespace UniMeet.UniversityModule.Tests.Domain.Aggregates;

public class UniversityTests
{
    [Fact]
    public void Rename_ShouldUpdateName()
    {
        var university = new University("OldName", "Poland", "Pomorskie", "Gdansk", "Some Address");
        university.Rename("NewName");
        Assert.Equal("NewName", university.Name);
    }
    
    [Fact]
    public void ChangeAddress_ShouldUpdateAddress()
    {
        var university = new University("UniMeet", "Poland", "Pomorskie", "Gdansk", "Old Address");
        university.ChangeAddress("New Address");
        Assert.Equal("New Address", university.Address);
    }
    
    [Fact]
    public void ChangeCity_ShouldUpdateCity()
    {
        var university = new University("UniMeet", "Poland", "Pomorskie", "Old City", "Some Address");
        university.ChangeCity("New City");
        Assert.Equal("New City", university.City);
    }
    
    [Fact]
    public void ChangeVoivodeship_ShouldUpdateVoivodeship()
    {
        var university = new University("UniMeet", "Poland", "Old Voivodeship", "Gdansk", "Some Address");
        university.ChangeVoivodeship("New Voivodeship");
        Assert.Equal("New Voivodeship", university.Voivodeship);
    }
    
    [Fact]
    public void ChangeCountry_ShouldUpdateCountry()
    {
        var university = new University("UniMeet", "Old Country", "Pomorskie", "Gdansk", "Some Address");
        university.ChangeCountry("New Country");
        Assert.Equal("New Country", university.Country);
    }
    
    [Fact]
    public void AddDepartment_ShouldAddDepartment()
    {
        var university = new University("UniMeet", "Poland", "Pomorskie", "Gdansk", "Some Address");
        var department = university.AddDepartment("Computer Science", university.Id);
        Assert.Contains(department, university.Departments);
    }

    [Fact]
    public void AddDepartment_DuplicateName_ShouldThrowException()
    {
        var university = new University("UniMeet", "Poland", "Pomorskie", "Gdansk", "Some Address");
        university.AddDepartment("Computer Science", university.Id);
        Assert.Throws<InvalidOperationException>(() => university.AddDepartment("Computer Science", university.Id));
    }
    
    [Fact]
    public void AddAllowedEmailDomain_ShouldAddDomain()
    {
        var university = new University("UniMeet", "Poland", "Pomorskie", "Gdansk", "Some Address");
        var domain = university.AddAllowedEmailDomain("unimeet.pl", university.Id);
        Assert.Contains(domain, university.AllowedEmailDomains);
    }

    [Fact]
    public void AddAllowedEmailDomain_DuplicateDomain_ShouldThrowException()
    {
        var university = new University("UniMeet", "Poland", "Pomorskie", "Gdansk", "Some Address");
        university.AddAllowedEmailDomain("unimeet.pl", university.Id);
        Assert.Throws<InvalidOperationException>(() => university.AddAllowedEmailDomain("unimeet.pl", university.Id));
    }
    
    [Fact]
    public void RemoveAllowedEmailDomain_ShouldRemoveDomain()
    {
        var university = new University("UniMeet", "Poland", "Pomorskie", "Gdansk", "Some Address");
        var domain = university.AddAllowedEmailDomain("unimeet.pl", university.Id);
        university.RemoveAllowedEmailDomain("unimeet.pl");
        Assert.DoesNotContain(domain, university.AllowedEmailDomains);
    }
    
    [Fact]
    public void RemoveAllowedEmailDomain_NonExistentDomain_ShouldThrowException()
    {
        var university = new University("UniMeet", "Poland", "Pomorskie", "Gdansk", "Some Address");
        Assert.Throws<InvalidOperationException>(() => university.RemoveAllowedEmailDomain("nonexistent.com"));
    }
    
    [Fact]
    public void ChangeDomain_ShouldUpdateDomain()
    {
        var university = new University("UniMeet", "Poland", "Pomorskie", "Gdansk", "Some Address");
        var domain = university.AddAllowedEmailDomain("unimeet.pl", university.Id);
        university.ChangeAllowedEmailDomain("unimeet.pl", "newdomain.com");
        Assert.Equal("newdomain.com", domain.Domain);
        Assert.Contains(domain, university.AllowedEmailDomains);
    }

    [Fact]
    public void ChangeDomain_NonExistentDomain_ShouldThrowException()
    {
        var university = new University("UniMeet", "Poland", "Pomorskie", "Gdansk", "Some Address");
        Assert.Throws<InvalidOperationException>(() => university.ChangeAllowedEmailDomain("nonexistent.com", "newdomain.com"));
    }
    
    [Fact]
    public void ChangeDomain_DuplicateNewDomain_ShouldThrowException()
    {
        var university = new University("UniMeet", "Poland", "Pomorskie", "Gdansk", "Some Address");
        university.AddAllowedEmailDomain("unimeet.pl", university.Id);
        university.AddAllowedEmailDomain("existing.com", university.Id);
        Assert.Throws<InvalidOperationException>(() => university.ChangeAllowedEmailDomain("unimeet.pl", "existing.com"));
    }
    
    [Fact]
    public void ChangeDomain_ShouldChangeDomainInUniversity()
    {
        var university = new University("UniMeet", "Poland", "Pomorskie", "Gdansk", "Some Address");
        university.AddAllowedEmailDomain("unimeet.pl", university.Id);
        university.ChangeAllowedEmailDomain("unimeet.pl", "newdomain.com");
        Assert.Contains(university.AllowedEmailDomains, d => d.Domain == "newdomain.com");
        Assert.DoesNotContain(university.AllowedEmailDomains, d => d.Domain == "unimeet.pl");
    }
    
    [Fact]
    public void RemoveDepartment_ShouldRemoveDepartment()
    {
        var university = new University("UniMeet", "Poland", "Pomorskie", "Gdansk", "Some Address");
        var department = university.AddDepartment("Computer Science", university.Id);
        university.RemoveDepartment("Computer Science");
        Assert.DoesNotContain(department, university.Departments);
    }
    
    [Fact]
    public void RemoveDepartment_NonExistentDepartment_ShouldThrowException()
    {
        var university = new University("UniMeet", "Poland", "Pomorskie", "Gdansk", "Some Address");
        Assert.Throws<InvalidOperationException>(() => university.RemoveDepartment("NonExistentDept"));
    }
    
    [Fact]
    public void RenameDepartment_ShouldUpdateDepartmentName()
    {
        var university = new University("UniMeet", "Poland", "Pomorskie", "Gdansk", "Some Address");
        var department = university.AddDepartment("Computer Science", university.Id);
        university.RenameDepartment("Computer Science", "Informatics");
        Assert.Equal("Informatics", department.Name);
        Assert.Contains(department, university.Departments);
    }
    
    [Fact]
    public void RenameDepartment_NonExistentDepartment_ShouldThrowException()
    {
        var university = new University("UniMeet", "Poland", "Pomorskie", "Gdansk", "Some Address");
        Assert.Throws<InvalidOperationException>(() => university.RenameDepartment("NonExistentDept", "NewName"));
    }
    
    [Fact]
    public void RenameDepartment_DuplicateName_ShouldThrowException()
    {
        var university = new University("UniMeet", "Poland", "Pomorskie", "Gdansk", "Some Address");
        university.AddDepartment("Computer Science", university.Id);
        university.AddDepartment("Informatics", university.Id);
        Assert.Throws<InvalidOperationException>(() => university.RenameDepartment("Computer Science", "Informatics"));
    }
    
    [Fact]
    public void AddFieldOfStudyToDepartment_ShouldAddFieldOfStudy()
    {
        var university = new University("UniMeet", "Poland", "Pomorskie", "Gdansk", "Some Address");
        var department = university.AddDepartment("Computer Science", university.Id);
        var fieldOfStudy = university.AddFieldOfStudyToDepartment("Computer Science", "Software Engineering");
        Assert.Contains(fieldOfStudy, department.FieldsOfStudy);
    }

    [Fact]
    public void AddFieldOfStudyToDepartment_NonExistentDepartment_ShouldThrowException()
    {
        var university = new University("UniMeet", "Poland", "Pomorskie", "Gdansk", "Some Address");
        Assert.Throws<InvalidOperationException>(() => university.AddFieldOfStudyToDepartment("NonExistentDept", "Software Engineering"));
    }

    [Fact]
    public void AddFieldOfStudyToDepartment_DuplicateFieldOfStudy_ShouldThrowException()
    {
        var university = new University("UniMeet", "Poland", "Pomorskie", "Gdansk", "Some Address");
        university.AddDepartment("Computer Science", university.Id);
        university.AddFieldOfStudyToDepartment("Computer Science", "Software Engineering");
        Assert.Throws<InvalidOperationException>(() => university.AddFieldOfStudyToDepartment("Computer Science", "Software Engineering"));
    }

    [Fact]
    public void RemoveFieldOfStudyFromDepartment_ShouldRemoveFieldOfStudy()
    {
        var university = new University("UniMeet", "Poland", "Pomorskie", "Gdansk", "Some Address");
        var department = university.AddDepartment("Computer Science", university.Id);
        var fieldOfStudy = university.AddFieldOfStudyToDepartment("Computer Science", "Software Engineering");
        university.RemoveFieldOfStudyFromDepartment("Computer Science", "Software Engineering");
        Assert.DoesNotContain(fieldOfStudy, department.FieldsOfStudy);
    }

    [Fact]
    public void RemoveFieldOfStudyFromDepartment_NonExistentDepartment_ShouldThrowException()
    {
        var university = new University("UniMeet", "Poland", "Pomorskie", "Gdansk", "Some Address");
        Assert.Throws<InvalidOperationException>(() => university.RemoveFieldOfStudyFromDepartment("NonExistentDept", "Software Engineering"));
    }

    [Fact]
    public void RemoveFieldOfStudyFromDepartment_NonExistentFieldOfStudy_ShouldThrowException()
    {
        var university = new University("UniMeet", "Poland", "Pomorskie", "Gdansk", "Some Address");
        university.AddDepartment("Computer Science", university.Id);
        Assert.Throws<InvalidOperationException>(() => university.RemoveFieldOfStudyFromDepartment("Computer Science", "NonExistentField"));
    }

    [Fact]
    public void RenameFieldOfStudyInDepartment_ShouldUpdateFieldOfStudyName()
    {
        var university = new University("UniMeet", "Poland", "Pomorskie", "Gdansk", "Some Address");
        var department = university.AddDepartment("Computer Science", university.Id);
        var fieldOfStudy = university.AddFieldOfStudyToDepartment("Computer Science", "Software Engineering");
        university.RenameFieldOfStudyInDepartment("Computer Science", "Software Engineering", "Computer Engineering");
        Assert.Equal("Computer Engineering", fieldOfStudy.Name);
        Assert.Contains(fieldOfStudy, department.FieldsOfStudy);
    }

    [Fact]
    public void RenameFieldOfStudyInDepartment_NonExistentDepartment_ShouldThrowException()
    {
        var university = new University("UniMeet", "Poland", "Pomorskie", "Gdansk", "Some Address");
        Assert.Throws<InvalidOperationException>(() => university.RenameFieldOfStudyInDepartment("NonExistentDept", "Software Engineering", "Computer Engineering"));
    }

    [Fact]
    public void RenameFieldOfStudyInDepartment_NonExistentFieldOfStudy_ShouldThrowException()
    {
        var university = new University("UniMeet", "Poland", "Pomorskie", "Gdansk", "Some Address");
        university.AddDepartment("Computer Science", university.Id);
        Assert.Throws<InvalidOperationException>(() => university.RenameFieldOfStudyInDepartment("Computer Science", "NonExistentField", "Computer Engineering"));
    }
}