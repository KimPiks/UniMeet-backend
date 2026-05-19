using ModularSystem.Contracts.University.AllowedEmailDomains;
using ModularSystem.Contracts.University.AllowedEmailDomains.AddAllowedEmailDomain;
using ModularSystem.Contracts.University.AllowedEmailDomains.DeleteAllowedEmailDomain;
using ModularSystem.Contracts.University.AllowedEmailDomains.GetAllowedEmailDomainById;
using ModularSystem.Contracts.University.AllowedEmailDomains.GetAllowedEmailDomainsByUniversityId;
using ModularSystem.Contracts.University.AllowedEmailDomains.UpdateAllowedEmailDomain;
using ModularSystem.Contracts.University.Departments;
using ModularSystem.Contracts.University.Departments.AddDepartment;
using ModularSystem.Contracts.University.Departments.DeleteDepartment;
using ModularSystem.Contracts.University.Departments.GetDepartmentById;
using ModularSystem.Contracts.University.Departments.GetDepartmentsByUniversityId;
using ModularSystem.Contracts.University.Departments.UpdateDepartment;
using ModularSystem.Contracts.University.FieldsOfStudy;
using ModularSystem.Contracts.University.FieldsOfStudy.AddFieldOfStudy;
using ModularSystem.Contracts.University.FieldsOfStudy.DeleteFieldOfStudy;
using ModularSystem.Contracts.University.FieldsOfStudy.GetFieldOfStudyById;
using ModularSystem.Contracts.University.FieldsOfStudy.GetFieldsOfStudyByDepartmentId;
using ModularSystem.Contracts.University.FieldsOfStudy.UpdateFieldOfStudy;
using ModularSystem.Contracts.University.Universities;
using ModularSystem.Contracts.University.Universities.CreateUniversity;
using ModularSystem.Contracts.University.Universities.DeleteUniversity;
using ModularSystem.Contracts.University.Universities.GetAllUniversities;
using ModularSystem.Contracts.University.Universities.GetUniversityById;
using ModularSystem.Contracts.University.Universities.UpdateUniversity;
using UniMeet.API.Controllers.University;
using UniMeet.API.Models.Requests;

namespace UniMeet.UnitTests.Api;

public class UniversityControllersTests
{
    [Fact]
    public async Task UniversitiesController_query_endpoints_return_payloads_and_forward_queries()
    {
        var dispatcher = new FakeModuleRequestDispatcher();
        var university = CreateUniversityDto();
        var domains = new[] { new AllowedEmailDomainDto { Id = 7, Domain = "uni.edu" } };
        var departments = new[] { new DepartmentDto { Id = 9, Name = "Engineering", FieldsOfStudy = [] } };
        dispatcher.QueueResult<UniversityDto?>(university);
        dispatcher.QueueResult<IEnumerable<UniversityDto>>([university]);
        dispatcher.QueueResult<IEnumerable<AllowedEmailDomainDto>>(domains);
        dispatcher.QueueResult<IEnumerable<DepartmentDto>>(departments);
        var controller = new UniversitiesController(dispatcher);

        var byId = ControllerTestHelpers.AssertOkResponse<UniversityDto?>(
            await controller.GetUniversityById(5),
            "University retrieved successfully");
        var all = ControllerTestHelpers.AssertOkResponse<IEnumerable<UniversityDto>>(
            await controller.GetAllUniversities(1, 20),
            "Universities retrieved successfully");
        var allowedDomains = ControllerTestHelpers.AssertOkResponse<IEnumerable<AllowedEmailDomainDto>>(
            await controller.GetAllowedEmailDomains(5, 2, 10),
            "Allowed email domains retrieved successfully");
        var allDepartments = ControllerTestHelpers.AssertOkResponse<IEnumerable<DepartmentDto>>(
            await controller.GetAllDepartments(5, 3, 15),
            "Departments retrieved successfully");

        Assert.Same(university, byId.Data);
        Assert.Same(university, Assert.Single(all.Data));
        Assert.Same(domains[0], Assert.Single(allowedDomains.Data));
        Assert.Same(departments[0], Assert.Single(allDepartments.Data));
        Assert.Collection(
            dispatcher.SentRequests,
            request =>
            {
                var query = Assert.IsType<GetUniversityByIdQuery>(request);
                Assert.Equal(5, query.UniversityId);
            },
            request =>
            {
                var query = Assert.IsType<GetAllUniversitiesQuery>(request);
                Assert.Equal(1, query.Offset);
                Assert.Equal(20, query.Limit);
            },
            request =>
            {
                var query = Assert.IsType<GetAllowedEmailDomainsByUniversityIdQuery>(request);
                Assert.Equal(5, query.UniversityId);
                Assert.Equal(2, query.Offset);
                Assert.Equal(10, query.Limit);
            },
            request =>
            {
                var query = Assert.IsType<GetDepartmentsByUniversityIdQuery>(request);
                Assert.Equal(5, query.UniversityId);
                Assert.Equal(3, query.Offset);
                Assert.Equal(15, query.Limit);
            });
    }

