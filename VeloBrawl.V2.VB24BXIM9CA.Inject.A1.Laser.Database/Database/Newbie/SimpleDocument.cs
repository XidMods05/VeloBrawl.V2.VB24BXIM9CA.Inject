using System.Diagnostics.CodeAnalysis;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Database.Database.Newbie;

[Serializable]
[SuppressMessage("ReSharper", "UnusedMember.Global")] // because it's used in tests
public class SimpleDocument
{
    public string Name = string.Empty;

    public dynamic Simple = new int[1];
    public Stack<int> StackOfInt = new();

    public long Id { get; set; }
    public string Text { get; set; } = null!;

    // and etc.
}