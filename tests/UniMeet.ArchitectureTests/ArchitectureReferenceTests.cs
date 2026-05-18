using System.Xml.Linq;
using Xunit;

namespace UniMeet.ArchitectureTests;

public class ArchitectureReferenceTests
{
    private static readonly string RepositoryRoot = FindRepositoryRoot();

    private static readonly IReadOnlyDictionary<string, string> ModuleNamespacePrefixes =
        new Dictionary<string, string>
        {
            ["MailingModule"] = "MailingModule",
            ["MatchingModule"] = "UniMeet.MatchingModule",
            ["MessagingModule"] = "UniMeet.MessagingModule",
            ["PermissionsModule"] = "PermissionsModule",
            ["UniversityModule"] = "UniMeet.UniversityModule",
            ["UserEnrollmentModule"] = "UniMeet.UserEnrollmentModule",
            ["UserModule"] = "UniMeet.UserModule"
        };

    [Fact]
    public void Only_modular_system_can_reference_shared_directly()
    {
        var violations = ProjectFiles()
            .Where(project => Path.GetFileNameWithoutExtension(project) != "ModularSystem")
            .SelectMany(project => GetProjectReferences(project, ModuleProjectsByPath())
                .Where(reference => reference.TargetProjectName == "UniMeet.Shared")
                .Select(reference => $"{ToRelativePath(project)} references {reference.RelativePath}"))
            .ToList();

        AssertNoViolations(violations, "Only ModularSystem can reference UniMeet.Shared directly.");
    }

    [Fact]
    public void Api_project_must_reference_modules_only_through_module_roots()
    {
        var apiProject = Path.Combine(RepositoryRoot, "src", "UniMeet.API", "UniMeet.API.csproj");
        var moduleProjectsByPath = ModuleProjectsByPath();

        var violations = GetProjectReferences(apiProject, moduleProjectsByPath)
            .Where(reference =>
                reference.TargetProjectName != "ModularSystem"
                && (reference.TargetModule is null || reference.TargetLayer != Layer.ModuleRoot))
            .Select(reference => $"{ToRelativePath(apiProject)} references {reference.RelativePath}")
            .ToList();

        AssertNoViolations(violations, "API can reference ModularSystem and module root projects only.");
    }

    [Fact]
    public void Module_projects_must_not_reference_projects_from_other_modules()
    {
        var violations = ModuleProjects()
            .SelectMany(project => project.ProjectReferences
                .Where(reference => reference.TargetModule is not null && reference.TargetModule != project.Module)
                .Select(reference => $"{project.RelativePath} references {reference.RelativePath}"))
            .ToList();

        AssertNoViolations(violations, "Module projects cannot reference projects from other modules.");
    }

    [Fact]
    public void Module_project_references_must_follow_clean_architecture_direction()
    {
        var violations = ModuleProjects()
            .SelectMany(project => project.ProjectReferences
                .Where(reference => !IsAllowedReference(project, reference))
                .Select(reference =>
                    $"{project.RelativePath} ({project.Layer}) references {reference.RelativePath} ({reference.TargetLayer?.ToString() ?? "external"})"))
            .ToList();

        AssertNoViolations(violations, "Project references do not follow the expected clean architecture direction.");
    }

    [Fact]
    public void Domain_projects_must_not_have_package_references()
    {
        var violations = ModuleProjects()
            .Where(project => project.Layer == Layer.Domain)
            .SelectMany(project => project.PackageReferences
                .Select(package => $"{project.RelativePath} references package {package}"))
            .ToList();

        AssertNoViolations(violations, "Domain projects should not depend on external packages.");
    }

    [Fact]
    public void Module_code_must_not_use_namespaces_from_other_modules()
    {
        var violations = ModuleSourceFiles()
            .SelectMany(file => ModuleNamespacePrefixes
                .Where(module => module.Key != file.Module)
                .Select(module => new
                {
                    TargetModule = module.Key,
                    Prefix = $"using {module.Value}",
                    Lines = File.ReadLines(file.FullPath)
                        .Select((line, index) => new { Line = line.Trim(), Number = index + 1 })
                        .Where(line => line.Line.StartsWith($"using {module.Value}", StringComparison.Ordinal))
                        .ToList()
                })
                .Where(result => result.Lines.Count > 0)
                .SelectMany(result => result.Lines.Select(line =>
                    $"{file.RelativePath}:{line.Number} uses {result.TargetModule}: {line.Line}")))
            .ToList();

        AssertNoViolations(violations, "Module source code cannot import namespaces from other modules.");
    }

    private static bool IsAllowedReference(ProjectInfo project, ProjectReference reference)
    {
        if (reference.TargetModule is not null && reference.TargetModule != project.Module)
        {
            return false;
        }

        if (reference.TargetModule is null)
        {
            return reference.TargetProjectName is "ModularSystem";
        }

        return project.Layer switch
        {
            Layer.Domain => false,
            Layer.Application => reference.TargetLayer is Layer.Domain,
            Layer.Infrastructure => reference.TargetLayer is Layer.Application or Layer.Domain,
            Layer.ModuleRoot => reference.TargetLayer is Layer.Application or Layer.Domain or Layer.Infrastructure,
            _ => false
        };
    }

    private static IReadOnlyList<ProjectInfo> ModuleProjects()
    {
        var projectByPath = ModuleProjectsByPath();

        return projectByPath.Values
            .Select(project => project with
            {
                ProjectReferences = GetProjectReferences(project.FullPath, projectByPath),
                PackageReferences = GetPackageReferences(project.FullPath)
            })
            .ToList();
    }

