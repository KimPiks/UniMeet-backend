namespace PermissionsModule.Config;

internal record Configuration(string ConnectionString, Dictionary<string, PermissionConfiguration> DefaultGroups);
internal record PermissionConfiguration(string Name, List<string> Permissions);