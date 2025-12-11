namespace AuthService.API.Extentions;

public static class SystemIdGenerator
{
    public static Task<string?> GetSystemIdAsync()
    {
        var counter = Random.Shared.Next(10000);
        return Task.FromResult("EMP-" + counter)!;
    }
}