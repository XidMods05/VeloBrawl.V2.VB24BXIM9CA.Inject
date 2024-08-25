using Microsoft.Extensions.DependencyInjection;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Auxiliary.Settings;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Database.AbstractionLayer;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Database.Database;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Database.Database.Newbie;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.DatabaseProtocol;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.OwnLogging;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.OwnLogging.MiniHelper;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol;

public static class Program
{
    public static void Main(string[] args)
    {
        var databaseName = AppConfig.DatabaseName.ToLower();

        if (databaseName.Contains("mongo"))
        {
            DependencyManualInjector.ServiceCollection.AddSingleton<IDatabaseProvider<DatabaseModel<AccountDocument>>>
            (new MongoDatabaseProvider<DatabaseModel<AccountDocument>>
                (AppConfig.MongoDbConnectionString, AppConfig.MongoDbClusterName, "accounts", true, 15));
            return;
        }

        DependencyManualInjector.ServiceCollection.AddSingleton<IDatabaseProvider<DatabaseModel<AccountDocument>>>
        (new PostgresDatabaseProvider<DatabaseModel<AccountDocument>>
            (AppConfig.PostgresConnectionString, "accounts", true, 15));

        if ((int)AppConfigurator.InAppGlobalLogLevel <= 1) TextProgressBarGroundDrawer.DrawTextProgressBar(8, 75, 100);
        ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Cmd, "Hi new project! <- Laser.Protocol: Hello!");
    }
}