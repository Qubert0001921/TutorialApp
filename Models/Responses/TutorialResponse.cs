namespace EmptyTest.Models.Responses;
public class TutorialResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid AccountId { get; set; }
    public decimal Price { get; set; }
    public bool IsPublic { get; set; }
    public string ImagePath { get; set; }
    public string AccountEmail { get; set; }
    public List<SectionResponse> SectionResponses { get; set; }
}
