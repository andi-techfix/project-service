namespace Infrastructure;

public record MongoDbConfiguration
{
    public const string SectionName = "MongoDatabase";
    public string Host { get; init; } = null!;
    public string? ReadHost { get; init; }
    public int Port { get; init; }
    public string DatabaseName { get; init; } = null!;
    public string? User { get; init; }
    public string? Password { get; init; }
    
    public static void ThrowIfInvalid(MongoDbConfiguration? configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrEmpty(configuration.Host);
        if (configuration.Port <= 0)
        {
            throw new ArgumentException("Port must be greater than 0");
        }
        ArgumentException.ThrowIfNullOrEmpty(configuration.DatabaseName);
    }
    
    public static string GetConnectionString(MongoDbConfiguration options, bool isReadOnly = false)
    {
        var host = isReadOnly && !string.IsNullOrEmpty(options.ReadHost) ? options.ReadHost : options.Host;
        var credentials = !string.IsNullOrEmpty(options.User)
            ? $"{options.User}:{options.Password}@"
            : string.Empty;
        // Compose host and port only; the database is selected via GetDatabase on IMongoClient
        return $"mongodb://{credentials}{host}:{options.Port}";
    }
}