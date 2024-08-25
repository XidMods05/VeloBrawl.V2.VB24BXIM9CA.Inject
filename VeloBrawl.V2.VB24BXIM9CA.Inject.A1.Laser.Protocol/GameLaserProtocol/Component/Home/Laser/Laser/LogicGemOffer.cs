using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector.HelpsLogic;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.MathematicalSector.GlobalId;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.Enumerations.Game;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Component.Home.Laser.Laser;

public class LogicGemOffer(ShopItemHelperTable shopItemHelperTable, int count)
{
    private int _itemGlobalId;
    private int _itemGlobalIdX;

    public void Encode(ByteStream byteStream)
    {
        byteStream.WriteVInt((int)shopItemHelperTable);
        byteStream.WriteVInt(_itemGlobalIdX > 0 ? 0 : count);
        ByteStreamHelper.WriteDataReference(byteStream, _itemGlobalIdX > 0 ? 0 : _itemGlobalId);
        byteStream.WriteVInt(_itemGlobalIdX > 1000000 ? GlobalId.GetInstanceId(_itemGlobalIdX) : _itemGlobalIdX);
    }

    public void SetItem(int itemGlobalId, bool isX = false)
    {
        _itemGlobalId = itemGlobalId;
        if (isX) _itemGlobalIdX = itemGlobalId;
    }

    public void SetSkin(int skinInstanceId)
    {
        _itemGlobalIdX = skinInstanceId;
    }
}