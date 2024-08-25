using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Auxiliary.Settings;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.RabbitMqFirewall.Lexical;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.RabbitMqFirewall.In;

public class RabbitInFirewall
{
    public IModel Channel { get; private set; } = null!;

    public void Create()
    {
        var factory = new ConnectionFactory { HostName = AppConfig.RabbitMqHost };
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare("VeloBrawl.o.firewall", true, false, false, null);
        Channel = channel;

        ConsumeMessages();
    }

    private void ConsumeMessages()
    {
        try
        {
            var consumer = new EventingBasicConsumer(Channel);
            {
                consumer.Received += (_, args) =>
                    LexicalAnalyzer.AnalyzeAndProcess(Encoding.UTF8.GetString(args.Body.ToArray()));
            }

            Channel.BasicConsume("VeloBrawl.o.firewall", true, consumer);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Thread.Sleep(1000);

            ConsumeMessages();
        }
    }
}