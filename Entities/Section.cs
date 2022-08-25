namespace EmptyTest.Entities;
public class Section : GuidEntity
{
    public Guid TutorialId { get; set; }
    public string Name { get; set; }
    public string ShortDescription { get; set; }

    public virtual List<Topic> Topics { get; set; }
}
