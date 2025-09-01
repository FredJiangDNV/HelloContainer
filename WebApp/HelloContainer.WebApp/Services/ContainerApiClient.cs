// using Microsoft.Identity.Web; // Temporarily commented out
using System.Net.Http.Headers;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace HelloContainer.WebApp.Services;

public class ContainerApiClient
{
    private readonly HttpClient _httpClient;
    // private readonly ITokenAcquisition _tokenAcquisition; // Temporarily commented out
    private readonly IConfiguration _configuration;
    private readonly ILogger<ContainerApiClient> _logger;

    public ContainerApiClient(
        HttpClient httpClient, 
        // ITokenAcquisition tokenAcquisition, // Temporarily commented out
        IConfiguration configuration,
        ILogger<ContainerApiClient> logger)
    {
        _httpClient = httpClient;
        // _tokenAcquisition = tokenAcquisition; // Temporarily commented out
        _configuration = configuration;
        _logger = logger;
    }

    private async Task SetAuthorizationHeaderAsync()
    {
        // Temporarily disabled - no authentication
        // var scope = _configuration["ContainerApi:Scope"];
        // var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { scope! });
        
        // _httpClient.DefaultRequestHeaders.Authorization = 
        //     new AuthenticationHeaderValue("Bearer", accessToken);
        
        await Task.CompletedTask; // Placeholder for async signature
    }

    public async Task<List<ContainerDto>?> GetContainersAsync(string? searchKeyword = null)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting containers");
            throw;
        }
    }

    public async Task<ContainerDto?> GetContainerByIdAsync(Guid id)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            
            var response = await _httpClient.GetAsync($"api/containers/{id}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ContainerDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting container {ContainerId}", id);
            throw;
        }
    }

    public async Task<ContainerDto?> CreateContainerAsync(CreateContainerDto createDto)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating container");
            throw;
        }
    }

    public async Task<ContainerDto?> AddWaterAsync(Guid id, decimal amount)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding water to container {ContainerId}", id);
            throw;
        }
    }

    public async Task DeleteContainerAsync(Guid id)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            
            var response = await _httpClient.DeleteAsync($"api/containers/{id}");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting container {ContainerId}", id);
            throw;
        }
    }
}

// DTOs matching the API
public class ContainerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Capacity { get; set; }
    public decimal CurrentVolume { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<Guid> ConnectedContainers { get; set; } = new();
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

