using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector.ChecksumLogic;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.AbstractionLayer.Interfaces;

public interface IPiranhaMessage
{
    public ByteStream ByteStream { get; }
    public ChecksumEncoder ChecksumEncoder { get; set; }

    public int GetProxySessionId();
    public void SetProxySessionId(int proxySessionId);

    public int GetEncodingLength();
    public byte[] GetMessageBytes();

    public int GetMessageType();
    public int GetServiceNodeType();
    public string GetMessageTypeName();

    public int GetMessageVersion();
    public void SetMessageVersion(int newArg);

    public void Encode();
    public void Decode();

    public void Clear();
    public void Destruct();

    public bool IsClientToServerMessage()
    {
        return GetMessageType() is >= 10000 and < 20000 or 30000;
    }

    public bool IsServerToClientMessage()
    {
        return GetMessageType() is >= 20000 and < 30000 or 40000;
    }
}