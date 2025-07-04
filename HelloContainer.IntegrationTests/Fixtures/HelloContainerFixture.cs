namespace HelloContainer.IntegrationTests.Fixtures;

public class HelloContainerFixture : IDisposable
{
    public HttpClient Client { get; set; }
    public Guid ContainerAId { get; set; }
    public Guid ContainerBId { get; set; }

    public HelloContainerFixture()
    {
        Client = new HelloContainerWebApplicationFactory().CreateClient();
    }

    public void Dispose()
    {
        this.Client?.Dispose();
    }
}
