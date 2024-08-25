using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Database.Database.Newbie;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.DatabaseProtocol;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector.HelpsLogic;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Avatar;

public class LogicClientAvatar(DatabaseModel<AccountDocument> accountModel)
{
    public DatabaseModel<AccountDocument> DatabaseModel { get; } = accountModel;

    public void Encode(ByteStream byteStream)
    {
        ByteStreamHelper.EncodeLogicLong(byteStream, DatabaseModel.Id);
        ByteStreamHelper.EncodeLogicLong(byteStream, DatabaseModel.GetDocument().AccountId);
        ByteStreamHelper.EncodeLogicLong(byteStream, DatabaseModel.GetDocument().HomeId);

        byteStream.WriteStringReference(DatabaseModel.GetDocument().AvatarName);
        byteStream.WriteBoolean(DatabaseModel.GetDocument().NameSetByUser);

        byteStream.WriteInt(0);

        byteStream.WriteVInt(8);

        byteStream.WriteVInt(2 + 1);

        ByteStreamHelper.WriteDataReference(byteStream, 23, 0);
        byteStream.WriteVInt(1);

        ByteStreamHelper.WriteDataReference(byteStream, 5, 8);
        byteStream.WriteVInt(0);

        ByteStreamHelper.WriteDataReference(byteStream, 5, 10);
        byteStream.WriteVInt(0);

        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(2);
    }

    public bool UseDiamonds(int count)
    {
        if (DatabaseModel.GetDocument().Diamonds < count) return false;
        DatabaseModel.GetDocument().Diamonds -= count;
        return true;
    }

    public bool UseUpgradeMaterials(int count)
    {
        if (DatabaseModel.GetDocument().UpgradeMaterial < count) return false;
        DatabaseModel.GetDocument().UpgradeMaterial -= count;
        return true;
    }

    public bool UseStarPoints(int count)
    {
        if (DatabaseModel.GetDocument().StarPoints < count) return false;
        DatabaseModel.GetDocument().StarPoints -= count;
        return true;
    }
}