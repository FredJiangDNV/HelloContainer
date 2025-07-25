using HelloContainer.DTOs;
using HelloContainer.IntegrationTests.Fixtures;
using System.Net.Http.Json;

namespace HelloContainer.IntegrationTests.Services
{
    public static class AlertEndpoints
    {
        public static async Task<IEnumerable<AlertReadDto>?> GetAlerts(this HelloContainerFixture fixture, Guid containerId)
        {
            var response = await fixture.Client.GetAsync($"/api/alerts/{containerId}");
            return await response.Content.ReadFromJsonAsync<IEnumerable<AlertReadDto>>();
        }
    }
}
