using System.Collections.Frozen;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.AbstractionLayer.Interfaces;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.DependencyLayer.P1.DI_P1;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Auxiliary.Debug;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Messages;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Network;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Server.Allocator.Alloc;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.OwnLogging;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Server.Allocator.MsgManager;

public class MessageManager : IOwnerMessageManager
{
    private readonly AllocSession _originalAllocSession;
    private bool _authed;

    private bool _closed;

    private FrozenDictionary<int, IMessageManager> _messageManagers;

    public MessageManager(AllocSession allocSession)
    {
        AllocSession = allocSession;
        _originalAllocSession = allocSession;

        _messageManagers = new Dictionary<int, IMessageManager>
        {
            { 1, new Node_1.Laser.MessageManager(allocSession, this) },
            { 9, new Node_9.Laser.MessageManager(allocSession, this) }
        }.ToFrozenDictionary();
    }

    public IEnumerable<byte> GuidKey { get; set; } = null!;
    public IAllocSession AllocSession { get; set; }

    public void SetAccountModel(long databaseId)
    {
        if (DependencyHelperP1.GetAccountInject().Get(databaseId) == null)
        {
            ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Error,
                $"Database account not found! Id: {databaseId}.");
            return;
        }

        _authed = true;

        foreach (var messageManager in _messageManagers)
            messageManager.Value.SetAccountModel(databaseId);
    }

    public int ReceiveMessage(int node, byte[] message)
    {
        if (_closed) return 0;

        if (node != 1)
            if (!_authed)
            {
                ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Error,
                    $"Authentication not passed! Id: {AllocSession.SessionId}.");
                return -403;
            }

        var header = Messaging.ReadHeader(message);
        var payload = message.Skip(7).ToArray();

        var piranhaMessage = LogicLaserMessageFactory.CreateMessageByType(header.Item1);
        {
            if (piranhaMessage == null)
            {
                ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Warn,
                    $"New unknown message received: {header.Item1}: {DebugInfoCollector.PacketCollectorY[header.Item1]}.");
                return 0;
            }
        }

        if (header.Item2 > 0)
        {
            piranhaMessage.ByteStream.SetByteArray(payload, payload.Length);
            piranhaMessage.ChecksumEncoder = piranhaMessage.ByteStream;

            piranhaMessage.Decode();
        }

        try
        {
            if (_messageManagers.TryGetValue(node, out var messageManager))
                return messageManager.ReceiveMessage(piranhaMessage);
            return -6001;
        }
        catch
        {
            return -6003;
        }
    }

    public int SendMessage(IPiranhaMessage piranhaMessage)
    {
        if (_closed) return 0;

        if (piranhaMessage.GetEncodingLength() <= 0)
            piranhaMessage.Encode();

        var message = Messaging.WriteHeader(
            piranhaMessage.GetMessageBytes(),
            piranhaMessage.GetMessageType(),
            piranhaMessage.GetMessageVersion());

        Buffer.BlockCopy(piranhaMessage.GetMessageBytes(), 0, message, 7, piranhaMessage.GetMessageBytes().Length);
        _originalAllocSession.NodeServerProducer.SendMessage(GuidKey.Concat(message).ToArray());
        return 0;
    }

    public void Goodbye()
    {
        try
        {
            foreach (var messageManager in _messageManagers)
                messageManager.Value.Goodbye();
        }
        catch (Exception e)
        {
            ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Error, $"Error while 'Goodbye' procession! {e}.");
        }

        _closed = true;
        _messageManagers = null!;
    }
}