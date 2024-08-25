using System.Text;
using RabbitMQ.Client;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.AbstractionLayer.Interfaces;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Auxiliary.Settings;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.RabbitMqFirewall.Out;

public class RabbitOutFirewall : IRabbitOut
{
    private readonly object _providerLock = new();

    public IModel Channel { get; private set; } = null!;

    public void SendMessage(string message)
    {
        lock (_providerLock)
        {
            Channel.BasicPublish("", "VeloBrawl.i.firewall", null, Encoding.UTF8.GetBytes(message));
        }
    }

    public void Create()
    {
        var factory = new ConnectionFactory { HostName = AppConfig.RabbitMqHost };
        var connection = factory.CreateConnection();

        Channel = connection.CreateModel();
    }
}