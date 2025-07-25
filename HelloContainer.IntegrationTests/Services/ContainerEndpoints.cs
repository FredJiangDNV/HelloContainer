using HelloContainer.DTOs;
using HelloContainer.IntegrationTests.Fixtures;
using System.Net.Http.Json;

namespace HelloContainer.IntegrationTests.Services
{
    public static class ContainerEndpoints
    {
        public static async Task<HttpResponseMessage> CreateContainer(this HelloContainerFixture fixture, string name, double capacity)
        {
            var createDto = new CreateContainerDto(name, capacity);
            return await fixture.Client.PostAsJsonAsync("/api/containers", createDto);
        }
    }
}
