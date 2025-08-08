namespace HelloContainer.IntegrationTests.Fixtures;

public class HelloContainerFixture : IDisposable
{
    public HttpClient Client { get; set; }
    public Guid C_Id { get; set; }
    public Guid C2_Id { get; set; }

    public HelloContainerFixture()
    {
        Client = new HelloContainerWebApplicationFactory().CreateClient();
    }

    public void Dispose()
    {
        this.Client?.Dispose();
    }
}
