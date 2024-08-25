using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.RabbitMqProxy.Consu;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.RabbitMqProxy.Produ;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Server.Allocator.RabbitShell;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.OwnLogging;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.OwnLogging.MiniHelper;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.RabbitMqProxy;

public static class Program
{
    private static readonly int[] Nodes = [1, 3, 4, 6, 7, 9, 10, 11, 13, 23, 25, 26, 27, 30, 33, 57];

    public static void Main(string[] args)
    {
        var ddd = Nodes.ToDictionary(node => node, node => new RabbitMqMessageProcessor(node));

        foreach (var node in Nodes)
        {
            var c = new NodeServerConsumer($"VeloBrawl.i.node_{node}"); // injected

            c.Create();
            c.ConsumeMessages((_, ea) => ddd[node].ConsumerCallback(ea.Body.ToArray()));

            ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Debug, $"Starting consumer for node {node}!");
        }

        Thread.Sleep(1000);
        ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Cmd,
            "Waiting for Laser.RabbitMqProxy normalization...");
        if ((int)AppConfigurator.InAppGlobalLogLevel <= 1) TextProgressBarGroundDrawer.DrawTextProgressBar(8, 85, 100);
        Thread.Sleep(1500);

        foreach (var node in Nodes)
        {
            var p = new NodeServerProducer($"VeloBrawl.c.node_{node}"); // core

            p.Create();
            ddd[node].AddProducer(p);

            ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Debug, $"Starting producer for node {node}!");
        }

        if ((int)AppConfigurator.InAppGlobalLogLevel <= 1) TextProgressBarGroundDrawer.DrawTextProgressBar(8, 90, 100);
        ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Cmd, "Hi new project! <- Laser.RabbitMqProxy: Hello!");
    }
}