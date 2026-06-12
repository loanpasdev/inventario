using System.Net.Http.Headers;
using System.Net.Http.Json;
using InventoryManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InventoryManagement.Web.Pages.Categorias;

public sealed class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IHttpClientFactory httpClientFactory, ILogger<IndexModel> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public IReadOnlyList<CategoryListItemViewModel> Categories { get; private set; } = [];

    [TempData]
    public string? StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString(SessionKeys.AccessToken)))
        {
            return RedirectToPage("/Login");
        }

        Categories = await LoadCategoriesAsync();
        return Page();
    }

    private async Task<IReadOnlyList<CategoryListItemViewModel>> LoadCategoriesAsync()
    {
        var client = _httpClientFactory.CreateClient("InventoryApi");

        try
        {
            if (!AttachBearerToken(client))
            {
                StatusMessage = "Tu sesión expiró. Vuelve a iniciar sesión.";
                return [];
            }

            var categories = await client.GetFromJsonAsync<List<CategoryListItemViewModel>>("api/categories");
            return categories ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "No fue posible obtener categorías; se mostrará la tabla vacía.");
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

