// Azure AD Authentication - Temporarily commented out
// using Microsoft.AspNetCore.Authentication.OpenIdConnect;
// using Microsoft.Identity.Web;
// using Microsoft.Identity.Web.UI;
using HelloContainer.WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Azure AD Authentication - Temporarily commented out
// builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
//     .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
//     .EnableTokenAcquisitionToCallDownstreamApi()
//     .AddInMemoryTokenCaches();

builder.Services.AddControllersWithViews();
    // .AddMicrosoftIdentityUI(); // Commented out

builder.Services.AddRazorPages();

// Authorization - Temporarily commented out
// builder.Services.AddAuthorization(options =>
// {
//     options.FallbackPolicy = options.DefaultPolicy;
// });

builder.Services.AddHttpClient<ContainerApiClient>(client =>
{
    var apiBaseUrl = builder.Configuration["ContainerApi:BaseUrl"];
    client.BaseAddress = new Uri(apiBaseUrl);
});

// Updated service registration without ITokenAcquisition dependency
//builder.Services.AddScoped<ContainerApiClient>(provider =>
//{
//    var httpClient = provider.GetRequiredService<HttpClient>();
//    var configuration = provider.GetRequiredService<IConfiguration>();
//    var logger = provider.GetRequiredService<ILogger<ContainerApiClient>>();
    
//    return new ContainerApiClient(httpClient, configuration, logger);
//});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Azure AD Authentication & Authorization - Temporarily commented out
// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

