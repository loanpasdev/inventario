using System.Net.Http.Json;
using System.Text.Json;
using InventoryManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InventoryManagement.Web.Pages;

public sealed class LoginModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(IHttpClientFactory httpClientFactory, ILogger<LoginModel> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    [BindProperty]
    public LoginInputModel Input { get; set; } = new();

    [TempData]
    public string? StatusMessage { get; set; }

    public IActionResult OnGet()
    {
        if (!string.IsNullOrWhiteSpace(HttpContext.Session.GetString(SessionKeys.AccessToken)))
        {
            return RedirectToPage("/Index");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var client = _httpClientFactory.CreateClient("InventoryApi");

        try
        {
            var response = await client.PostAsJsonAsync("api/auth/login", new
            {
                Username = Input.Email,
                Input.Password
            });

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Correo electrónico o contraseña inválidos.");
                return Page();
            }

            var token = await response.Content.ReadFromJsonAsync<ApiAuthenticationResponse>();
            if (token is null || string.IsNullOrWhiteSpace(token.AccessToken))
            {
                ModelState.AddModelError(string.Empty, "La API no devolvio un token valido.");
                return Page();
            }

            HttpContext.Session.SetString(SessionKeys.AccessToken, token.AccessToken);
            HttpContext.Session.SetString(SessionKeys.Email, Input.Email);

            var (fullName, username, roles) = ReadUserInfoFromJwt(token.AccessToken);
            HttpContext.Session.Remove(SessionKeys.Username);
            HttpContext.Session.Remove(SessionKeys.FullName);
            HttpContext.Session.Remove(SessionKeys.Roles);
            if (!string.IsNullOrWhiteSpace(username))
            {
                HttpContext.Session.SetString(SessionKeys.Username, username);
            }

            if (!string.IsNullOrWhiteSpace(fullName))
            {
                HttpContext.Session.SetString(SessionKeys.FullName, fullName);
            }

            if (roles.Length > 0)
            {
                HttpContext.Session.SetString(SessionKeys.Roles, string.Join(',', roles));
            }

            StatusMessage = "Sesion iniciada correctamente.";
            return RedirectToPage("/Index");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "No fue posible completar el login contra la API.");
            ModelState.AddModelError(string.Empty, "No fue posible conectar con la API.");
            return Page();
        }
    }

    private static (string? FullName, string? Username, string[] Roles) ReadUserInfoFromJwt(string accessToken)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return (null, null, Array.Empty<string>());
        }

        var parts = accessToken.Split('.', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
        {
            return (null, null, Array.Empty<string>());
        }

        try
        {
            var payloadBytes = Base64UrlDecode(parts[1]);
            using var document = JsonDocument.Parse(payloadBytes);

            string? fullName = null;
            string? username = null;
            if (document.RootElement.TryGetProperty("full_name", out var fullNameElement))
            {
                fullName = fullNameElement.GetString();
            }

            if (document.RootElement.TryGetProperty("name", out var usernameElement))
            {
                username = usernameElement.GetString();
            }

            if (string.IsNullOrWhiteSpace(username)
                && document.RootElement.TryGetProperty("sub", out var subElement))
            {
                username = subElement.GetString();
            }

            var roles = new List<string>();
            if (document.RootElement.TryGetProperty("role", out var roleElement))
            {
                if (roleElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in roleElement.EnumerateArray())
                    {
                        var value = item.GetString();
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            roles.Add(value);
                        }
                    }
                }
                else if (roleElement.ValueKind == JsonValueKind.String)
                {
                    var value = roleElement.GetString();
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        roles.Add(value);
                    }
                }
            }

            return (fullName, username, roles.Distinct(StringComparer.OrdinalIgnoreCase).ToArray());
        }
        catch
        {
            return (null, null, Array.Empty<string>());
        }
    }

    private static byte[] Base64UrlDecode(string value)
    {
        var base64 = value.Replace('-', '+').Replace('_', '/');
        var padding = base64.Length % 4;
        if (padding == 2)
        {
            base64 += "==";
        }
        else if (padding == 3)
        {
            base64 += "=";
        }

        return Convert.FromBase64String(base64);
    }
}
