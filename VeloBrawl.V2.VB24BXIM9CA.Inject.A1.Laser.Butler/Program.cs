using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Auxiliary.Settings;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Butler.LobbyRotator;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.OwnLogging;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.OwnLogging.MiniHelper;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Butler;

public static class Program
{
    public static void Main(string[] args)
    {
        VisibilityRotator.DefaultSecTimeForEventUpdate = AppConfig.TimeInSecsToNextEventUpdate;
        VisibilityRotatorAdministrator.InitializeRotateEvents("SaveBase/events.json");

        if ((int)AppConfigurator.InAppGlobalLogLevel <= 1) TextProgressBarGroundDrawer.DrawTextProgressBar(8, 35, 100);
        ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Cmd, "Hi new project! <- Laser.Butler: Hello!");
    }
}