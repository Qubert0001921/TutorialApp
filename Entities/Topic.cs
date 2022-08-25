namespace EmptyTest.Entities;
public class Topic : GuidEntity
{
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public DateTime LastModifiedDate { get; set; }
    public Guid SectionId { get; set; }
}
