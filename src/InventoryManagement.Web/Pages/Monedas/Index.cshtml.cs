using System.Net.Http.Headers;
using System.Net.Http.Json;
using InventoryManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InventoryManagement.Web.Pages.Monedas;

public sealed class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IHttpClientFactory httpClientFactory, ILogger<IndexModel> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public IReadOnlyList<CurrencyListItemViewModel> Currencies { get; private set; } = [];

    [TempData]
    public string? StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString(SessionKeys.AccessToken)))
        {
            return RedirectToPage("/Login");
        }

        Currencies = await LoadCurrenciesAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostToggleStatusAsync(int id, string currentStatus)
    {
        if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString(SessionKeys.AccessToken)))
        {
            return RedirectToPage("/Login");
        }

        var nextStatus = string.Equals(currentStatus, "Activo", StringComparison.OrdinalIgnoreCase)
            ? "Inactivo"
            : "Activo";
        var client = _httpClientFactory.CreateClient("InventoryApi");

        try
        {
            if (!AttachBearerToken(client))
            {
                StatusMessage = "Tu sesión expiró. Vuelve a iniciar sesión.";
                return RedirectToPage("/Login");
            }

            var response = await client.PutAsJsonAsync($"api/currencies/{id}/status", new { Status = nextStatus });
            if (response.IsSuccessStatusCode)
            {
                StatusMessage = "Estado de la moneda actualizado correctamente.";
                return RedirectToPage();
            }

            StatusMessage = $"No fue posible actualizar la moneda. La API respondió con estado {(int)response.StatusCode}.";
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "No fue posible actualizar el estado de la moneda.");
            StatusMessage = "No fue posible actualizar la moneda.";
        }

        return RedirectToPage();
    }

    private async Task<IReadOnlyList<CurrencyListItemViewModel>> LoadCurrenciesAsync()
    {
        var client = _httpClientFactory.CreateClient("InventoryApi");

        try
        {
            if (!AttachBearerToken(client))
            {
                StatusMessage = "Tu sesión expiró. Vuelve a iniciar sesión.";
                return [];
            }

            var currencies = await client.GetFromJsonAsync<List<CurrencyListItemViewModel>>("api/currencies");
            return currencies ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "No fue posible obtener monedas; se mostrará la tabla vacía.");
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
