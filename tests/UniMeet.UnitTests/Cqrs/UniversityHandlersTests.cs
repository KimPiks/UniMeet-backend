using ModularSystem.Contracts.University.AllowedEmailDomains.AddAllowedEmailDomain;
using ModularSystem.Contracts.University.AllowedEmailDomains.DeleteAllowedEmailDomain;
using ModularSystem.Contracts.University.AllowedEmailDomains.GetAllowedEmailDomainById;
using ModularSystem.Contracts.University.AllowedEmailDomains.GetAllowedEmailDomainsByUniversityId;
using ModularSystem.Contracts.University.AllowedEmailDomains.UpdateAllowedEmailDomain;
using ModularSystem.Contracts.University.Departments.AddDepartment;
using ModularSystem.Contracts.University.Departments.DeleteDepartment;
using ModularSystem.Contracts.University.Departments.GetDepartmentById;
using ModularSystem.Contracts.University.Departments.GetDepartmentsByUniversityId;
using ModularSystem.Contracts.University.Departments.UpdateDepartment;
using ModularSystem.Contracts.University.FieldsOfStudy.AddFieldOfStudy;
using ModularSystem.Contracts.University.FieldsOfStudy.DeleteFieldOfStudy;
using ModularSystem.Contracts.University.FieldsOfStudy.GetFieldOfStudyById;
using ModularSystem.Contracts.University.FieldsOfStudy.GetFieldsOfStudyByDepartmentId;
using ModularSystem.Contracts.University.FieldsOfStudy.UpdateFieldOfStudy;
using ModularSystem.Contracts.University.Universities.CreateUniversity;
using ModularSystem.Contracts.University.Universities.DeleteUniversity;
using ModularSystem.Contracts.University.Universities.GetAllUniversities;
using ModularSystem.Contracts.University.Universities.GetByAllowedDomain;
using ModularSystem.Contracts.University.Universities.GetUniversityById;
using ModularSystem.Contracts.University.Universities.UpdateUniversity;
using ModularSystem.Contracts.University;
using UniMeet.UniversityModule.Application.AllowedEmailDomains.DeleteAllowedEmailDomain;
using UniMeet.UniversityModule.Application.AllowedEmailDomains.GetAllowedEmailDomainById;
using UniMeet.UniversityModule.Application.AllowedEmailDomains.GetAllowedEmailDomainsByUniversityId;
using UniMeet.UniversityModule.Application.AllowedEmailDomains.UpdateAllowedEmailDomain;
using UniMeet.UniversityModule.Application.Departments.DeleteDepartment;
using UniMeet.UniversityModule.Application.Departments.GetDepartmentById;
using UniMeet.UniversityModule.Application.Departments.GetDepartmentsByUniversityId;
using UniMeet.UniversityModule.Application.Departments.UpdateDepartment;
using UniMeet.UniversityModule.Application.FieldsOfStudy.DeleteFieldOfStudy;
using UniMeet.UniversityModule.Application.FieldsOfStudy.GetFieldOfStudyById;
using UniMeet.UniversityModule.Application.FieldsOfStudy.GetFieldsOfStudyByDepartmentId;
using UniMeet.UniversityModule.Application.FieldsOfStudy.UpdateFieldOfStudy;
using UniMeet.Shared.Exceptions;
using UniMeet.UniversityModule.Application.AllowedEmailDomains.AddAllowedEmailDomain;
using UniMeet.UniversityModule.Application.Departments.AddDepartment;
using UniMeet.UniversityModule.Application.FieldsOfStudy.AddFieldOfStudy;
using UniMeet.UniversityModule.Application.Universities.CreateUniversity;
using UniMeet.UniversityModule.Application.Universities.DeleteUniversity;
using UniMeet.UniversityModule.Application.Universities.GetAllUniversities;
using UniMeet.UniversityModule.Application.Universities.GetByAllowedDomain;
using UniMeet.UniversityModule.Application.Universities.GetUniversityByAllowedDomain;
using UniMeet.UniversityModule.Application.Universities.GetUniversityById;
using UniMeet.UniversityModule.Application.Universities.UpdateUniversity;
using UniMeet.UniversityModule.Domain.Universities;
using UniMeet.UniversityModule.Infrastructure.Universities;

namespace UniMeet.UnitTests.Cqrs;

public class UniversityHandlersTests
{
    [Fact]
    public async Task CreateUniversityCommandHandler_validates_adds_and_saves_university()
    {
        await using var context = RepositoryTestContextFactory.CreateUniversityContext();
        var repository = new UniversityRepository(context);
        var handler = new CreateUniversityCommandHandler(repository);

        await handler.HandleAsync(new CreateUniversityCommand("Uni", "Poland", "Mazowieckie", "Warsaw", "Main 1"), CancellationToken.None);

        var university = Assert.Single(context.Universities);
        Assert.Equal("Uni", university.Name);
    }

