namespace EmptyTest.Models.Responses;
public class SectionResponse
{
    public Guid Id { get; set; }
    public Guid TutorialId { get; set; }
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public List<TopicResponse> Topics { get; set; }
}
