using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.OwnLogging.MiniHelper;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1;

public static class Program
{
    public static void Main(string[] args)
    {
        Laser.Auxiliary.Program.Main(args);
        Laser.Butler.Program.Main(args);
        Laser.Database.Program.Main(args);
        Laser.Protocol.Program.Main(args);
        Laser.RabbitMqProxy.Program.Main(args);
        Laser.RabbitMqFirewall.Program.Main(args);

        if ((int)AppConfigurator.InAppGlobalLogLevel <= 1) TextProgressBarGroundDrawer.DrawTextProgressBar(8, 100, 100);
        DependencyManualInjector.Build();
        for (;;) ;
    }
}