    [Fact]
    public async Task CreateUniversityCommandHandler_rejects_invalid_command()
    {
        await using var context = RepositoryTestContextFactory.CreateUniversityContext();
        var handler = new CreateUniversityCommandHandler(new UniversityRepository(context));

        await Assert.ThrowsAsync<ValidationException>(() =>
            handler.HandleAsync(new CreateUniversityCommand("", "Poland", "Mazowieckie", "Warsaw", "Main 1"), CancellationToken.None));

        Assert.Empty(context.Universities);
    }

    [Fact]
    public async Task University_child_command_handlers_modify_aggregate()
    {
        await using var context = RepositoryTestContextFactory.CreateUniversityContext();
        var repository = new UniversityRepository(context);
        var university = new University("Uni", "Poland", "Mazowieckie", "Warsaw", "Main 1");
        await repository.AddAsync(university);
        await repository.SaveChangesAsync();

        await new AddDepartmentCommandHandler(repository)
            .HandleAsync(new AddDepartmentCommand(university.Id, "Engineering"), CancellationToken.None);
        var departmentId = university.Departments.Single().Id;
        await new AddFieldOfStudyCommandHandler(repository)
            .HandleAsync(new AddFieldOfStudyCommand(departmentId, "Robotics"));
        await new AddAllowedEmailDomainCommandHandler(repository)
            .HandleAsync(new AddAllowedEmailDomainCommand(university.Id, "uni.edu"), CancellationToken.None);

        Assert.Single(university.Departments);
        Assert.Single(university.Departments.Single().FieldsOfStudy);
        Assert.Single(university.AllowedEmailDomains);
    }

    [Fact]
    public async Task GetUniversityByIdQueryHandler_returns_mapped_university_or_null()
    {
        await using var context = RepositoryTestContextFactory.CreateUniversityContext();
        var repository = new UniversityRepository(context);
        var university = new University("Uni", "Poland", "Mazowieckie", "Warsaw", "Main 1");
        await repository.AddAsync(university);
        await repository.SaveChangesAsync();
        var handler = new GetUniversityByIdQueryHandler(repository);

        var existing = await handler.HandleAsync(new GetUniversityByIdQuery(university.Id), CancellationToken.None);
        var missing = await handler.HandleAsync(new GetUniversityByIdQuery(999), CancellationToken.None);

        Assert.NotNull(existing);
        Assert.Equal(university.Id, existing.Id);
        Assert.Null(missing);
    }

    [Fact]
    public async Task University_command_handlers_update_and_delete_existing_aggregate()
    {
        await using var context = RepositoryTestContextFactory.CreateUniversityContext();
        var (repository, university) = await SeedUniversityAsync(context);
        var handler = new UpdateUniversityCommandHandler(repository);

        await handler.HandleAsync(new UpdateUniversityCommand(
            university.Id,
            "Updated",
            "Germany",
            "Bavaria",
            "Munich",
            "New 2"), CancellationToken.None);
        await new DeleteUniversityCommandHandler(repository)
            .HandleAsync(new DeleteUniversityCommand(university.Id), CancellationToken.None);

        Assert.Empty(context.Universities);
    }

    [Fact]
    public async Task University_query_handlers_return_paged_and_lookup_results()
    {
        await using var context = RepositoryTestContextFactory.CreateUniversityContext();
        var (repository, university) = await SeedUniversityAsync(context);
        var second = new University("Second", "Poland", "Slaskie", "Gliwice", "Side 2");
        await repository.AddAsync(second);
        await repository.SaveChangesAsync();

        var all = (await new GetAllUniversitiesQueryHandler(repository)
            .HandleAsync(new GetAllUniversitiesQuery(1, 1), CancellationToken.None)).ToList();
        var byAllowedDomain = await new GetByAllowedDomainQueryHandler(repository)
            .HandleAsync(new GetByAllowedDomainQuery("uni.edu"), CancellationToken.None);
        var lookup = await new GetUniversityByAllowedDomainQueryHandler(repository)
            .HandleAsync(new GetUniversityByAllowedDomainQuery("uni.edu"), CancellationToken.None);

        Assert.Single(all);
        Assert.Equal(second.Id, all[0].Id);
        Assert.NotNull(byAllowedDomain);
        Assert.Equal(university.Id, byAllowedDomain.Id);
        Assert.Equal(new UniversityLookupDto(university.Id, university.Name), lookup);
    }

