namespace API.Models;

public class ExternalIdMap
{
    public Guid Id { get; private set; }

    // Where did this record come from?
    public string SourceSystem { get; private set; } = null!;   // e.g. "adorbit_dw"

    // What table/object in that system?
    public string SourceTable { get; private set; } = null!;    // e.g. "Dim Company"

    // The external system's ID (store as string to be safe)
    public string SourceId { get; private set; } = null!;

    // What entity this maps to in *your* system
    public string EntityType { get; private set; } = null!;     // e.g. "company", "order"

    // Your internal primary key
    public Guid InternalId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    // EF Core constructor
    private ExternalIdMap() { }

    // Factory constructor (recommended)
    public ExternalIdMap(
        string sourceSystem,
        string sourceTable,
        string sourceId,
        string entityType,
        Guid internalId)
    {
        Id = Guid.NewGuid();
        SourceSystem = sourceSystem;
        SourceTable = sourceTable;
        SourceId = sourceId;
        EntityType = entityType;
        InternalId = internalId;
        CreatedAt = DateTime.UtcNow;
    }
}