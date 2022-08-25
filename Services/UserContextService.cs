using System.Security.Claims;

namespace EmptyTest.Services;
public interface IUserContextService
{
	ClaimsPrincipal? User { get; }
	Claim? AccountId { get; }
}

public class UserContextService : IUserContextService
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public UserContextService(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public ClaimsPrincipal? User => _httpContextAccessor?.HttpContext?.User;
	public Claim? AccountId => this.User?.FindFirst(x => x.Type == ClaimTypes.NameIdentifier);
}
