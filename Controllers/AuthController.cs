using EmptyTest.Helpers;
using EmptyTest.Models.Requests.Auth;
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

    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(SignInRequest requestModel)
    {
        if (!ModelState.IsValid) return View();

        var (result, principal) = await _authService.SignIn(requestModel);
        if (result == Helpers.ServiceResult.ValidationError)
        {
            ViewData["SignInError"] = "Invalid email or password";
        }
        else if (result == Helpers.ServiceResult.Success && principal is not null)
        {
            await this.HttpContext.SignInAsync(principal);
            return Redirect(Routes.HomePage);
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
