using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Database.Database.Newbie;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.DatabaseProtocol;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home.Laser;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector.HelpsLogic;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home;

public class LogicClientHome(DatabaseModel<AccountDocument> accountModel)
{
    public void Encode(ByteStream byteStream)
    {
        new LogicDailyData(accountModel).Encode(byteStream);
        new LogicConfData(accountModel).Encode(byteStream);

        byteStream.WriteLong(accountModel.Document.AccountId);

        byteStream.WriteVInt(0);
        {
            /*foreach (var notification in v1.FreeTextNotifications)
            {
                byteStream.WriteVInt(notification.Value.GetNotificationType());
                notification.Value.Encode(byteStream);
            }*/
        }

        byteStream.WriteVInt(-1);
        byteStream.WriteBoolean(false);
        byteStream.WriteVInt(0);

        var v101 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v101; i++) ByteStreamHelper.WriteDataReference(byteStream, 0);
        }
    }
}