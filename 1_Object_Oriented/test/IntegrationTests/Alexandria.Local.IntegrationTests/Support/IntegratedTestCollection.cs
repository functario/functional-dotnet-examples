namespace Alexandria.Local.IntegrationTests.Support;

#pragma warning disable IDE0059 // Unnecessary assignment of a value

[CollectionDefinition(nameof(IntegratedTests))]
public class IntegratedTestCollection : ICollectionFixture<IntegratedTests>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

#pragma warning restore IDE0059 // Unnecessary assignment of a value
