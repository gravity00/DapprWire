namespace DapperWire;

public record TestTableEntity
{
    public int Id { get; init; }

    public Guid ExternalId { get; init; }

    public string Name { get; init; } = string.Empty;
}