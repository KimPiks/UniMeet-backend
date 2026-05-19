using UniMeet.UniversityModule.Domain.Universities;
using UniMeet.UniversityModule.Infrastructure.Universities;

namespace UniMeet.UnitTests.Infrastructure;

public class UniversityRepositoryTests
{
    [Fact]
    public async Task UniversityRepository_adds_loads_nested_graph_and_deletes_university()
    {
        await using var context = RepositoryTestContextFactory.CreateUniversityContext();
        var repository = new UniversityRepository(context);
        var university = CreateUniversityWithGraph();

        await repository.AddAsync(university);
        await repository.SaveChangesAsync();
        var byId = await repository.GetByIdAsync(university.Id);
        var byDomainId = await repository.GetByAllowedDomainIdAsync(university.AllowedEmailDomains.Single().Id);
        var byEmail = await repository.GetByAllowedEmailAsync("uni.edu");
        var byDepartment = await repository.GetByDepartmentIdAsync(university.Departments.Single().Id);
        var byField = await repository.GetByFieldOfStudyIdAsync(university.Departments.Single().FieldsOfStudy.Single().Id);
        var all = (await repository.GetAllAsync(0, 10)).Single();
        repository.Delete(all);
        await repository.SaveChangesAsync();

        Assert.NotNull(byId);
        Assert.Single(byId.Departments);
        Assert.Single(byId.AllowedEmailDomains);
        Assert.Same(university, byDomainId);
        Assert.Same(university, byEmail);
        Assert.Same(university, byDepartment);
        Assert.Same(university, byField);
        Assert.Empty(await repository.GetAllAsync(0, 10));
    }

    private static University CreateUniversityWithGraph()
    {
        var university = new University("Uni", "Poland", "Mazowieckie", "Warsaw", "Main 1");
        university.AddAllowedEmailDomain("uni.edu");
        university.AddDepartment("Engineering");
        var department = university.Departments.Single();
        department.Id = 10;
        university.AddFieldOfStudy(10, "Robotics");
        return university;
    }
}
