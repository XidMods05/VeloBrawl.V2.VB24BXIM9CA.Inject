using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.OwnLogging;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.OwnLogging.MiniHelper;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Database;

public static class Program
{
    public static void Main(string[] args)
    {
        if ((int)AppConfigurator.InAppGlobalLogLevel <= 1) TextProgressBarGroundDrawer.DrawTextProgressBar(8, 70, 100);
        ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Cmd, "Hi new project! <- Laser.Database: Hello!");
    }
}