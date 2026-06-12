using InventoryManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InventoryManagement.Web.Pages;

public sealed class LogoutModel : PageModel
{
    public IActionResult OnPost()
    {
        HttpContext.Session.Remove(SessionKeys.AccessToken);
        HttpContext.Session.Remove(SessionKeys.Email);
        HttpContext.Session.Remove(SessionKeys.Username);
        HttpContext.Session.Remove(SessionKeys.FullName);
        HttpContext.Session.Remove(SessionKeys.Roles);
        return RedirectToPage("/Login");
    }
}
