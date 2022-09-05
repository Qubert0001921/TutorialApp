using AutoMapper;
using EmptyTest.Entities;
using EmptyTest.Models.Requests.MyTutorials;
using EmptyTest.Models.Responses;

namespace EmptyTest.MappingProfiles;
public class TopicMappingProfile : Profile
{
	public TopicMappingProfile()
	{
		CreateMap<AddTopicRequest, Topic>();
		CreateMap<Topic, TopicResponse>();
	}
}
