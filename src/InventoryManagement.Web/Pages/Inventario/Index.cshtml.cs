using System.Net.Http.Headers;
using System.Net.Http.Json;
using InventoryManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InventoryManagement.Web.Pages.Inventario;

public sealed class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IHttpClientFactory httpClientFactory, ILogger<IndexModel> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public IReadOnlyList<ProductListItemViewModel> LowStockProducts { get; private set; } = [];

    [TempData]
    public string? StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString(SessionKeys.AccessToken)))
        {
            return RedirectToPage("/Login");
        }

        LowStockProducts = await LoadLowStockProductsAsync();
        return Page();
    }

    private async Task<IReadOnlyList<ProductListItemViewModel>> LoadLowStockProductsAsync()
    {
        var client = _httpClientFactory.CreateClient("InventoryApi");

        try
        {
            if (!AttachBearerToken(client))
            {
                StatusMessage = "Tu sesión expiró. Vuelve a iniciar sesión.";
                return [];
            }

            var products = await client.GetFromJsonAsync<List<ProductListItemViewModel>>("api/products/low-stock");
            return products ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "No fue posible obtener productos con stock bajo.");
            return [];
        }
    }

    private bool AttachBearerToken(HttpClient client)
    {
        var accessToken = HttpContext.Session.GetString(SessionKeys.AccessToken);
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return false;
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        return true;
    }
}

