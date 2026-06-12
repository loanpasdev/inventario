namespace InventoryManagement.Web.Models;

public sealed class ApiAuthenticationResponse
{
    public string AccessToken { get; init; } = string.Empty;
    public string TokenType { get; init; } = string.Empty;
}
