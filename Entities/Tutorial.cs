namespace EmptyTest.Entities;
public class Tutorial : GuidEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid AccountId { get; set; }
    public decimal Price { get; set; }
    public bool IsPublic { get; set; }
    public string ImagePath { get; set; }

    public virtual Account Account { get; set; }
    public virtual List<Section> Sections { get; set; }
}