    [Fact]
    public async Task UniversitiesController_command_endpoints_forward_commands()
    {
        var dispatcher = new FakeModuleRequestDispatcher();
        var controller = new UniversitiesController(dispatcher);

        var created = ControllerTestHelpers.AssertOkResponse<object>(
            await controller.CreateUniversity(new UniversityCreateRequest("Uni", "Poland", "Mazowieckie", "Warsaw", "Main 1")),
            "University created successfully");
        var updated = ControllerTestHelpers.AssertOkResponse<object>(
            await controller.UpdateUniversity(new UniversityUpdateRequest
            {
                UniversityId = 5,
                Name = "Updated",
                Country = "Poland",
                Voivodeship = "Slaskie",
                City = "Gliwice",
                Address = "Side 2"
            }),
            "University updated successfully");
        var deleted = ControllerTestHelpers.AssertOkResponse<object>(
            await controller.DeleteUniversity(5),
            "University deleted successfully");

        Assert.Null(created.Data);
        Assert.Null(updated.Data);
        Assert.Null(deleted.Data);
        Assert.Collection(
            dispatcher.SentRequests,
            request =>
            {
                var command = Assert.IsType<CreateUniversityCommand>(request);
                Assert.Equal("Uni", command.Name);
                Assert.Equal("Poland", command.Country);
                Assert.Equal("Mazowieckie", command.Voivodeship);
                Assert.Equal("Warsaw", command.City);
                Assert.Equal("Main 1", command.Address);
            },
            request =>
            {
                var command = Assert.IsType<UpdateUniversityCommand>(request);
                Assert.Equal(5, command.UniversityId);
                Assert.Equal("Updated", command.Name);
                Assert.Equal("Slaskie", command.Voivodeship);
            },
            request =>
            {
                var command = Assert.IsType<DeleteUniversityCommand>(request);
                Assert.Equal(5, command.UniversityId);
            });
    }

    [Fact]
    public async Task DepartmentsController_endpoints_forward_requests_and_return_expected_responses()
    {
        var dispatcher = new FakeModuleRequestDispatcher();
        var department = new DepartmentDto { Id = 3, Name = "Engineering", FieldsOfStudy = [] };
        var fields = new[] { new FieldOfStudyDto { Id = 4, Name = "Robotics" } };
        dispatcher.QueueResult<DepartmentDto?>(department);
        dispatcher.QueueResult<IEnumerable<FieldOfStudyDto>>(fields);
        var controller = new DepartmentsController(dispatcher);

        var created = ControllerTestHelpers.AssertOkResponse<object>(
            await controller.CreateDepartment(new DepartmentCreateRequest(5, "Engineering")),
            "Department added successfully");
        var found = ControllerTestHelpers.AssertOkResponse<DepartmentDto?>(
            await controller.GetDepartmentById(3),
            "Department retrieved successfully");
        var foundFields = ControllerTestHelpers.AssertOkResponse<IEnumerable<FieldOfStudyDto>>(
            await controller.GetFieldsOfStudy(3, 1, 10),
            "Fields of study retrieved successfully");
        var deleted = ControllerTestHelpers.AssertOkResponse<object>(
            await controller.DeleteDepartment(3),
            "Department deleted successfully");
        var updated = ControllerTestHelpers.AssertOkResponse<object>(
            await controller.UpdateDepartment(new DepartmentUpdateRequest { DepartmentId = 3, DepartmentName = "Science" }),
            "Department updated successfully");

        Assert.Null(created.Data);
        Assert.Same(department, found.Data);
        Assert.Same(fields[0], Assert.Single(foundFields.Data));
        Assert.Null(deleted.Data);
        Assert.Null(updated.Data);
        Assert.Collection(
            dispatcher.SentRequests,
            request => Assert.Equal(new AddDepartmentCommand(5, "Engineering"), Assert.IsType<AddDepartmentCommand>(request)),
            request => Assert.Equal(new GetDepartmentByIdQuery(3), Assert.IsType<GetDepartmentByIdQuery>(request)),
            request => Assert.Equal(new GetFieldsOfStudyByDepartmentIdQuery(3, 1, 10), Assert.IsType<GetFieldsOfStudyByDepartmentIdQuery>(request)),
            request => Assert.Equal(new DeleteDepartmentCommand(3), Assert.IsType<DeleteDepartmentCommand>(request)),
            request => Assert.Equal(new UpdateDepartmentCommand(3, "Science"), Assert.IsType<UpdateDepartmentCommand>(request)));
    }

