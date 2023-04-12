using System.Net;
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
    
    public async virtual Task InitializeAsync()
    {
        await WaitServiceReadyAsync();
    }

    public virtual Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
    
    private async Task WaitServiceReadyAsync()
    {
        using var client = Factory.CreateClient();

        var healthy = false;
        var ready = false;
        const int maxRetries = 30;
        var retryCount = 0;
        while ((!healthy || !ready) && retryCount < maxRetries)
        {
            await Task.Delay(1000);

            var healthResult = await client.GetAsync("/health");
            var readyResult = await client.GetAsync("/ready");
            healthy = healthResult.StatusCode == HttpStatusCode.OK;
            ready = readyResult.StatusCode == HttpStatusCode.OK;
            
            retryCount++;
            if ((!healthy || !ready) && retryCount == maxRetries)
            {
                TestOutputHelper.WriteLine(await healthResult.Content.ReadAsStringAsync());
                TestOutputHelper.WriteLine(await readyResult.Content.ReadAsStringAsync());
                
                throw new Exception($"Service not ready after {maxRetries} seconds.");
            }
        }
    }
}