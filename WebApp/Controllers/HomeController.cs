using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HelloContainer.WebApp.Services;

namespace HelloContainer.WebApp.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ContainerApiClient _containerApiClient;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ContainerApiClient containerApiClient, ILogger<HomeController> logger)
    {
        _containerApiClient = containerApiClient;
        _logger = logger;
    }

    public async Task<IActionResult> Index(string? searchKeyword)
    {
        var containers = await _containerApiClient.GetContainersAsync(searchKeyword);
        ViewBag.SearchKeyword = searchKeyword;
        return View(containers ?? new List<ContainerDto>());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateContainerDto model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Invalid container data. Please check your input.";
            return RedirectToAction(nameof(Index));
        }

        var container = await _containerApiClient.CreateContainerAsync(model);
        TempData["Success"] = "Container created successfully!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddWater(Guid id, decimal amount)
    {
        await _containerApiClient.AddWaterAsync(id, amount);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _containerApiClient.DeleteContainerAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Connect(Guid sourceId, Guid targetId)
    {
        await _containerApiClient.ConnectContainersAsync(sourceId, targetId);
        TempData["Success"] = "Containers connected successfully!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Disconnect(Guid sourceId, Guid targetId)
    {
        await _containerApiClient.DisconnectContainersAsync(sourceId, targetId);
        TempData["Success"] = "Containers disconnected successfully!";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}

