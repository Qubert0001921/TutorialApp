using AutoMapper;
using EmptyTest.Entities;
using EmptyTest.Helpers;
using EmptyTest.Models.Requests.Auth;
using EmptyTest.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EmptyTest.Services;
public interface IAuthService
{
    Task<(ServiceResult, ClaimsPrincipal?)> SignIn(SignInRequest requestModel);
    Task<ServiceResult> RegisterAccount(RegisterAccountRequest requestModel);
}

public class AuthService : IAuthService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<Account> _passwordHasher;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IAccountRepository accountRepository, IMapper mapper, IPasswordHasher<Account> passwordHasher, IHttpContextAccessor httpContextAccessor)
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResult> RegisterAccount(RegisterAccountRequest requestModel)
    {
        var existingAccount = await _accountRepository.FindByEmailAsync(requestModel.Email);
        if (existingAccount is not null)
        {
            return ServiceResult.ValidationError;
        }

        var account = _mapper.Map<Account>(requestModel);
        var hasherPassword = _passwordHasher.HashPassword(account, requestModel.Password);
        account.PasswordHash = hasherPassword;

        await _accountRepository.CreateAsync(account);
        return ServiceResult.Created;
    }

    public async Task<(ServiceResult, ClaimsPrincipal?)> SignIn(SignInRequest requestModel)
    {
        var account = await _accountRepository.FindByEmailAsync(requestModel.Email);
        if (account is null)
        {
            return (ServiceResult.ValidationError, null);
        }

        var result = _passwordHasher.VerifyHashedPassword(account, account.PasswordHash, requestModel.Password);
        if (result != PasswordVerificationResult.Success)
        {
            return (ServiceResult.ValidationError, null);
        }

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
            new Claim(ClaimTypes.Email, account.Email)
        };

        var identity = new ClaimsIdentity(claims, "Cookie");
        var principal = new ClaimsPrincipal(identity);
        return (ServiceResult.Success, principal);
    }
}
