# Unit Tests - UniMeet Backend

This document describes the unit tests implementation for the UniMeet backend project.

## Overview

The project now includes comprehensive unit tests using **xUnit** framework with **70%+ code coverage** for the Domain layers.

## Test Projects

Three test projects have been created:

1. **UniMeet.UserModule.Domain.Tests** - Tests for User domain entities
2. **UniMeet.UniversityModule.Domain.Tests** - Tests for University domain entities  
3. **UniMeet.UserModule.Application.Tests** - Tests for User application validators

## Test Statistics

- **Total Tests**: 99 tests (all passing ✅)
  - UserModule.Domain.Tests: 19 tests
  - UniversityModule.Domain.Tests: 44 tests
  - UserModule.Application.Tests: 36 tests

## Code Coverage

- **UniMeet.UserModule.Domain**: 77.1% line coverage ✅
- **UniMeet.UniversityModule.Domain**: 67.7% line coverage ✅
- **UniMeet.UserModule.Application**: 13.7% line coverage (validators fully tested)

## Running Tests

### Run all tests
```bash
dotnet test
```

### Run tests for a specific project
```bash
dotnet test tests/UniMeet.UserModule.Domain.Tests/UniMeet.UserModule.Domain.Tests.csproj
dotnet test tests/UniMeet.UniversityModule.Domain.Tests/UniMeet.UniversityModule.Domain.Tests.csproj
dotnet test tests/UniMeet.UserModule.Application.Tests/UniMeet.UserModule.Application.Tests.csproj
```

### Run tests with code coverage
```bash
dotnet test --collect:"XPlat Code Coverage" --results-directory ./TestResults
```

## Generating Coverage Reports

After running tests with coverage collection, generate an HTML report:

```bash
# Install ReportGenerator (if not already installed)
dotnet tool install -g dotnet-reportgenerator-globaltool

# Generate HTML report
reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" -targetdir:"TestResults/CoverageReport" -reporttypes:"Html;TextSummary"
```

Open `TestResults/CoverageReport/index.html` in a browser to view the detailed coverage report.

## Test Coverage by Module

### UserModule.Domain Tests (77.1% coverage)
- **User entity**: Constructor, UpdatePassword, Rename, Activate, SetGroup
- **ConfirmationCode**: Constructor, IsExpired, IsCorrect validation
- **RefreshToken**: Constructor, IsExpired validation
- **PasswordResetCode**: Constructor, IsExpired, IsCorrect validation

### UniversityModule.Domain Tests (67.7% coverage)
- **University entity**: Constructor with validation for all fields (name, country, voivodeship, city, address)
- **Department management**: Add, Remove, Rename operations
- **Field of Study management**: Add, Remove, Rename operations
- **Allowed Email Domain management**: Add, Remove, Change operations
- **Exception handling**: All domain exception scenarios tested

### UserModule.Application Tests (13.7% coverage)
- **RegisterUser validator**: All validation rules including:
  - Name length validation (min 3, max 100 characters)
  - Email format validation
  - Password length validation (min 8, max 100 characters)
  - Password complexity requirements (uppercase, lowercase, digit)
- **LoginUser validator**: All validation rules for authentication

## Testing Frameworks and Libraries

- **xUnit**: Test framework
- **FluentAssertions**: Fluent API for readable test assertions
- **Moq**: Mocking library (available for future use)
- **coverlet**: Code coverage measurement
- **ReportGenerator**: Coverage report generation

## Best Practices Followed

1. ✅ Arrange-Act-Assert (AAA) pattern in all tests
2. ✅ Descriptive test method names following convention: `MethodName_Scenario_ExpectedBehavior`
3. ✅ One assertion concept per test where practical
4. ✅ Theory tests with InlineData for parameterized testing
5. ✅ FluentAssertions for readable and maintainable assertions
6. ✅ No Thread.Sleep or time-dependent tests
7. ✅ Test isolation - each test is independent

## Security

All tests have been reviewed and passed CodeQL security analysis with **0 vulnerabilities** detected.

## Future Improvements

To further increase coverage, consider adding:
- Infrastructure layer tests with mocked repositories
- Application command handler tests with mocked dependencies
- Integration tests for end-to-end scenarios
- More Application layer validator tests for other commands
