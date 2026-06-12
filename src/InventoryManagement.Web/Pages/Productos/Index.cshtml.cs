using System.Net.Http.Headers;
using System.Net.Http.Json;
using InventoryManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryManagement.Web.Pages.Productos;

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
    public IReadOnlyList<CategoryListItemViewModel> Categories { get; private set; } = [];
    public IReadOnlyList<UnitOfMeasureListItemViewModel> UnitsOfMeasure { get; private set; } = [];
    public IReadOnlyList<CurrencyListItemViewModel> Currencies { get; private set; } = [];

    public IReadOnlyList<SelectListItem> CategoryOptions =>
        Categories.Select(category => new SelectListItem(category.Name, category.Id.ToString()))
            .ToList();

    public IReadOnlyList<SelectListItem> UnitOfMeasureOptions =>
        UnitsOfMeasure.Select(unitOfMeasure => new SelectListItem(unitOfMeasure.DisplayName, unitOfMeasure.Id.ToString()))
            .ToList();

    public IReadOnlyList<SelectListItem> CurrencyOptions =>
        Currencies.Select(currency => new SelectListItem(currency.DisplayName, currency.Code))
            .ToList();

    [BindProperty]
    public CreateProductInputModel Input { get; set; } = new();

    [TempData]
    public string? StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (!IsAuthenticated())
        {
            return RedirectToPage("/Login");
        }

        await LoadPageDataAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!IsAuthenticated())
        {
            return RedirectToPage("/Login");
        }

        if (!ModelState.IsValid)
        {
            await LoadPageDataAsync();
            return Page();
        }

        var client = _httpClientFactory.CreateClient("InventoryApi");

        try
        {
            var authenticated = AttachBearerToken(client);
            if (!authenticated)
            {
                StatusMessage = "Tu sesión expiró. Vuelve a iniciar sesión.";
                return RedirectToPage("/Login");
            }

            var response = await client.PostAsJsonAsync("api/products", Input);
            if (response.IsSuccessStatusCode)
            {
                StatusMessage = "Producto registrado correctamente.";
                return RedirectToPage();
            }

            StatusMessage = $"La API respondió con estado {(int)response.StatusCode}.";
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "No fue posible registrar el producto usando la API.");
            StatusMessage = "No fue posible conectar con la API.";
        }

        await LoadPageDataAsync();
        return Page();
    }

    private async Task LoadPageDataAsync()
    {
        Products = await LoadProductsAsync();
        Categories = await LoadCategoriesAsync();
        UnitsOfMeasure = await LoadUnitsOfMeasureAsync();
        Currencies = await LoadCurrenciesAsync();
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

            var response = await client.GetAsync("api/products");
            if (!response.IsSuccessStatusCode)
            {
                StatusMessage = $"No fue posible cargar los productos. La API respondió con estado {(int)response.StatusCode}.";
                return [];
            }

            var products = await response.Content.ReadFromJsonAsync<List<ProductListItemViewModel>>();
            return products ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "No fue posible obtener productos; se mostrará la tabla vacía.");
            StatusMessage = "No fue posible cargar los productos. Verifica que la API esté levantada y con las migraciones aplicadas.";
            return [];
        }
    }

    private async Task<IReadOnlyList<CategoryListItemViewModel>> LoadCategoriesAsync()
    {
        var client = _httpClientFactory.CreateClient("InventoryApi");

        try
        {
            if (!AttachBearerToken(client))
            {
                return [];
            }

            var categories = await client.GetFromJsonAsync<List<CategoryListItemViewModel>>("api/categories");
            return categories ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "No fue posible obtener categorías; el combo quedará vacío.");
            return [];
        }
    }

    private async Task<IReadOnlyList<UnitOfMeasureListItemViewModel>> LoadUnitsOfMeasureAsync()
    {
        var client = _httpClientFactory.CreateClient("InventoryApi");

        try
        {
            if (!AttachBearerToken(client))
            {
                return [];
            }

            var unitsOfMeasure = await client.GetFromJsonAsync<List<UnitOfMeasureListItemViewModel>>("api/units-of-measure");
            return unitsOfMeasure ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "No fue posible obtener unidades de medida; el combo quedará vacío.");
            return [];
        }
    }

    private async Task<IReadOnlyList<CurrencyListItemViewModel>> LoadCurrenciesAsync()
    {
        var client = _httpClientFactory.CreateClient("InventoryApi");

        try
        {
            if (!AttachBearerToken(client))
            {
                return [];
            }

            var currencies = await client.GetFromJsonAsync<List<CurrencyListItemViewModel>>("api/currencies");
            return currencies ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "No fue posible obtener monedas; el combo quedará vacío.");
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

