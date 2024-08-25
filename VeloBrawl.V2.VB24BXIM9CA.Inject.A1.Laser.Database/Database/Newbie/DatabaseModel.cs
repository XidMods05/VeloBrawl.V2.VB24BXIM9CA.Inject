using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Database.AbstractionLayer;
using Veltonsoft.BrawlStars.Velofauna.VelofaunaProjects.HelpDirectory.Marshal;

namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Database.Database.Newbie;

[Serializable]
public class DatabaseModel<T> : IDatabaseModel where T : class
{
    /// <summary>
    ///     <para>Binary instance of T class.</para>
    ///     Name in BSON: <c>_documentBinInstance</c> |
    ///     Name in JSON: <c>_documentBinInstance</c>
    /// </summary>
    [BsonElement("_documentBinInstance")]
    [JsonProperty("_documentBinInstance")]
    public byte[] DocumentBinaryInstance { get; set; } = null!;

    /// <summary>
    ///     <para>Unix-time of last registered update of T class.</para>
    ///     Name in BSON: <c>_documentLastRegUnixTime</c> |
    ///     Name in JSON: <c>_documentLastRegUnixTime</c>
    /// </summary>
    [BsonElement("_documentLastRegUnixTime")]
    [JsonProperty("_documentLastRegUnixTime")]
    public long DocumentLastRegUnixTime { get; set; } = -1;

    /// <summary>
    ///     <para>Secure property for moderation of the T class.</para>
    ///     <para>BsonIgnored field</para>
    ///     JsonIgnored field
    /// </summary>
    [MaybeNull]
    [BsonIgnore]
    [JsonIgnore]
    private T DocumentMarshal
    {
        get
        {
            if (UnSerializedDocument == null)
                DocumentMarshal = MarshalDoc.Deserialize<T>(DocumentBinaryInstance);
            return UnSerializedDocument;
        }
        set
        {
            if (value == null!) return;
            DocumentBinaryInstance = MarshalDoc.Serialize(value);
            UnSerializedDocument = value;
        }
    }

    /// <summary>
    ///     <para>Auto private-saved dynamic property of the T class.</para>
    ///     <para>BsonIgnored field</para>
    ///     JsonIgnored field
    /// </summary>
    [MaybeNull]
    [BsonIgnore]
    [JsonIgnore]
    private T UnSerializedDocument { get; set; } = null!;

    /// <summary>
    ///     Safe property of DocumentMarshal.
    /// </summary>
    [NotNull]
    [BsonIgnore]
    [JsonIgnore]
    public T Document
    {
        get => GetDocument();
        set => SetDocument(value);
    }

    /// <summary>
    ///     <para>ID of <see cref="DatabaseModel{T}" />.</para>
    ///     Name in BSON: <c>_id</c> |
    ///     Name in JSON: <c>_id</c>
    /// </summary>
    [BsonElement("_id")]
    [JsonProperty("_id")]
    public long Id { get; set; } = -1L;

    /// <summary>
    ///     Method for secure safe and regulate of T class mutations.
    /// </summary>
    public void SecureSaveOfMutations()
    {
        SetDocument(DocumentMarshal!);
        DocumentLastRegUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    /// <summary>
    ///     Disposable.
    /// </summary>
    public void Dispose()
    {
        UnSerializedDocument = null!;
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Getter of DocumentMarshal(T).
    /// </summary>
    /// <returns></returns>
    public T GetDocument()
    {
        return DocumentMarshal!;
    }

    /// <summary>
    ///     Setter of DocumentMarshal(T).
    /// </summary>
    /// <param name="document">new document</param>
    /// <returns></returns>
    public void SetDocument(T document)
    {
        DocumentMarshal = document;
    }

    /// <summary>
    ///     Destructor.
    /// </summary>
    ~DatabaseModel()
    {
        UnSerializedDocument = null!;
    }
}