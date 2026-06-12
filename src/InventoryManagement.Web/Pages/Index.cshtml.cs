using System.Net.Http.Headers;
using System.Net.Http.Json;
using InventoryManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InventoryManagement.Web.Pages;

public sealed class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IHttpClientFactory httpClientFactory, ILogger<IndexModel> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public IReadOnlyList<ProductListItemViewModel> Products { get; private set; } = [];
    public IReadOnlyList<InventoryValueByCategoryViewModel> InventoryValuesByCategory { get; private set; } = [];

    [TempData]
    public string? StatusMessage { get; set; }

    public decimal TotalInventoryValue => InventoryValuesByCategory.Sum(item => item.TotalInventoryValue);
    public decimal UsdToVesRate => Products
        .Select(product => product.UsdToVesRate)
        .FirstOrDefault(rate => rate > 0);

    public decimal TotalInventoryValueUsd => UsdToVesRate <= 0
        ? 0
        : TotalInventoryValue / UsdToVesRate;
    public int TotalProducts => Products.Count;
    public int LowStockProducts => Products.Count(product => product.Stock < product.MinimumStock);
    public int DistinctCategories => Products
        .Select(product => product.Category)
        .Where(category => !string.IsNullOrWhiteSpace(category))
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .Count();

    public async Task<IActionResult> OnGetAsync()
    {
        if (!IsAuthenticated())
        {
            return RedirectToPage("/Login");
        }

        Products = await LoadProductsAsync();
        InventoryValuesByCategory = await LoadInventoryValueByCategoryAsync();
        return Page();
    }

    private async Task<IReadOnlyList<ProductListItemViewModel>> LoadProductsAsync()
    {
        var client = _httpClientFactory.CreateClient("InventoryApi");

        try
        {
            if (!AttachBearerToken(client))
            {
                StatusMessage = "Tu sesión expiró. Vuelve a iniciar sesión.";
                return [];
            }

            var products = await client.GetFromJsonAsync<List<ProductListItemViewModel>>("api/products");
            return products ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "No fue posible obtener los productos para el dashboard.");
            StatusMessage = "No fue posible cargar el dashboard. Verifica la API.";
            return [];
        }
    }

    private async Task<IReadOnlyList<InventoryValueByCategoryViewModel>> LoadInventoryValueByCategoryAsync()
    {
        var client = _httpClientFactory.CreateClient("InventoryApi");

        try
        {
            if (!AttachBearerToken(client))
            {
                return [];
            }

            var values = await client.GetFromJsonAsync<List<InventoryValueByCategoryViewModel>>("api/products/inventory-value-by-category");
            return values ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "No fue posible obtener el valor del inventario por categoría.");
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

    private bool IsAuthenticated()
    {
        return !string.IsNullOrWhiteSpace(HttpContext.Session.GetString(SessionKeys.AccessToken));
    }
}