    private static IReadOnlyDictionary<string, ProjectInfo> ModuleProjectsByPath()
    {
        return ModuleProjectFiles()
            .ToDictionary(path => path, path => CreateProjectInfo(path, []));
    }

    private static IReadOnlyList<string> ModuleProjectFiles()
    {
        var moduleRoot = Path.Combine(RepositoryRoot, "src", "Modules");
        return Directory
            .EnumerateFiles(moduleRoot, "*.csproj", SearchOption.AllDirectories)
            .Where(path => !IsGeneratedPath(path))
            .Select(Path.GetFullPath)
            .ToList();
    }

    private static IReadOnlyList<string> ProjectFiles()
    {
        return Directory
            .EnumerateFiles(RepositoryRoot, "*.csproj", SearchOption.AllDirectories)
            .Where(path => !IsGeneratedPath(path))
            .Select(Path.GetFullPath)
            .ToList();
    }

    private static ProjectInfo CreateProjectInfo(string projectPath, IReadOnlyList<ProjectReference> projectReferences)
    {
        var relativePath = ToRelativePath(projectPath);
        var module = GetModuleName(projectPath)
                     ?? throw new InvalidOperationException($"Project is outside src/Modules: {relativePath}");
        var projectName = Path.GetFileNameWithoutExtension(projectPath);

        return new ProjectInfo(
            projectName,
            module,
            GetLayer(projectName),
            relativePath,
            projectPath,
            projectReferences,
            []);
    }

    private static IReadOnlyList<ProjectReference> GetProjectReferences(
        string projectPath,
        IReadOnlyDictionary<string, ProjectInfo> moduleProjectsByPath)
    {
        var projectDirectory = Path.GetDirectoryName(projectPath)!;

        return LoadProject(projectPath)
            .Descendants()
            .Where(element => element.Name.LocalName == "ProjectReference")
            .Select(element => element.Attribute("Include")?.Value)
            .Where(include => !string.IsNullOrWhiteSpace(include))
            .Select(include =>
            {
                var fullPath = Path.GetFullPath(Path.Combine(projectDirectory, include!));
                moduleProjectsByPath.TryGetValue(fullPath, out var targetProject);

                return new ProjectReference(
                    ToRelativePath(fullPath),
                    Path.GetFileNameWithoutExtension(fullPath),
                    GetModuleName(fullPath),
                    targetProject?.Layer);
            })
            .ToList();
    }

    private static IReadOnlyList<string> GetPackageReferences(string projectPath)
    {
        return LoadProject(projectPath)
            .Descendants()
            .Where(element => element.Name.LocalName == "PackageReference")
            .Select(element => element.Attribute("Include")?.Value)
            .Where(include => !string.IsNullOrWhiteSpace(include))
            .Select(include => include!)
            .ToList();
    }

    private static IReadOnlyList<SourceFileInfo> ModuleSourceFiles()
    {
        var moduleRoot = Path.Combine(RepositoryRoot, "src", "Modules");

        return Directory
            .EnumerateFiles(moduleRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path => !IsGeneratedPath(path))
            .Select(path => new SourceFileInfo(
                GetModuleName(path) ?? throw new InvalidOperationException($"File is outside src/Modules: {path}"),
                ToRelativePath(path),
                Path.GetFullPath(path)))
            .ToList();
    }

    private static XDocument LoadProject(string projectPath)
    {
        using var stream = File.OpenRead(projectPath);
        return XDocument.Load(stream);
    }

    private static Layer GetLayer(string projectName)
    {
        if (projectName.EndsWith(".Domain", StringComparison.Ordinal))
        {
            return Layer.Domain;
        }

        if (projectName.EndsWith(".Application", StringComparison.Ordinal))
        {
            return Layer.Application;
        }

        if (projectName.EndsWith(".Infrastructure", StringComparison.Ordinal))
        {
            return Layer.Infrastructure;
        }

        return Layer.ModuleRoot;
    }

    private static string? GetModuleName(string path)
    {
        var relativePath = ToRelativePath(path);
        var parts = relativePath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

        for (var i = 0; i < parts.Length - 1; i++)
        {
            if (parts[i] == "Modules")
            {
                return parts[i + 1];
            }
        }

        return null;
    }

    private static string ToRelativePath(string path)
    {
        return Path.GetRelativePath(RepositoryRoot, path).Replace('\\', '/');
    }

    private static bool IsGeneratedPath(string path)
    {
        var normalized = path.Replace('\\', '/');
        return normalized.Contains("/bin/", StringComparison.Ordinal)
               || normalized.Contains("/obj/", StringComparison.Ordinal)
               || normalized.Contains("/Migrations/", StringComparison.Ordinal);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "UniMeet.sln")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new InvalidOperationException("Could not find UniMeet.sln in current directory hierarchy.");
    }

    private static void AssertNoViolations(IReadOnlyCollection<string> violations, string message)
    {
        Assert.True(violations.Count == 0, $"{message}{Environment.NewLine}{string.Join(Environment.NewLine, violations)}");
    }

    private enum Layer
    {
        Domain,
        Application,
        Infrastructure,
        ModuleRoot
    }

    private sealed record ProjectInfo(
        string Name,
        string Module,
        Layer Layer,
        string RelativePath,
        string FullPath,
        IReadOnlyList<ProjectReference> ProjectReferences,
        IReadOnlyList<string> PackageReferences);

    private sealed record ProjectReference(
        string RelativePath,
        string TargetProjectName,
        string? TargetModule,
        Layer? TargetLayer);

    private sealed record SourceFileInfo(string Module, string RelativePath, string FullPath);
}
