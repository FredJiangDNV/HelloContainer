// using Microsoft.AspNetCore.Authorization; // Temporarily commented out
using Microsoft.AspNetCore.Mvc;
using HelloContainer.WebApp.Services;
using System.Diagnostics;
using HelloContainer.WebApp.Models;

namespace HelloContainer.WebApp.Controllers;

// [Authorize] // Temporarily commented out for development
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
        try
        {
            var containers = await _containerApiClient.GetContainersAsync(searchKeyword);
            ViewBag.SearchKeyword = searchKeyword;
            return View(containers ?? new List<ContainerDto>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading containers");
            ViewBag.Error = "Failed to load containers. Please try again.";
            return View(new List<ContainerDto>());
        }
    }

    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var container = await _containerApiClient.GetContainerByIdAsync(id);
            if (container == null)
            {
                return NotFound();
            }
            return View(container);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading container {ContainerId}", id);
            return NotFound();
        }
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateContainerDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var container = await _containerApiClient.CreateContainerAsync(model);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating container");
            ModelState.AddModelError("", "Failed to create container. Please try again.");
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddWater(Guid id, decimal amount)
    {
        try
        {
            await _containerApiClient.AddWaterAsync(id, amount);
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding water to container {ContainerId}", id);
            TempData["Error"] = "Failed to add water. Please try again.";
            return RedirectToAction(nameof(Details), new { id });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _containerApiClient.DeleteContainerAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting container {ContainerId}", id);
            TempData["Error"] = "Failed to delete container. Please try again.";
            return RedirectToAction(nameof(Index));
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

