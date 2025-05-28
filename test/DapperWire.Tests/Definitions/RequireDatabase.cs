namespace DapperWire.Definitions;

[CollectionDefinition(nameof(RequireDatabase))]
public class RequireDatabase : ICollectionFixture<DatabaseFixture>;