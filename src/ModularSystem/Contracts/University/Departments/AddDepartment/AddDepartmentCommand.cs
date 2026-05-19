using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.University.Departments.AddDepartment;

public record AddDepartmentCommand(int UniversityId, string DepartmentName) : ICommand;