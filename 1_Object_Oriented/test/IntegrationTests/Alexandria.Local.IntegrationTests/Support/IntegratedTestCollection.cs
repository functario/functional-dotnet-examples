namespace Alexandria.Local.IntegrationTests.Support;

[CollectionDefinition(nameof(IntegratedTests))]
public class IntegratedTestCollection : ICollectionFixture<IntegratedTests>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

// Empty class for reference
public class IntegratedTests { }
