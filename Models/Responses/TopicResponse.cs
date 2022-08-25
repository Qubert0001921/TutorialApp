namespace EmptyTest.Models.Responses;
public class TopicResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public DateTime LastModifiedDate { get; set; }
    public Guid TutorialId { get; set; }
}
