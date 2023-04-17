using AutoFixture;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests;

public abstract class TestBase : IAsyncLifetime
{
    protected readonly TestFactory Factory;
    protected readonly ITestOutputHelper TestOutputHelper;

    protected static IFixture AutoFixture
    {
        get
        {
            var fixture = new Fixture();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            return fixture;
        }
    }

    protected TestBase(TestFactory factory, ITestOutputHelper testOutputHelper)
    {
        Factory = factory;
        TestOutputHelper = testOutputHelper;
    }
    
    public virtual Task InitializeAsync()
    {
        return Task.FromResult(Task.CompletedTask);
    }

    public virtual Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}