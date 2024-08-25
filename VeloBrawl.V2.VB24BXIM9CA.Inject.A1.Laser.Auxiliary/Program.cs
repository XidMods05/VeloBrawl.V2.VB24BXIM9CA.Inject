using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Auxiliary.Cleaner;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Auxiliary.Settings;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.TablesOfDataSector;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.Enumerations.Secure;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.HelpDirectory;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.HelpDirectory.Profanity;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.OwnLogging;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.OwnLogging.MiniHelper;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Auxiliary;

public static class Program
{
    public static void Main(string[] args)
    {
        AppConfigurator.InAppGlobalLogLevel = (UniqueLogLevels)AppConfig.LogSensitive;

        AppConfigurator.LogoConstant =
            "\n\u2588\u2588\u2557\u2588\u2588\u2588\u2557\u2591\u2591\u2588\u2588\u2557\u2591\u2591\u2591\u2591\u2591\u2588\u2588\u2557\u2588\u2588\u2588\u2588\u2588\u2588\u2588\u2557\u2591\u2588\u2588\u2588\u2588\u2588\u2557\u2591\u2588\u2588\u2588\u2588\u2588\u2588\u2588\u2588\u2557\n\u2588\u2588\u2551\u2588\u2588\u2588\u2588\u2557\u2591\u2588\u2588\u2551\u2591\u2591\u2591\u2591\u2591\u2588\u2588\u2551\u2588\u2588\u2554\u2550\u2550\u2550\u2550\u255d\u2588\u2588\u2554\u2550\u2550\u2588\u2588\u2557\u255a\u2550\u2550\u2588\u2588\u2554\u2550\u2550\u255d\n\u2588\u2588\u2551\u2588\u2588\u2554\u2588\u2588\u2557\u2588\u2588\u2551\u2591\u2591\u2591\u2591\u2591\u2588\u2588\u2551\u2588\u2588\u2588\u2588\u2588\u2557\u2591\u2591\u2588\u2588\u2551\u2591\u2591\u255a\u2550\u255d\u2591\u2591\u2591\u2588\u2588\u2551\u2591\u2591\u2591\n\u2588\u2588\u2551\u2588\u2588\u2551\u255a\u2588\u2588\u2588\u2588\u2551\u2588\u2588\u2557\u2591\u2591\u2588\u2588\u2551\u2588\u2588\u2554\u2550\u2550\u255d\u2591\u2591\u2588\u2588\u2551\u2591\u2591\u2588\u2588\u2557\u2591\u2591\u2591\u2588\u2588\u2551\u2591\u2591\u2591\n\u2588\u2588\u2551\u2588\u2588\u2551\u2591\u255a\u2588\u2588\u2588\u2551\u255a\u2588\u2588\u2588\u2588\u2588\u2554\u255d\u2588\u2588\u2588\u2588\u2588\u2588\u2588\u2557\u255a\u2588\u2588\u2588\u2588\u2588\u2554\u255d\u2591\u2591\u2591\u2588\u2588\u2551\u2591\u2591\u2591\n\u255a\u2550\u255d\u255a\u2550\u255d\u2591\u2591\u255a\u2550\u2550\u255d\u2591\u255a\u2550\u2550\u2550\u2550\u255d\u2591\u255a\u2550\u2550\u2550\u2550\u2550\u2550\u255d\u2591\u255a\u2550\u2550\u2550\u2550\u255d\u2591\u2591\u2591\u2591\u255a\u2550\u255d\u2591\u2591\u2591";
        LogoGroundDrawer.DrawLogoLines();

        GcManualCollector.StartCollector(AppConfig.MinutesToGc);
        AppEnvironment.PathToSavedFiles = HelperCity.FixAutoimmuneFilePath(AppDomain.CurrentDomain.BaseDirectory);

        ProfanityManager.SetFilePath("VelofaunaInternal/profanity.txt");
        ProfanityManager.Initialize();

        LogicDataTables.CreateReferences();

        if ((int)AppConfigurator.InAppGlobalLogLevel <= 1) TextProgressBarGroundDrawer.DrawTextProgressBar(8, 10, 100);
        ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Cmd, "Hi new project! <- Laser.Auxiliary: Hello!");
    }
}