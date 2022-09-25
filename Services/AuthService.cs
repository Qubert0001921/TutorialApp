using AutoMapper;
using EmptyTest.Entities;
using EmptyTest.Exceptions;
using EmptyTest.Models.Requests.Auth;
using EmptyTest.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EmptyTest.Services;
public interface IAuthService
{
    Task<ClaimsPrincipal?> SignIn(SignInRequest requestModel);
    Task RegisterAccount(RegisterAccountRequest requestModel);
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

    public async Task RegisterAccount(RegisterAccountRequest requestModel)
    {
        var existingAccount = await _accountRepository.FindByEmailAsync(requestModel.Email);
        if (existingAccount is not null)
        {
            throw new BadRequestException("Account at this email already exists");
        }

        var account = _mapper.Map<Account>(requestModel);
        var hasherPassword = _passwordHasher.HashPassword(account, requestModel.Password);
        account.PasswordHash = hasherPassword;

        await _accountRepository.CreateAsync(account);
    }

    public async Task<ClaimsPrincipal?> SignIn(SignInRequest requestModel)
    {
        var account = await _accountRepository.FindByEmailAsync(requestModel.Email);
        if (account is null)
        {
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(account, account.PasswordHash, requestModel.Password);
        if (result != PasswordVerificationResult.Success)
        {
            return null;
        }

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
            new Claim(ClaimTypes.Email, account.Email)
        };

        var identity = new ClaimsIdentity(claims, "Cookie");
        var principal = new ClaimsPrincipal(identity);
        return principal;
    }
}
