using Xunit;

namespace IntegrationTests;

[CollectionDefinition(Name)]
public class TestCollection: ICollectionFixture<TestFactory>
{
    public const string Name = "TestContainers";
}