    [Fact]
    public async Task FieldsOfStudyController_endpoints_forward_requests_and_return_expected_responses()
    {
        var dispatcher = new FakeModuleRequestDispatcher();
        var field = new FieldOfStudyDto { Id = 4, Name = "Robotics" };
        dispatcher.QueueResult<FieldOfStudyDto?>(field);
        var controller = new FieldsOfStudyController(dispatcher);

        var found = ControllerTestHelpers.AssertOkResponse<FieldOfStudyDto?>(
            await controller.GetFieldOfStudy(4),
            "Field of study retrieved successfully");
        var created = ControllerTestHelpers.AssertOkResponse<object>(
            await controller.CreateFieldOfStudy(new FieldOfStudyCreateRequest(3, "Robotics")),
            "Field of study added successfully");
        var deleted = ControllerTestHelpers.AssertOkResponse<object>(
            await controller.DeleteFieldOfStudy(4),
            "Field of study deleted successfully");
        var updated = ControllerTestHelpers.AssertOkResponse<object>(
            await controller.UpdateFieldOfStudy(new FieldOfStudyUpdateRequest { FieldOfStudyId = 4, FieldOfStudyName = "Automation" }),
            "Field of study updated successfully");

        Assert.Same(field, found.Data);
        Assert.Null(created.Data);
        Assert.Null(deleted.Data);
        Assert.Null(updated.Data);
        Assert.Collection(
            dispatcher.SentRequests,
            request => Assert.Equal(new GetFieldOfStudyByIdQuery(4), Assert.IsType<GetFieldOfStudyByIdQuery>(request)),
            request => Assert.Equal(new AddFieldOfStudyCommand(3, "Robotics"), Assert.IsType<AddFieldOfStudyCommand>(request)),
            request => Assert.Equal(new DeleteFieldOfStudyCommand(4), Assert.IsType<DeleteFieldOfStudyCommand>(request)),
            request => Assert.Equal(new UpdateFieldOfStudyCommand(4, "Automation"), Assert.IsType<UpdateFieldOfStudyCommand>(request)));
    }

    [Fact]
    public async Task AllowedEmailDomainsController_endpoints_forward_requests_and_return_expected_responses()
    {
        var dispatcher = new FakeModuleRequestDispatcher();
        var domain = new AllowedEmailDomainDto { Id = 8, Domain = "uni.edu" };
        dispatcher.QueueResult<AllowedEmailDomainDto?>(domain);
        var controller = new AllowedEmailDomainsController(dispatcher);

        var created = ControllerTestHelpers.AssertOkResponse<object>(
            await controller.CreateAllowedEmailDomain(new AllowedEmailCreateRequest(5, "uni.edu")),
            "Allowed email domain added successfully");
        var found = ControllerTestHelpers.AssertOkResponse<AllowedEmailDomainDto?>(
            await controller.GetAllowedEmailDomain(8),
            "Allowed email domain retrieved successfully");
        var deleted = ControllerTestHelpers.AssertOkResponse<object>(
            await controller.DeleteAllowedEmailDomain(8),
            "Allowed email domain deleted successfully");
        var updated = ControllerTestHelpers.AssertOkResponse<object>(
            await controller.UpdateAllowedEmailDomain(new AllowedEmailUpdateRequest { DomainId = 8, NewDomain = "student.uni.edu" }),
            "Allowed email domain updated successfully");

        Assert.Null(created.Data);
        Assert.Same(domain, found.Data);
        Assert.Null(deleted.Data);
        Assert.Null(updated.Data);
        Assert.Collection(
            dispatcher.SentRequests,
            request => Assert.Equal(new AddAllowedEmailDomainCommand(5, "uni.edu"), Assert.IsType<AddAllowedEmailDomainCommand>(request)),
            request => Assert.Equal(new GetAllowedEmailDomainByIdQuery(8), Assert.IsType<GetAllowedEmailDomainByIdQuery>(request)),
            request => Assert.Equal(new DeleteAllowedEmailDomainCommand(8), Assert.IsType<DeleteAllowedEmailDomainCommand>(request)),
            request => Assert.Equal(new UpdateAllowedEmailDomainCommand(8, "student.uni.edu"), Assert.IsType<UpdateAllowedEmailDomainCommand>(request)));
    }

    private static UniversityDto CreateUniversityDto()
        => new()
        {
            Id = 5,
            Name = "Uni",
            Country = "Poland",
            Voivodeship = "Mazowieckie",
            City = "Warsaw",
            Address = "Main 1",
            Departments = [],
            AllowedEmailDomains = []
        };
}
