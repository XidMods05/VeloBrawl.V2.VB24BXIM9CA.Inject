using RabbitMQ.Client;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.AbstractionLayer.Interfaces;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Auxiliary.Settings;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.RabbitMqProxy.Produ;

public class NodeServerProducer(string node) : INodeServerProducer
{
    private readonly object _providerLock = new();

    public IModel Channel { get; private set; } = null!;

    public void SendMessage(byte[] message)
    {
        lock (_providerLock)
        {
            Channel.BasicPublish("", node, null, message);
        }
    }

    public void Create()
    {
        var factory = new ConnectionFactory { HostName = AppConfig.RabbitMqHost };
        var connection = factory.CreateConnection();

        Channel = connection.CreateModel();
    }
}