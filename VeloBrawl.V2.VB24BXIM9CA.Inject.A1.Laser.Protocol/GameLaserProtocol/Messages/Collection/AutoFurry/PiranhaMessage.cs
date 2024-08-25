using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.AbstractionLayer.Interfaces;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Auxiliary.Debug;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.EngineFactory.DataStreamSector.ChecksumLogic;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages.Collection.AutoFurry;

public abstract class PiranhaMessage : IPiranhaMessage
{
    private int _messageVersion = -1;
    private int _proxySessionId;

    public ByteStream ByteStream { get; } = new(8);
    public ChecksumEncoder ChecksumEncoder { get; set; } = new();

    public virtual int GetProxySessionId()
    {
        return _proxySessionId;
    }

    public virtual void SetProxySessionId(int proxySessionId)
    {
        _proxySessionId = proxySessionId;
    }

    public virtual int GetEncodingLength()
    {
        return ByteStream.GetLength();
    }

    public virtual byte[] GetMessageBytes()
    {
        return ByteStream.GetByteArray();
    }

    public abstract int GetMessageType();
    public abstract int GetServiceNodeType();

    public virtual string GetMessageTypeName()
    {
        return DebugInfoCollector.PacketCollectorY.GetValueOrDefault(GetMessageType(), ("UNKNOWN-NAME", -1)).Item1;
    }

    public virtual int GetMessageVersion()
    {
        return _messageVersion < 1 ? GetMessageType() == 20104 ? 1 : 0 : _messageVersion;
    }

    public void SetMessageVersion(int newArg)
    {
        _messageVersion = newArg;
    }

    public virtual void Encode()
    {
    }

    public virtual void Decode()
    {
    }

    public virtual void Clear()
    {
        ByteStream.Clear(GetEncodingLength());
    }

    public virtual void Destruct()
    {
        ByteStream.Destruct();
    }

    public bool IsClientToServerMessage()
    {
        return GetMessageType() is >= 10000 and < 20000 or 30000;
    }

    public bool IsServerToClientMessage()
    {
        return GetMessageType() is >= 20000 and < 30000 or 40000;
    }
}