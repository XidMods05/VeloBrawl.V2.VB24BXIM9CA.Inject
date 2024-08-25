namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Database.AbstractionLayer;

public interface IDatabaseModel : IDisposable
{
    public long Id { get; set; }
    public void SecureSaveOfMutations();
}