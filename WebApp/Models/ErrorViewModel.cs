namespace HelloContainer.WebApp.Models;

/// <summary>
/// View model for error pages
/// </summary>
public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    
    public string? ErrorMessage { get; set; }
    
    public string? ErrorDetails { get; set; }
    
    public int StatusCode { get; set; }
}
