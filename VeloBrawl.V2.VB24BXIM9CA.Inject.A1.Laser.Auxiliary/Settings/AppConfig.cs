using Newtonsoft.Json.Linq;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Auxiliary.Settings;

public static class AppConfig
{
    static AppConfig()
    {
        var config = JObject.Parse(File.ReadAllText("SaveBase/config.json"));

        foreach (var valueTuple in typeof(AppConfig).GetProperties()
                     .Select(property => (property,
                         config.GetValue(property.Name.Replace("_", ""), StringComparison.CurrentCultureIgnoreCase))))
            if (valueTuple.Item2 != null)
                valueTuple.property.SetValue(null, valueTuple.Item2.ToObject(valueTuple.property.PropertyType));
    }

    public static int LogSensitive { get; set; }
    public static int MinutesToGc { get; set; }

    public static string RabbitMqHost { get; set; } = null!;

    public static int TimeInSecsToNextEventUpdate { get; set; }

    public static string DatabaseName { get; set; } = null!;

    public static string PostgresConnectionString { get; set; } = null!;

    public static string MongoDbConnectionString { get; set; } = null!;
    public static string MongoDbClusterName { get; set; } = null!;
}