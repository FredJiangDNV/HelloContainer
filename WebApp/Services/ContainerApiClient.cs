using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace HelloContainer.WebApp.Services;

public class ContainerApiClient
{
    private readonly HttpClient _httpClient;

    public ContainerApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ContainerDto>?> GetContainersAsync(string? searchKeyword = null)
    {
        var url = "api/containers";
        if (!string.IsNullOrEmpty(searchKeyword))
        {
            url += $"?searchKeyword={Uri.EscapeDataString(searchKeyword)}";
        }

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<ContainerDto>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<ContainerDto?> GetContainerByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"api/containers/{id}");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ContainerDto>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<ContainerDto?> CreateContainerAsync(CreateContainerDto createDto)
    {
        var json = JsonSerializer.Serialize(createDto);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            
        var response = await _httpClient.PostAsync("api/containers", content);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ContainerDto>(responseJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<ContainerDto?> AddWaterAsync(Guid id, decimal amount)
    {
        var addWaterDto = new { Amount = amount };
        var json = JsonSerializer.Serialize(addWaterDto);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            
        var response = await _httpClient.PostAsync($"api/containers/{id}/water", content);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ContainerDto>(responseJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task DeleteContainerAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/containers/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task ConnectContainersAsync(Guid sourceId, Guid targetId)
    {
        var connectDto = new { SourceContainerId = sourceId, TargetContainerId = targetId };
        var json = JsonSerializer.Serialize(connectDto);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            
        var response = await _httpClient.PostAsync("api/containers/connections", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task DisconnectContainersAsync(Guid sourceId, Guid targetId)
    {
        var disconnectDto = new { SourceContainerId = sourceId, TargetContainerId = targetId };
        var json = JsonSerializer.Serialize(disconnectDto);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            
        var response = await _httpClient.PostAsync("api/containers/disconnections", content);
        response.EnsureSuccessStatusCode();
    }
}

public class ContainerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Capacity { get; set; }
    public double Amount { get; set; }
    public List<Guid> ConnectedContainerIds { get; set; } = new();
    public double FillPercentage => Capacity > 0 ? Amount / Capacity : 0;
    public bool IsFull => FillPercentage >= 1.0;
}

public class CreateContainerDto
{
    [Required(ErrorMessage = "Container name is required")]
    [StringLength(100, ErrorMessage = "Container name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Capacity is required")]
    [Range(0.1, double.MaxValue, ErrorMessage = "Capacity must be greater than 0")]
    public decimal Capacity { get; set; }
}

