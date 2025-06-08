namespace DapprWire;

[CollectionDefinition(nameof(RequireDatabase))]
public class RequireDatabase : ICollectionFixture<DatabaseFixture>;