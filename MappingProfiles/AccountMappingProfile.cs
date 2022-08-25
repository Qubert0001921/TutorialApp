using AutoMapper;
using EmptyTest.Entities;
using EmptyTest.Models.Requests.Auth;

namespace EmptyTest.MappingProfiles;
public class AccountMappingProfile : Profile
{
	public AccountMappingProfile()
	{
		CreateMap<RegisterAccountRequest, Account>();
	}
}
