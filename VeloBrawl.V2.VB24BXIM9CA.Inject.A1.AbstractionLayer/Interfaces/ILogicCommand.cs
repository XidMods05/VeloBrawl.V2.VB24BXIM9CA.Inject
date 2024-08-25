using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.AbstractionLayer.Interfaces;

public interface ILogicServerCommand
{
    public void Decode(ByteStream byteStream);
    public void Encode(ByteStream byteStream);
    public int GetCommandType();
}