using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Server.Allocator.Alloc;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.OwnLogging;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.RabbitMqFirewall.Lexical;

public static class LexicalAnalyzer
{
    public static void AnalyzeAndProcess(string input)
    {
        try
        {
            var tokens = input.Split(' ');

            switch (tokens[0])
            {
                case "CLOSE":
                {
                    var guid = new Guid(tokens[1]);

                    var session = SessionCollector.AllocSessions.TryGetValue(guid, out var value);
                    if (!session) return;

                    value!.Close(true);
                    break;
                }

                case "BAN_C1":
                {
                    break;
                }

                case "BAN_C2":
                {
                    break;
                }

                case "BAN_D":
                {
                    break;
                }

                case "UNBAN_C1":
                {
                    break;
                }

                case "UNBAN_C2":
                {
                    break;
                }

                case "UNBAN_D":
                {
                    break;
                }
            }
        }
        catch (Exception e)
        {
            ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Error, e);
        }
    }
}