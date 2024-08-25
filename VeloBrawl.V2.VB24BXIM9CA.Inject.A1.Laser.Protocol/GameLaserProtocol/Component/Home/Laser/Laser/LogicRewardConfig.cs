using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.MathematicalSector.GlobalId;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.Enumerations.Game;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home.Laser.Laser;

public class LogicRewardConfig
{
    public void Encode(ByteStream byteStream)
    {
        new LogicCondition().Encode(byteStream);

        var c = new LogicGemOffer(ShopItemHelperTable.UpgradeMaterial, 1);
        {
            c.SetItem(GlobalId.CreateGlobalId(16, 0), true);
        }

        c.Encode(byteStream);
    }
}