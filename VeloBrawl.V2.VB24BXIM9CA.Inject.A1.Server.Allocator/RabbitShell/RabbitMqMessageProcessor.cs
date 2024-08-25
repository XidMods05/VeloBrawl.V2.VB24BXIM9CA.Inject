using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.AbstractionLayer.Interfaces;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Auxiliary.Debug;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Server.Allocator.Alloc;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Server.Allocator.MsgManager;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.OwnLogging;
using Timer = System.Timers.Timer;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Server.Allocator.RabbitShell;

public class RabbitMqMessageProcessor(int node) : IRabbitMqMessageProcessor
{
    public INodeServerProducer NodeServerProducer { get; private set; } = null!;

    public void ConsumerCallback(byte[] message)
    {
        var len = (message[0] << 24) | (message[1] << 16) | (message[2] << 8) | message[3];
        var id = new Guid(message.Skip(4).Take(len).ToArray());

        var guidKey = message.Take(len + 4);
        message = message.Skip(len + 4).ToArray();

        var t = new Timer(250) { Enabled = true, AutoReset = false };
        t.Elapsed += (_, _) =>
        {
            var ot = new Timer(3000) { Enabled = true, AutoReset = false };
            {
                ot.Elapsed += (_, _) =>
                {
                    ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Warn,
                        $"Message {DebugInfoCollector.PacketCollectorY[(message[0] << 8) | message[1]]} unsuccessfully processed (recv)!");

                    if (t != null)
                    {
                        t.Dispose();
                        t = null;
                    }

                    if (ot == null!) return;
                    ot.Dispose();
                    ot = null;
                };
            }

            // start action
            try
            {
                if (!SessionCollector.AllocSessions.TryGetValue(id, out var value))
                {
                    SessionCollector.AllocSessions.TryAdd(id, value = new AllocSession { SessionId = id });

                    value.OwnerMessageManager = new MessageManager(value) { GuidKey = guidKey };
                    value.NodeServerProducer = NodeServerProducer;
                }

                var result = value.OwnerMessageManager.ReceiveMessage(node, message);
                ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Info,
                    $"Message {DebugInfoCollector.PacketCollectorY[(message[0] << 8) | message[1]]} successfully processed (recv)! Code: {result}.");
            }
            catch (Exception e)
            {
                ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Info,
                    $"Message {DebugInfoCollector.PacketCollectorY[(message[0] << 8) | message[1]]} unsuccessfully processed (recv)! Error: {e}.");
            }
            // end action

            if (ot == null!) return;
            ot.Stop();
            ot.Dispose();
            ot = null;

            if (t == null) return;
            ot.Stop();
            t.Dispose();
            t = null;
        };
    }

    public void AddProducer(INodeServerProducer nodeServerProducer)
    {
        NodeServerProducer = nodeServerProducer;
    }
}