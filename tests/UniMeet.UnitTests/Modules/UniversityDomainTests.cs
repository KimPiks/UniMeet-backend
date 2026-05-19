using UniMeet.UniversityModule.Domain.Universities;
using UniMeet.UniversityModule.Domain.Universities.Exceptions;

namespace UniMeet.UnitTests.Modules;

public class UniversityDomainTests
{
    [Fact]
    public void Constructor_rejects_blank_name()
    {
        Assert.Throws<InvalidUniversityNameException>(() =>
            new University(" ", "Poland", "Mazowieckie", "Warsaw", "Main 1"));
    }

    [Fact]
    public void AddDepartment_rejects_duplicate_department_name()
    {
        var university = CreateUniversity();

        university.AddDepartment("Computer Science");

        Assert.Throws<DepartmentAlreadyExistsException>(() => university.AddDepartment("Computer Science"));
    }

    [Fact]
    public void Add_and_rename_field_of_study_updates_department()
    {
        var university = CreateUniversity();
        university.AddDepartment("Engineering");
        var department = university.Departments.Single();
        department.Id = 15;

        university.AddFieldOfStudy(15, "Robotics");
        var field = department.FieldsOfStudy.Single();
        field.Id = 77;
        university.RenameFieldOfStudy(77, "Automation");

        Assert.Equal("Automation", field.Name);
        Assert.Same(field, university.GetFieldOfStudyById(77));
    }

    [Fact]
    public void RemoveFieldOfStudy_throws_when_field_is_missing()
    {
        var university = CreateUniversity();
        university.AddDepartment("Engineering");
        university.Departments.Single().Id = 15;

        Assert.Throws<FieldOfStudyNotFoundException>(() => university.RemoveFieldOfStudy(404));
    }

    [Fact]
    public void ChangeAllowedEmailDomain_rejects_duplicate_domain()
    {
        var university = CreateUniversity();
        university.AddAllowedEmailDomain("uni.edu");
        university.AddAllowedEmailDomain("students.uni.edu");
        var secondDomain = university.AllowedEmailDomains.Last();
        secondDomain.Id = 2;

        Assert.Throws<AllowedDomainAlreadyExistsException>(() =>
            university.ChangeAllowedEmailDomain(2, "uni.edu"));
    }

    private static University CreateUniversity()
        => new("Uni", "Poland", "Mazowieckie", "Warsaw", "Main 1");
}
