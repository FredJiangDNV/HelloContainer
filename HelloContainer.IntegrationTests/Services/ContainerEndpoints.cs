using HelloContainer.DTOs;
using HelloContainer.IntegrationTests.Fixtures;
using System.Net.Http.Json;

namespace HelloContainer.IntegrationTests.Services
{
    public static class ContainerEndpoints
    {
        public static async Task<ContainerReadDto?> CreateContainer(this HelloContainerFixture fixture, string name, double capacity)
        {
            var createDto = new CreateContainerDto(name, capacity);
            var response = await fixture.Client.PostAsJsonAsync("/api/containers", createDto);
            return await response.Content.ReadFromJsonAsync<ContainerReadDto>();
        }

        public static async Task<ContainerReadDto?> GetContainer(this HelloContainerFixture fixture, Guid id)
        {
            var response = await fixture.Client.GetAsync($"/api/containers/{id}");
            return await response.Content.ReadFromJsonAsync<ContainerReadDto>();
        }

        public static async Task<ContainerReadDto?> ConnectContainers(this HelloContainerFixture fixture, Guid c1, Guid c2)
        {
            var connectDto = new ConnectContainersDto(c1, c2);
            var response = await fixture.Client.PostAsJsonAsync("/api/containers/connections", connectDto);
            return await response.Content.ReadFromJsonAsync<ContainerReadDto>();
        }

        public static async Task<ContainerReadDto?> AddWater(this HelloContainerFixture fixture, Guid id, double amount)
        {
            var addWaterDto = new AddWaterDto(amount);
            var response = await fixture.Client.PostAsJsonAsync($"/api/containers/{id}/water", addWaterDto);

            return await response.Content.ReadFromJsonAsync<ContainerReadDto>();
        }
    }
}
