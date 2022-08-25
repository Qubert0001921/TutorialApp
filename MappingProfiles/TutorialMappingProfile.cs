using AutoMapper;
using EmptyTest.Entities;
using EmptyTest.Models.Requests.MyTutorials;
using EmptyTest.Models.Responses;

namespace EmptyTest.MappingProfiles;
public class TutorialMappingProfile : Profile
{
	public TutorialMappingProfile()
	{
		CreateMap<CreateTutorialRequest, Tutorial>();
		CreateMap<Tutorial, TutorialResponse>()
			.ForMember(x => x.AccountEmail, xx => xx.MapFrom(d => d.Account.Email));
	}
}
