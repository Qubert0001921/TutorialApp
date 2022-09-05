using EmptyTest.Helpers;
using EmptyTest.Models.Requests.Auth;
using EmptyTest.Models.Requests.Queries;
using EmptyTest.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace EmptyTest.Controllers;
public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    public IActionResult SignIn([FromQuery] SingInQuery query)
    {
        ViewData["RedirectUrl"] = query.RedirectUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(SignInRequest requestModel, [FromQuery] SingInQuery query)
    {
        if (!ModelState.IsValid) return View();

        var principal = await _authService.SignIn(requestModel);
        if (principal is null)
        {
            ViewData["SignInError"] = "Invalid email or password";
        }
        else
        {
            await this.HttpContext.SignInAsync(principal);
            return Redirect(query.RedirectUrl ?? Routes.HomePage);
        }
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterAccountRequest requestModel)
    {
        if (!ModelState.IsValid) return View();

        var result = await _authService.RegisterAccount(requestModel);
        switch (result)
        {
            case Helpers.ServiceResult.ValidationError:
                ViewData["RegisterError"] = "This email is already taken";
                break;
            case Helpers.ServiceResult.Created:
                ViewData["RegisterSuccess"] = "Successfully created an account";
                break;
        }

        return View();
    }

    public IActionResult AccountSettings()
    {
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.SignOutAsync();
        return Redirect(Routes.HomePage);
    }
}
