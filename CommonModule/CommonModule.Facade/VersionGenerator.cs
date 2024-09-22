namespace CommonModule.Facade;

public static class VersionGenerator
{
    private static readonly string version;

    static VersionGenerator()
    {
        version = GenerateVersion();
    }

    private static string GenerateVersion()
    {
        return Guid.NewGuid().ToString().Replace("-", "");
    }

    public static string GetVersion()
    {
        return version;
    }
}
