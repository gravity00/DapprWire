namespace DapprWire;

public record TestTableEntity
{
    public int Id { get; set; }

    public Guid ExternalId { get; set; }

    public string Name { get; set; } = string.Empty;
}