    [Fact]
    public async Task Department_handlers_get_update_and_delete_child_entity()
    {
        await using var context = RepositoryTestContextFactory.CreateUniversityContext();
        var (repository, university) = await SeedUniversityAsync(context);
        var departmentId = university.Departments.Single().Id;

        var byId = await new GetDepartmentByIdQueryHandler(repository)
            .HandleAsync(new GetDepartmentByIdQuery(departmentId), CancellationToken.None);
        var paged = (await new GetDepartmentsByUniversityIdQueryHandler(repository)
            .HandleAsync(new GetDepartmentsByUniversityIdQuery(university.Id, 0, 10), CancellationToken.None)).ToList();
        await new UpdateDepartmentCommandHandler(repository)
            .HandleAsync(new UpdateDepartmentCommand(departmentId, "Science"), CancellationToken.None);
        await new DeleteDepartmentCommandHandler(repository)
            .HandleAsync(new DeleteDepartmentCommand(departmentId), CancellationToken.None);

        Assert.NotNull(byId);
        Assert.Equal("Engineering", byId.Name);
        Assert.Single(paged);
        Assert.Empty(university.Departments);
    }

    [Fact]
    public async Task Field_of_study_handlers_get_update_and_delete_child_entity()
    {
        await using var context = RepositoryTestContextFactory.CreateUniversityContext();
        var (repository, university) = await SeedUniversityAsync(context);
        var departmentId = university.Departments.Single().Id;
        var fieldOfStudyId = university.Departments.Single().FieldsOfStudy.Single().Id;

        var byId = await new GetFieldOfStudyByIdQueryHandler(repository)
            .HandleAsync(new GetFieldOfStudyByIdQuery(fieldOfStudyId), CancellationToken.None);
        var paged = (await new GetFieldsOfStudyByDepartmentIdQueryHandler(repository)
            .HandleAsync(new GetFieldsOfStudyByDepartmentIdQuery(departmentId, 0, 10), CancellationToken.None)).ToList();
        await new UpdateFieldOfStudyCommandHandler(repository)
            .HandleAsync(new UpdateFieldOfStudyCommand(fieldOfStudyId, "Automation"), CancellationToken.None);
        await new DeleteFieldOfStudyCommandHandler(repository)
            .HandleAsync(new DeleteFieldOfStudyCommand(fieldOfStudyId), CancellationToken.None);

        Assert.NotNull(byId);
        Assert.Equal("Robotics", byId.Name);
        Assert.Single(paged);
        Assert.Empty(university.Departments.Single().FieldsOfStudy);
    }

    [Fact]
    public async Task Allowed_email_domain_handlers_get_update_and_delete_child_entity()
    {
        await using var context = RepositoryTestContextFactory.CreateUniversityContext();
        var (repository, university) = await SeedUniversityAsync(context);
        var domainId = university.AllowedEmailDomains.Single().Id;

        var byId = await new GetAllowedEmailDomainByIdQueryHandler(repository)
            .HandleAsync(new GetAllowedEmailDomainByIdQuery(domainId), CancellationToken.None);
        var paged = (await new GetAllowedEmailDomainsByUniversityIdQueryHandler(repository)
            .HandleAsync(new GetAllowedEmailDomainsByUniversityIdQuery(university.Id, 0, 10), CancellationToken.None)).ToList();
        await new UpdateAllowedEmailDomainCommandHandler(repository)
            .HandleAsync(new UpdateAllowedEmailDomainCommand(domainId, "student.uni.edu"), CancellationToken.None);
        await new DeleteAllowedEmailDomainCommandHandler(repository)
            .HandleAsync(new DeleteAllowedEmailDomainCommand(domainId), CancellationToken.None);

        Assert.NotNull(byId);
        Assert.Equal("uni.edu", byId.Domain);
        Assert.Single(paged);
        Assert.Empty(university.AllowedEmailDomains);
    }

    private static async Task<(UniversityRepository Repository, University University)> SeedUniversityAsync(
        UniMeet.UniversityModule.Infrastructure.UniversityContext context)
    {
        var repository = new UniversityRepository(context);
        var university = new University("Uni", "Poland", "Mazowieckie", "Warsaw", "Main 1");
        await repository.AddAsync(university);
        await repository.SaveChangesAsync();

        university.AddDepartment("Engineering");
        await repository.SaveChangesAsync();

        var departmentId = university.Departments.Single().Id;
        university.AddFieldOfStudy(departmentId, "Robotics");
        university.AddAllowedEmailDomain("uni.edu");
        await repository.SaveChangesAsync();

        return (repository, university);
    }
}
