using AutoMapper;
using EmptyTest.Entities;
using EmptyTest.Models.Requests.MyTutorials;
using EmptyTest.Models.Responses;

namespace EmptyTest.MappingProfiles;
public class SectionMappingProfile : Profile
{
	public SectionMappingProfile()
	{
		CreateMap<AddSectionRequest, Section>();
		CreateMap<Section, SectionResponse>()
			.ForMember(x => x.TutorialTitle, x => x.MapFrom(xx => xx.Tutorial.Name));
	}
}
