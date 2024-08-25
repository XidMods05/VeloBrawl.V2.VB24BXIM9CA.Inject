using Microsoft.Extensions.DependencyInjection;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Database.AbstractionLayer;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Database.Database.Newbie;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.DatabaseProtocol;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.DependencyLayer.P1.DI_P1;

public static class DependencyHelperP1
{
    public static IDatabaseProvider<DatabaseModel<AccountDocument>> GetAccountInject()
    {
        return DependencyManualInjector.ServiceProvider
            .GetRequiredService<IDatabaseProvider<DatabaseModel<AccountDocument>>>();
    }
}