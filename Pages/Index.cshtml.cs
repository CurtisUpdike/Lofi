using Lofi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lofi.Pages;

public class IndexModel : PageModel
{
    private readonly ITokenProvider _tokenProvider;

    [BindProperty]
    public string Token { get; set; } = string.Empty;

    public IndexModel(ITokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    public void OnGet()
    {
        Token = _tokenProvider.Token;
    }
}
