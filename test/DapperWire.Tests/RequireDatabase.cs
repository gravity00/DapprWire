namespace DapperWire;

[CollectionDefinition(nameof(RequireDatabase))]
public class RequireDatabase : ICollectionFixture<DatabaseFixture>;